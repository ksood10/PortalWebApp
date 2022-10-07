using Microsoft.AspNetCore.SignalR;
using PortalWebApp.Areas.Utilities;
using PortalWebApp.Data;
using PortalWebApp.Hubs;
using PortalWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace PortalWebApp.Utilities
{
    public class BulkConfiguratorQueue
    {
        private readonly System.Text.StringBuilder sb = new System.Text.StringBuilder();
        private List<Organization> myUserOrganizations;
        private  List<TankConfig> myTankConfigs;
        private  TankConfig myTankConfig;
        private  FileWriter statusReportWriter;
        private readonly bool checkRTUCondition;
        private bool wroteErrorFile;
        

        #region Properties

        internal string ConnectionString { get; set; }
        internal string StatusFilePath { get; set; }
        internal string FileName { get; set; }
        internal string ErrorFilePath { get; set; }
        internal string ErrorFileName { get; set; }
        internal string StatusFileName { get; set; }
        internal string OleDBConnectionString { get; set; }
        public int TotalEXCELCount { get; set; }
        public int CurrentEXCELCount { get; set; }

        internal int RecordThrottle { get; set; }

        internal int ThrottleAmount { get; set; }

        internal int UserID { get; set; }

        internal int OrganizationID { get; set; }

        internal int UserOrganizationID { get; set; }

        public bool HaveError { get; set; }

        public bool HaveEXCELReadError { get; set; }

        internal bool InvalidUser { get; set; }

        internal bool SuperUser { get; set; }

        internal string StatusMessage { get; set; }

        public PortalWebAppContext DatabaseContext { get; }

        #endregion

        public BulkConfiguratorQueue()
        {
        }

        public BulkConfiguratorQueue(string conn, string excelfilename, int userid, int recordthrottle, int throttleamount, bool checkrtu , IHubContext<ProgressHub> _notificationHubContext)
        {
            this.ConnectionString = conn;
            this.FileName = excelfilename;
            this.UserID = userid;
            this.RecordThrottle = recordthrottle;
            this.ThrottleAmount = throttleamount;
            this.checkRTUCondition = checkrtu;
            this.ErrorFilePath = AppDomain.CurrentDomain.BaseDirectory + "ErrorFile";
            this.ErrorFileName = this.ErrorFilePath + "\\" + "Errors_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
            this.StatusFilePath = AppDomain.CurrentDomain.BaseDirectory + "SummaryFile";
            this.StatusFileName = this.StatusFilePath + "\\" + "Summary_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".txt";
            DeleteOldReportFiles();
            GetUserOrganization();
            if (!this.HaveError)
            {
                if (!this.SuperUser)
                    BuildListOfUserOrganizations();
                ReadEXCELFile();
                if (!this.HaveEXCELReadError)
                {
                    // MessageBox.Show("# of EXCEL Records To Process: " + myTankConfigs.Count.ToString());
                    BulkUpdate.TotalRows = myTankConfigs.Count;
                    this.TotalEXCELCount = myTankConfigs.Count;
                    
                    //myThread = new Thread(new ThreadStart(ValidateTheEXCELFile));
                    ValidateTheEXCELFile(_notificationHubContext);
                }
            }
        }

        private void DeleteOldReportFiles()
        {
            try
            {
                bool directoryExists = Directory.Exists(this.ErrorFilePath);
                if (!directoryExists)
                    Directory.CreateDirectory(this.ErrorFilePath);
                FileInfo errorReportFile = new FileInfo(this.ErrorFileName);
                if (errorReportFile.Exists)
                    errorReportFile.Delete();
                directoryExists = Directory.Exists(this.StatusFilePath);
                if (!directoryExists)
                    Directory.CreateDirectory(this.StatusFilePath);
                FileInfo statusReportFile = new FileInfo(this.StatusFileName);
                if (statusReportFile.Exists)
                    statusReportFile.Delete();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        private void GetUserOrganization()
        {
            bool hasglobalorgsecurity = false;
            try
            {
                var userList = (from user in DatabaseContext.User
                                where user.UserId == this.UserID
                                orderby user.UserId
                                select new
                                {
                                    user.OrganizationID,
                                    user.HasGlobalOrgSecurity
                                })
                                .ToList();

                if (userList.Count > 0)
                {
                    this.UserOrganizationID = userList.First().OrganizationID;
                    hasglobalorgsecurity = userList.First().HasGlobalOrgSecurity;
                }
                else
                {
                    this.InvalidUser = true;
                    this.HaveError = true;
                }
                if (this.UserOrganizationID == 10 && hasglobalorgsecurity)
                    this.SuperUser = true;

            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(ErrorFileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at GetUserOrganization - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        private void BuildListOfUserOrganizations()
        {
            try
            {
                myUserOrganizations = new List<Organization>();
             
                var userList = (from orgTree in DatabaseContext.OrganizationTree
                                where orgTree.OrgID == this.UserOrganizationID
                                select new { orgTree.ChildOrgID }).ToList();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(ErrorFileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at BuildListOfUserOrganizations - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        private void ReadEXCELFile()
        {
            try
            {
                bool wrongFileExtension = false;
                var filename = Path.GetFileName(FileName);
                var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(MainPath))
                    Directory.CreateDirectory(MainPath);
                //get file path 
                var filePath = Path.Combine(MainPath, filename);


               

                string conString = string.Empty;
                switch (Path.GetExtension(filename))
                {
                    case ".xls": //Excel 97-03.
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                    default:
                        wrongFileExtension = true;
                        break;
                }
                if (wrongFileExtension) { this.HaveEXCELReadError = true; }
                else
                {
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);
                    using (var connExcel = new OleDbConnection(conString))
                    {
                        using OleDbCommand cmdExcel = new OleDbCommand();
                        using OleDbDataAdapter odaExcel = new OleDbDataAdapter();
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }


                    if (dt == null)
                    {
                        this.HaveEXCELReadError = true;
                    }
                    else
                        this.HaveEXCELReadError = false;
                    //if (!this.HaveEXCELReadError)
                    //{
                    int tankIDOrdinal = dt.Columns["TankID"].Ordinal;
                    int tankNameOrdinal = dt.Columns["TankName"].Ordinal;
                    int rtuNumberOrdinal = dt.Columns["RTUNumber"].Ordinal;
                    int tankHgtOrdinal = dt.Columns["TankHgt"].Ordinal;
                    int tankCapOrdinal = dt.Columns["TankCap"].Ordinal;
                    int capacityLimitOrdinal = dt.Columns["CapacityLimit"].Ordinal;
                    int tankMinimumOrdinal = dt.Columns["TankMinimum"].Ordinal;
                    int reorderUsageOrdinal = dt.Columns["ReOrderUsage"].Ordinal;
                    int safetyStockUsageOrdinal = dt.Columns["SafetyStockUsage"].Ordinal;
                    int lowLowLevelOrdinal = dt.Columns["LowLowLevel"].Ordinal;
                    int lowLevelOrdinal = dt.Columns["LowLevel"].Ordinal;
                    int highLevelOrdinal = dt.Columns["HighLevel"].Ordinal;
                    int highHighLevelOrdinal = dt.Columns["HighHighLevel"].Ordinal;
                    int fillDetectDeltaOrdinal = dt.Columns["FillDetectDelta"].Ordinal;
                    int shortFillDeltaOrdinal = dt.Columns["ShortFillDelta"].Ordinal;
                    int volumeDeltaOrdinal = dt.Columns["VolumeDelta"].Ordinal;
                    int rateChangeDeltaOrdinal = dt.Columns["RateChangeDelta"].Ordinal;
                    int callsperdayOrdinal = dt.Columns["Callsperday"].Ordinal;
                    int callDayOrdinal = dt.Columns["CallDay"].Ordinal;
                    int intervalOrdinal = dt.Columns["Interval"].Ordinal;
                    int diagCallDayMaskOrdinal = dt.Columns["DiagCallDayMask"].Ordinal;
                    int dataLogDeltaOrdinal = dt.Columns["DataLogDelta"].Ordinal;
                    int usageDeltaOrdinal = dt.Columns["UsageDelta"].Ordinal;
                    int wakeIntervalOrdinal = dt.Columns["WakeInterval"].Ordinal;
                    int startTimeOrdinal = dt.Columns["StartTime"].Ordinal;
                    int highSetPointOrdinal = dt.Columns["HighSetPoint"].Ordinal;
                    int lowSetPointOrdinal = dt.Columns["LowSetPoint"].Ordinal;
                    int sensorOffsetOrdinal = dt.Columns["SensorOffset"].Ordinal;
                    int coeffExpOrdinal = dt.Columns["CoeffExp"].Ordinal;
                    int specGravOrdinal = dt.Columns["SpecGrav"].Ordinal;
                    int deviceFillDetectDeltaOrdinal = dt.Columns["DeviceFillDetectDelta"].Ordinal;
                    int deviceFillHysteresisOrdinal = dt.Columns["DeviceFillHysteresis"].Ordinal;
                    int deviceCriticalLowLevelOrdinal = dt.Columns["DeviceCriticalLowLevel"].Ordinal;
                    int deviceLowLevelOrdinal = dt.Columns["DeviceLowLevel"].Ordinal;
                    int deviceHighLevelOrdinal = dt.Columns["DeviceHighLevel"].Ordinal;
                    int deviceCriticalHighLevelOrdinal = dt.Columns["DeviceCriticalHighLevel"].Ordinal;
                    int deviceFillDetectOrdinal = dt.Columns["DeviceFillDetect"].Ordinal;
                    int deviceUsageAlarmOrdinal = dt.Columns["DeviceUsageAlarm"].Ordinal;
                    int hasExpectedCallAlarmOrdinal = dt.Columns["HasExpectedCallAlarm"].Ordinal;
                    int tankNormallyFillsOrdinal = dt.Columns["TankNormallyFills"].Ordinal;
                    //    //new code added to accomodate EnableLocation and EnableGPS+
                    //    //D Arcilla Oct 2020
                    int enableLocationOrdinal = dt.Columns["EnableLocation"].Ordinal;
                    int enableGPSOrdinal = dt.Columns["EnableGPS"].Ordinal;
                    if (dt.Rows.Count > 0)
                    {
                        myTankConfigs = new List<TankConfig>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            myTankConfig = new TankConfig(this.ConnectionString, this.UserID)
                            {
                                ErrorFilePath = this.ErrorFilePath,
                                CheckRTUCondition = checkRTUCondition
                            };
                            if (dr[tankIDOrdinal] == DBNull.Value)
                                myTankConfig.TankID = "*** Empty ***";
                            else
                                myTankConfig.TankID = dr[tankIDOrdinal].ToString();
                            if (dr[rtuNumberOrdinal] == DBNull.Value)
                                myTankConfig.RTUNumber = "*** Empty ***";
                            else
                                myTankConfig.RTUNumber = dr[rtuNumberOrdinal].ToString();
                            if (dr[tankNameOrdinal] == DBNull.Value)
                                myTankConfig.TankName = "*** Empty ***";
                            else
                                myTankConfig.TankName = dr[tankNameOrdinal].ToString();
                            if (dr[tankHgtOrdinal] == DBNull.Value)
                                myTankConfig.TankHgt = "*** Empty ***";
                            else
                                myTankConfig.TankHgt = dr[tankHgtOrdinal].ToString();
                            if (dr[tankCapOrdinal] == DBNull.Value)
                                myTankConfig.TankCap = "*** Empty ***";
                            else
                                myTankConfig.TankCap = dr[tankCapOrdinal].ToString();
                            if (dr[capacityLimitOrdinal] == DBNull.Value)
                                myTankConfig.CapacityLimit = "*** Empty ***";
                            else
                                myTankConfig.CapacityLimit = dr[capacityLimitOrdinal].ToString();
                            if (dr[tankMinimumOrdinal] == DBNull.Value)
                                myTankConfig.TankMinimum = "*** Empty ***";
                            else
                                myTankConfig.TankMinimum = dr[tankMinimumOrdinal].ToString();
                            if (dr[reorderUsageOrdinal] == DBNull.Value)
                                myTankConfig.ReorderUsage = "*** Empty ***";
                            else
                                myTankConfig.ReorderUsage = dr[reorderUsageOrdinal].ToString();
                            if (dr[safetyStockUsageOrdinal] == DBNull.Value)
                                myTankConfig.SafetyStockUsage = "*** Empty ***";
                            else
                                myTankConfig.SafetyStockUsage = dr[safetyStockUsageOrdinal].ToString();
                            if (dr[lowLowLevelOrdinal] == DBNull.Value)
                                myTankConfig.LowLowLevel = "*** Empty ***";
                            else
                                myTankConfig.LowLowLevel = dr[lowLowLevelOrdinal].ToString();
                            if (dr[lowLevelOrdinal] == DBNull.Value)
                                myTankConfig.LowLevel = "*** Empty ***";
                            else
                                myTankConfig.LowLevel = dr[lowLevelOrdinal].ToString();
                            if (dr[highLevelOrdinal] == DBNull.Value)
                                myTankConfig.HighLevel = "*** Empty ***";
                            else
                                myTankConfig.HighLevel = dr[highLevelOrdinal].ToString();
                            if (dr[highHighLevelOrdinal] == DBNull.Value)
                                myTankConfig.HighHighLevel = "*** Empty ***";
                            else
                                myTankConfig.HighHighLevel = dr[highHighLevelOrdinal].ToString();
                            if (dr[fillDetectDeltaOrdinal] == DBNull.Value)
                                myTankConfig.FillDetectDelta = "*** Empty ***";
                            else
                                myTankConfig.FillDetectDelta = dr[fillDetectDeltaOrdinal].ToString();
                            if (dr[shortFillDeltaOrdinal] == DBNull.Value)
                                myTankConfig.ShortFillDelta = "*** Empty ***";
                            else
                                myTankConfig.ShortFillDelta = dr[shortFillDeltaOrdinal].ToString();
                            if (dr[volumeDeltaOrdinal] == DBNull.Value)
                                myTankConfig.VolumeDelta = "*** Empty ***";
                            else
                                myTankConfig.VolumeDelta = dr[volumeDeltaOrdinal].ToString();
                            if (dr[rateChangeDeltaOrdinal] == DBNull.Value)
                                myTankConfig.RateChangeDelta = "*** Empty ***";
                            else
                                myTankConfig.RateChangeDelta = dr[rateChangeDeltaOrdinal].ToString();
                            if (dr[callsperdayOrdinal] == DBNull.Value)
                                myTankConfig.Callsperday = "*** Empty ***";
                            else
                                myTankConfig.Callsperday = dr[callsperdayOrdinal].ToString();
                            if (dr[callDayOrdinal] == DBNull.Value)
                                myTankConfig.CallDay = "*** Empty ***";
                            else
                                myTankConfig.CallDay = dr[callDayOrdinal].ToString();
                            if (dr[intervalOrdinal] == DBNull.Value)
                                myTankConfig.Interval = "*** Empty ***";
                            else
                                myTankConfig.Interval = dr[intervalOrdinal].ToString();
                            if (dr[diagCallDayMaskOrdinal] == DBNull.Value)
                                myTankConfig.DiagCallDayMask = "*** Empty ***";
                            else
                                myTankConfig.DiagCallDayMask = dr[diagCallDayMaskOrdinal].ToString();
                            if (dr[dataLogDeltaOrdinal] == DBNull.Value)
                                myTankConfig.DataLogDelta = "*** Empty ***";
                            else
                                myTankConfig.DataLogDelta = dr[dataLogDeltaOrdinal].ToString();
                            if (dr[usageDeltaOrdinal] == DBNull.Value)
                                myTankConfig.UsageDelta = "*** Empty ***";
                            else
                                myTankConfig.UsageDelta = dr[usageDeltaOrdinal].ToString();
                            if (dr[wakeIntervalOrdinal] == DBNull.Value)
                                myTankConfig.WakeInterval = "*** Empty ***";
                            else
                                myTankConfig.WakeInterval = dr[wakeIntervalOrdinal].ToString();
                            if (dr[startTimeOrdinal] == DBNull.Value)
                                myTankConfig.StartTime = "*** Empty ***";
                            else
                                myTankConfig.StartTime = dr[startTimeOrdinal].ToString();
                            if (dr[highSetPointOrdinal] == DBNull.Value)
                                myTankConfig.HighSetPoint = "*** Empty ***";
                            else
                                myTankConfig.HighSetPoint = dr[highSetPointOrdinal].ToString();
                            if (dr[lowSetPointOrdinal] == DBNull.Value)
                                myTankConfig.LowSetPoint = "*** Empty ***";
                            else
                                myTankConfig.LowSetPoint = dr[lowSetPointOrdinal].ToString();
                            if (dr[sensorOffsetOrdinal] == DBNull.Value)
                                myTankConfig.SensorOffset = "*** Empty ***";
                            else
                                myTankConfig.SensorOffset = dr[sensorOffsetOrdinal].ToString();
                            if (dr[coeffExpOrdinal] == DBNull.Value)
                                myTankConfig.CoeffExp = "*** Empty ***";
                            else
                                myTankConfig.CoeffExp = dr[coeffExpOrdinal].ToString();
                            if (dr[specGravOrdinal] == DBNull.Value)
                                myTankConfig.SpecGrav = "*** Empty ***";
                            else
                                myTankConfig.SpecGrav = dr[specGravOrdinal].ToString();
                            if (dr[deviceFillDetectDeltaOrdinal] == DBNull.Value)
                                myTankConfig.DeviceFillDetectDelta = "*** Empty ***";
                            else
                                myTankConfig.DeviceFillDetectDelta = dr[deviceFillDetectDeltaOrdinal].ToString();
                            if (dr[deviceFillHysteresisOrdinal] == DBNull.Value)
                                myTankConfig.DeviceFillHysteresis = "*** Empty ***";
                            else
                                myTankConfig.DeviceFillHysteresis = dr[deviceFillHysteresisOrdinal].ToString();
                            if (dr[deviceCriticalLowLevelOrdinal] == DBNull.Value)
                                myTankConfig.DeviceCriticalLowLevel = "*** Empty ***";
                            else
                                myTankConfig.DeviceCriticalLowLevel = dr[deviceCriticalLowLevelOrdinal].ToString();
                            if (dr[deviceLowLevelOrdinal] == DBNull.Value)
                                myTankConfig.DeviceLowLevel = "*** Empty ***";
                            else
                                myTankConfig.DeviceLowLevel = dr[deviceLowLevelOrdinal].ToString();
                            if (dr[deviceHighLevelOrdinal] == DBNull.Value)
                                myTankConfig.DeviceHighLevel = "*** Empty ***";
                            else
                                myTankConfig.DeviceHighLevel = dr[deviceHighLevelOrdinal].ToString();
                            if (dr[deviceCriticalHighLevelOrdinal] == DBNull.Value)
                                myTankConfig.DeviceCriticalHighLevel = "*** Empty ***";
                            else
                                myTankConfig.DeviceCriticalHighLevel = dr[deviceCriticalHighLevelOrdinal].ToString();
                            if (dr[deviceFillDetectOrdinal] == DBNull.Value)
                                myTankConfig.DeviceFillDetect = "*** Empty ***";
                            else
                                myTankConfig.DeviceFillDetect = dr[deviceFillDetectOrdinal].ToString();
                            if (dr[deviceUsageAlarmOrdinal] == DBNull.Value)
                                myTankConfig.DeviceUsageAlarm = "*** Empty ***";
                            else
                                myTankConfig.DeviceUsageAlarm = dr[deviceUsageAlarmOrdinal].ToString();
                            if (dr[hasExpectedCallAlarmOrdinal] == DBNull.Value)
                                myTankConfig.HasExpectedCallAlarm = "*** Empty ***";
                            else
                                myTankConfig.HasExpectedCallAlarm = dr[hasExpectedCallAlarmOrdinal].ToString();
                            if (dr[tankNormallyFillsOrdinal] == DBNull.Value)
                                myTankConfig.TankNormallyFills = "*** Empty ***";
                            else
                                myTankConfig.TankNormallyFills = dr[tankNormallyFillsOrdinal].ToString();
                            //new code added to accomodate EnableLocation and EnableGPS
                            // D Arcilla
                            //   Oct 2020
                            if (dr[enableLocationOrdinal] == DBNull.Value)
                                myTankConfig.EnableLocation = "*** Empty ***";
                            else
                                myTankConfig.EnableLocation = dr[enableLocationOrdinal].ToString();
                            if (dr[enableGPSOrdinal] == DBNull.Value)
                                myTankConfig.EnableGPS = "*** Empty ***";
                            else
                                myTankConfig.EnableGPS = dr[enableGPSOrdinal].ToString();

                            myTankConfigs.Add(myTankConfig);
                        }
                        //        dr.Close();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ReadEXCELFile - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal async void ValidateTheEXCELFile(IHubContext<ProgressHub> _notificationHubContext)
        {
            try
            {
                RequiredColumnsCheck();
                await _notificationHubContext.Clients.All.SendAsync("sendToUser","Required Columns", 1, 5);

                if (this.HaveError && !wroteErrorFile)  WriteErrorReport();
                if (!this.HaveError)                    DataTypeCheck();
                await _notificationHubContext.Clients.All.SendAsync("sendToUser","Data Type", 2, 5);

                if (this.HaveError && !wroteErrorFile)  WriteErrorReport();
                if (!this.HaveError && checkRTUCondition)  ValidateTankIDRTUNumber();
                await _notificationHubContext.Clients.All.SendAsync("sendToUser","TankID RTU Number",3, 5);
                if (this.HaveError && !wroteErrorFile)  WriteErrorReport();
               
                if (!this.HaveError)
                {
                    ValidateTankScope();
                    if (this.HaveError)
                        WriteErrorReport();
                }
                await _notificationHubContext.Clients.All.SendAsync("sendToUser","Valid Tank Scope", 4, 5);

                if (this.HaveError && !wroteErrorFile)  WriteErrorReport();
                if (!this.HaveError)                    BulkConfigValueChecks();
                if (this.HaveError && !wroteErrorFile)  WriteErrorReport();
                await _notificationHubContext.Clients.All.SendAsync("sendToUser", "Bulk Config Value Check", 5, 5);

            }
            catch (Exception ex)
            {
                _ = ex.Message;
                ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ValidateTheEXCELFile - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void RequiredColumnsCheck()
        {
            try
            {
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                    aTankConfig.TankIDRTUNumberCheck();
                    if (aTankConfig.HaveError)
                        this.HaveError = true;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at RequiredColumnsCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        private void WriteErrorReport()
        {
            try
            {
                int i = 0;
                bool directoryExists = Directory.Exists(this.ErrorFilePath);
                if (!directoryExists)
                    Directory.CreateDirectory(this.ErrorFilePath);
                FileInfo errorReportFile = new FileInfo(this.ErrorFileName);
                if (errorReportFile.Exists)
                    errorReportFile.Delete();
               // using (ExcelPackage package = new ExcelPackage(errorReportFile))
               // {
                    i = 2;
                    //errorReportSheet = package.Workbook.Worksheets.Add("BulkConfig Errors");
                    //errorReportSheet.Cells[1, 1].Value = "TankID";
                    //errorReportSheet.Cells[1, 2].Value = "Bad Column";
                    //errorReportSheet.Cells[1, 3].Value = "Bad Column Value";
                    //errorReportSheet.Cells[1, 4].Value = "Error";
                    foreach (TankConfig aTankConfig in myTankConfigs)
                    {
                       // if (aTankConfig.HaveError)
                        {
                           // errorReportSheet.Cells[i, 1].Value = aTankConfig.TankID;
                           // errorReportSheet.Cells[i, 2].Value = aTankConfig.BadColumn;
                          //  errorReportSheet.Cells[i, 3].Value = aTankConfig.BadColumnValue;
                         //   errorReportSheet.Cells[i, 4].Value = aTankConfig.StatusMessage;
                            i ++;
                        }
                    }
                    //using (var range = errorReportSheet.Cells[1, 1, 1, 4])
                    //{
                    //    range.Style.Font.Bold = true;
                    //   // range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    //    range.Style.Font.Color.SetColor(Color.White);
                    //}
                    //errorReportSheet.Cells.AutoFitColumns(0);
                    //errorReportSheet.View.PageLayoutView = false;
                  //  package.Save();
               // }
                wroteErrorFile = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at WriteErrorReport - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DataTypeCheck()
        {
            try
            {
                int i = 0;
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                    i ++;
                    aTankConfig.IntegerClassCheck("TankID");
                    aTankConfig.IntegerClassCheck("RTUNumber");
                    aTankConfig.DecimalClassCheck("TankHgt");
                    aTankConfig.DecimalClassCheck("TankCap");
                    aTankConfig.DecimalClassCheck("CapacityLimit");
                    aTankConfig.DecimalClassCheck("TankMinimum");
                    aTankConfig.IntegerClassCheck("ReorderUsage");
                    aTankConfig.IntegerClassCheck("SafetyStockUsage");
                    aTankConfig.DateTimeClassCheck("StartTime");
                    aTankConfig.IntegerClassCheck("CallDay");
                    aTankConfig.IntegerClassCheck("Interval");
                    aTankConfig.IntegerClassCheck("DiagCallDayMask");
                    aTankConfig.IntegerClassCheck("Callsperday");
                    aTankConfig.DecimalClassCheck("HighSetPoint");
                    aTankConfig.DecimalClassCheck("LowSetPoint");
                    aTankConfig.DecimalClassCheck("SensorOffset");
                    aTankConfig.DecimalClassCheck("CoeffExp");
                    aTankConfig.DecimalClassCheck("SpecGrav");
                    aTankConfig.IntegerClassCheck("LowLowLevel");
                    aTankConfig.IntegerClassCheck("LowLevel");
                    aTankConfig.IntegerClassCheck("HighLevel");
                    aTankConfig.IntegerClassCheck("HighHighLevel");
                    aTankConfig.DecimalClassCheck("FillDetectDelta");
                    aTankConfig.DecimalClassCheck("ShortFillDelta");
                    aTankConfig.IntegerClassCheck("VolumeDelta");
                    aTankConfig.IntegerClassCheck("RateChangeDelta");
                    aTankConfig.BoolClassCheck("DeviceCriticalLowLevel");
                    aTankConfig.BoolClassCheck("DeviceLowLevel");
                    aTankConfig.BoolClassCheck("DeviceHighLevel");
                    aTankConfig.BoolClassCheck("DeviceCriticalHighLevel");
                    aTankConfig.BoolClassCheck("DeviceFillDetect");
                    aTankConfig.DecimalClassCheck("DeviceFillDetectDelta");
                    aTankConfig.DecimalClassCheck("DeviceFillHysteresis");
                    aTankConfig.IntegerClassCheck("DataLogDelta");
                    aTankConfig.IntegerClassCheck("UsageDelta");
                    aTankConfig.IntegerClassCheck("WakeInterval");
                    aTankConfig.BoolClassCheck("DeviceUsageAlarm");
                    aTankConfig.BoolClassCheck("HasExpectedCallAlarm");
                    aTankConfig.BoolClassCheck("TankNormallyFills");
                    aTankConfig.BoolClassCheck("EnableGPS");
                    aTankConfig.BoolClassCheck("EnableLocation");
                    if (aTankConfig.HaveError)
                        this.HaveError = true;
                   
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DataTypeCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void ValidateTankIDRTUNumber()
        {
            try
            {
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                    aTankConfig.GetCurrentDeviceID();
                    if (!aTankConfig.ValidateRTUNumber())
                    {
                        aTankConfig.HaveError = true;
                        this.HaveError = true;
                        aTankConfig.StatusMessage = "TankID/RTU Number mismatch";
                        aTankConfig.BadColumn = "RTUNumber";
                        aTankConfig.BadColumnValue = aTankConfig.RTUNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ValidateTankIDRTUNumber - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void ValidateTankScope()
        {
            try
            {
                bool matchfound = false;
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                    matchfound = false;
                    aTankConfig.GetCurrentTankConfigInfo();
                    this.SuperUser = true;
                    if (!this.SuperUser)
                    {
                        foreach (var userOrganization in myUserOrganizations)
                        {
                            if (aTankConfig.OrganizationID == userOrganization.OrganizationID)
                            {
                                matchfound = true;
                                break;
                            }
                        }
                        if (!matchfound)
                        {
                            this.HaveError = true;
                            aTankConfig.HaveError = true;
                            aTankConfig.StatusMessage = "TankID not within User scope";
                            aTankConfig.BadColumn = "OrganizationID";
                            aTankConfig.BadColumnValue = aTankConfig.OrganizationID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ValidateTankScope - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void BulkConfigValueChecks()
        {
            try
            {
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                    if (!aTankConfig.HaveError)
                    {
                        aTankConfig.TankNameCheck();
                        aTankConfig.TankHgtCheck();
                        aTankConfig.TankCapCheck();
                        aTankConfig.CapcityLimitCheck();
                        aTankConfig.TankMinimumCheck();
                        aTankConfig.ReorderUsageCheck();
                        aTankConfig.SafetyStockCheck();
                        aTankConfig.StartTimeCheck();
                        aTankConfig.CallDayCheck();
                        aTankConfig.DiagCallDayMaskCheck();
                        aTankConfig.IntervalCheck();
                        aTankConfig.CallsperdayCheck();
                        aTankConfig.SensorOffsetCheck();
                        aTankConfig.CoeffExpCheck();
                        aTankConfig.SpecGravCheck();
                        aTankConfig.LowLowLevelCheck();
                        aTankConfig.LowLevelCheck();
                        aTankConfig.HighLevelCheck();
                        aTankConfig.HighHighLevelCheck();
                        aTankConfig.FillDetectDeltaCheck();
                        aTankConfig.ShortFillDeltaCheck();
                        aTankConfig.VolumeDeltaCheck();
                        aTankConfig.RateChangeDeltaCheck();
                        aTankConfig.DeviceFillDetectDeltaCheck();
                        aTankConfig.DeviceFillHysteresisCheck();
                        aTankConfig.DataLogDeltaCheck();
                        aTankConfig.UsageDeltaCheck();
                        aTankConfig.WakeIntervalCheck();
                        aTankConfig.EnableGPSCheck();
                        aTankConfig.EnableLocationCheck();
                    }
                    if (aTankConfig.HaveError)
                    {
                        this.HaveError = true;
                        WriteErrorReport();
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                 FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at BuildConfigValuesCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        public async void ApplyTankConfigChanges(IHubContext<ProgressHub> _notificationHubContext)
        {
            int recordsToProcessPerSecond = this.RecordThrottle;
            int processedRecordCount = 0;
            int i = 0;
            int throttleamount;
            try
            {
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                   statusReportWriter = new FileWriter(this.StatusFileName);
                    sb.Length = 0;
                    if (this.RecordThrottle == 0)
                    {
                        if (aTankConfig.PerformUpdate)
                        {
                            if (aTankConfig.Add())
                                aTankConfig.StatusMessage = "Successful Update";
                            else
                                aTankConfig.StatusMessage = "Update Failed";
                            sb.Append(DateTime.Now.ToString().PadRight(25));
                            sb.Append(aTankConfig.TankID.ToString().PadRight(20));
                            sb.Append(aTankConfig.StatusMessage);
                            statusReportWriter.Write(sb.ToString());
                            statusReportWriter.Close();
                            i++;
                        }
                    }
                    if (this.RecordThrottle != 0)
                    {
                        if (aTankConfig.PerformUpdate)
                        {
                            if (processedRecordCount >= recordsToProcessPerSecond)
                            {
                                processedRecordCount = 0;
                                throttleamount = this.ThrottleAmount * 1000;
                                Util.Throttle(throttleamount);
                            }
                            if (aTankConfig.Add())
                                aTankConfig.StatusMessage = "Successful Update";
                            else
                                aTankConfig.StatusMessage = "Update Failed";
                            processedRecordCount ++;
                            sb.Append(DateTime.Now.ToString().PadRight(25));
                            sb.Append(aTankConfig.TankID.ToString().PadRight(20));
                            sb.Append(aTankConfig.StatusMessage);
                            statusReportWriter.Write(sb.ToString());
                            statusReportWriter.Close();
                            i++;
                        }
                    }
                    this.CurrentEXCELCount = i;
                    await _notificationHubContext.Clients.All.SendAsync("sendToProcessing", aTankConfig.TankID,this.CurrentEXCELCount, myTankConfigs.Count);
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                 ErrorFileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ApplyTankConfigChanges - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

    }

}