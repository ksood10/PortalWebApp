using PortalWebApp.Areas.Utilities;
using PortalWebApp.Data;
using PortalWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PortalWebApp.Utilities
{
    public class BulkConfiguratorQueue
    {
        private readonly string oleDB = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
        private readonly StringBuilder sb = new StringBuilder();
        private List<Organization> myUserOrganizations;
        private  List<TankConfig> myTankConfigs;
        private  TankConfig myTankConfig;
        private  FileWriter statusReportWriter;
        private readonly bool checkRTUCondition;
        private  bool wroteErrorFileHeadings;
        private bool wroteErrorFile;
        private  bool wroteStatusFileHeadings;
        private  Thread myThread;

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

        #endregion

        public BulkConfiguratorQueue()
        {
        }

        private PortalWebAppContext _databaseContext;
        public BulkConfiguratorQueue(BulkUpdate bulkUpdate, PortalWebAppContext databaseContext)
        {
            _databaseContext = databaseContext;
            this.ConnectionString = bulkUpdate.Environment;
            this.FileName = bulkUpdate.FileName;
            this.OleDBConnectionString = BuildOleDbConnectionString();
            this.UserID = bulkUpdate.UserID;
            this.RecordThrottle = bulkUpdate.ThrottleNum;
            this.ThrottleAmount = bulkUpdate.ThrottleDuration;
            wroteErrorFileHeadings = false;
            checkRTUCondition = bulkUpdate.RTU;
            //this.ErrorFilePath = "C:\\BulkConfig\\ErrorFile\\";
            this.ErrorFilePath = AppDomain.CurrentDomain.BaseDirectory + "ErrorFile";
            //this.ErrorFileName = this.ErrorFilePath + "Errors_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
            this.ErrorFileName = this.ErrorFilePath + "\\" + "Errors_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
            //this.StatusFilePath = "C:\\BulkConfig\\SummaryFile\\";
            this.StatusFilePath = AppDomain.CurrentDomain.BaseDirectory + "SummaryFile";
            //this.StatusFileName = this.StatusFilePath + "Summary_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".txt";
            this.StatusFileName = this.StatusFilePath + "\\" + "Summary_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".txt";
            DeleteOldReportFiles();
            GetUserOrganization(UserID);
            if (!this.HaveError)
            {
                if (!this.SuperUser)
                    BuildListOfUserOrganizations();
                ReadEXCELFile();
                if (!this.HaveEXCELReadError)
                {
                   // MessageBox.Show("# of EXCEL Records To Process: " + myTankConfigs.Count.ToString());
                    this.TotalEXCELCount = myTankConfigs.Count;
                    myThread = new Thread(new ThreadStart(ValidateTheEXCELFile));
                    ValidateTheEXCELFile();
                }
            }
        }

        private string BuildOleDbConnectionString()
        {
            sb.Length = 0;
            sb.Append(oleDB);
            sb.Append(this.FileName);
            sb.Append(";Extended Properties=");
            sb.Append(Convert.ToChar(34).ToString());
            sb.Append("Excel 12.0 Xml;HDR=YES;IMEX=1;TypeGuessRows=0");
            sb.Append(Convert.ToChar(34).ToString());
            return sb.ToString();
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
                string errorMsg = ex.Message;
            }
        }

        private void GetUserOrganization(int userid)
        {
            bool hasglobalorgsecurity = false;
            try
            {
                var userList = (from user in _databaseContext.User
                                where user.UserId == userid
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
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
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
             
                var userList = (from orgTree in _databaseContext.OrganizationTree
                                where orgTree.OrgID == this.UserOrganizationID
                                select new { orgTree.ChildOrgID }).ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at BuildListOfUserOrganizations - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        private void ReadEXCELFile()
        {
            string result = string.Empty;
            string test = string.Empty;
            //////////////////////////////////////////////////////
            ///
            /////////////////////////////////////////////////////
            var filename = Path.GetFileName(FileName);
           
            // bulkConfiguratorQueue.
            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            //create directory "Uploads" if it doesn't exists
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);
            //get file path 
            var filePath = Path.Combine(MainPath, filename);
            //using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))            //{            //    await origstream.CopyToAsync(stream);            //}

            string conString = string.Empty;

            switch (Path.GetExtension(filename))
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            DataTable dt = new DataTable();
            conString = string.Format(conString, filePath);

            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using OleDbCommand cmdExcel = new OleDbCommand();
                using OleDbDataAdapter odaExcel = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet.
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                //   string sheetName = "Sheet1$";
                connExcel.Close();

                //Read Data from First Sheet.
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                odaExcel.SelectCommand = cmdExcel;
                odaExcel.Fill(dt);
                connExcel.Close();
            }
            //////////////////////////////////////////////
            ///
            try
            {
                //sb.Length = 0;
                //sb.Append("select [TankID], [RTUNumber], [TankName], [TankHgt],	[TankCap], [CapacityLimit], ");
                //sb.Append("[TankMinimum], [ReorderUsage], [SafetyStockUsage], [StartTime], [Callsperday], [CallDay], ");
                //sb.Append("[Interval], [DiagCallDayMask], [HighSetPoint], [LowSetPoint], [SensorOffset], [CoeffExp], ");
                //sb.Append("[SpecGrav], [LowLowLevel], [LowLevel], [HighLevel], [HighHighLevel], [FillDetectDelta], ");
                //sb.Append("[ShortFillDelta], [VolumeDelta],	[RateChangeDelta], [DeviceCriticalLowLevel], [DeviceLowLevel], ");
                //sb.Append("[DeviceHighLevel], [DeviceCriticalHighLevel], [DeviceFillDetect], [DeviceFillDetectDelta], ");
                //sb.Append("[DeviceFillHysteresis], [DataLogDelta], [UsageDelta], [WakeInterval], [DeviceUsageAlarm], ");
                //sb.Append("[HasExpectedCallAlarm], [TankNormallyFills] ");
                //new code added to accomodate EnableLocation and EnableGPS
                //D Arcilla
                //Oct 2020
                //sb.Append(",[EnableGPS], [EnableLocation] ");
                //sb.Append("from [Sheet1$]");
                //  OleDbDataReader dr = DAL.ReturnOleDbDataReader2(sb.ToString(), this.OleDBConnectionString, null, true, ref result);
                //MessageBox.Show("Error: " + result);
                //if (dr == null)
                //{
                //    this.HaveEXCELReadError = true;
                //}
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
                int callsPerDayOrdinal = dt.Columns["CallsPerDay"].Ordinal;
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
                //    //D Arcilla
                //    //Oct 2020
                int enableLocationOrdinal = dt.Columns["EnableLocation"].Ordinal;
                int enableGPSOrdinal = dt.Columns["EnableGPS"].Ordinal;
                if (dt.Rows.Count > 0)
                {
                    var myTankConfigs = new List<TankConfig>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        myTankConfig = new TankConfig(this.ConnectionString, this.UserID);
                        myTankConfig.ErrorFilePath = this.ErrorFilePath;
                        myTankConfig.CheckRTUCondition = checkRTUCondition;
                        if (dr[tankIDOrdinal] == DBNull.Value)
                            myTankConfig.TankID = 0;
                        else
                            myTankConfig.TankID = Convert.ToInt32(dr[tankIDOrdinal]);
                        if (dr[rtuNumberOrdinal] == DBNull.Value)
                            myTankConfig.RTUNumber = "*** Empty ***";
                        else
                            myTankConfig.RTUNumber = dr[rtuNumberOrdinal].ToString();
                        if (dr[tankNameOrdinal] == DBNull.Value)
                            myTankConfig.TankName = "*** Empty ***";
                        else
                            myTankConfig.TankName = dr[tankNameOrdinal].ToString();
                        if (dr[tankHgtOrdinal] == DBNull.Value)
                            myTankConfig.TankHgt =0;
                        else
                            myTankConfig.TankHgt = Convert.ToDecimal(dr[tankHgtOrdinal]);
                        if (dr[tankCapOrdinal] == DBNull.Value)
                            myTankConfig.TankCap = 0;
                        else
                            myTankConfig.TankCap = Convert.ToDecimal( dr[tankCapOrdinal]);
                        if (dr[capacityLimitOrdinal] == DBNull.Value)
                            myTankConfig.CapacityLimit = 0;
                        else
                            myTankConfig.CapacityLimit = Convert.ToDecimal(dr[capacityLimitOrdinal]);
                        if (dr[tankMinimumOrdinal] == DBNull.Value)
                            myTankConfig.TankMinimum = 0;
                        else
                            myTankConfig.TankMinimum = Convert.ToDecimal(dr[tankMinimumOrdinal]);
                        if (dr[reorderUsageOrdinal] == DBNull.Value)
                            myTankConfig.ReorderUsage = 0;
                        else
                            myTankConfig.ReorderUsage = Convert.ToInt32(dr[reorderUsageOrdinal]);
                        if (dr[safetyStockUsageOrdinal] == DBNull.Value)
                            myTankConfig.SafetyStockUsage = 0;
                        else
                            myTankConfig.SafetyStockUsage = Convert.ToInt32(dr[safetyStockUsageOrdinal]);
                        if (dr[lowLowLevelOrdinal] == DBNull.Value)
                            myTankConfig.LowLowLevel = 0;
                        else
                            myTankConfig.LowLowLevel = Convert.ToInt32(dr[lowLowLevelOrdinal]);
                        if (dr[lowLevelOrdinal] == DBNull.Value)
                            myTankConfig.LowLevel = 0;
                        else
                            myTankConfig.LowLevel = Convert.ToInt32(dr[lowLevelOrdinal]);
                        if (dr[highLevelOrdinal] == DBNull.Value)
                            myTankConfig.HighLevel = 0;
                        else
                            myTankConfig.HighLevel = Convert.ToInt32(dr[highLevelOrdinal]);
                        if (dr[highHighLevelOrdinal] == DBNull.Value)
                            myTankConfig.HighHighLevel = 0;
                        else
                            myTankConfig.HighHighLevel = Convert.ToInt32(dr[highHighLevelOrdinal]);
                        if (dr[fillDetectDeltaOrdinal] == DBNull.Value)
                            myTankConfig.FillDetectDelta =0;
                        else
                            myTankConfig.FillDetectDelta = Convert.ToDecimal(dr[fillDetectDeltaOrdinal]);
                        if (dr[shortFillDeltaOrdinal] == DBNull.Value)
                            myTankConfig.ShortFillDelta =0;
                        else
                            myTankConfig.ShortFillDelta = Convert.ToDecimal(dr[shortFillDeltaOrdinal]);
                        if (dr[volumeDeltaOrdinal] == DBNull.Value)
                            myTankConfig.VolumeDelta = 0;
                        else
                            myTankConfig.VolumeDelta = Convert.ToInt32(dr[volumeDeltaOrdinal]);
                        if (dr[rateChangeDeltaOrdinal] == DBNull.Value)
                            myTankConfig.RateChangeDelta = 0;
                        else
                            myTankConfig.RateChangeDelta = Convert.ToInt32(dr[rateChangeDeltaOrdinal]);
                        if (dr[callsPerDayOrdinal] == DBNull.Value)
                            myTankConfig.CallDay =0;
                        else
                            myTankConfig.CallDay = Convert.ToInt32(dr[callsPerDayOrdinal]);
                        if (dr[callDayOrdinal] == DBNull.Value)
                            myTankConfig.CallDay =0;
                        else
                            myTankConfig.CallDay = Convert.ToInt32(dr[callDayOrdinal]);
                        if (dr[intervalOrdinal] == DBNull.Value)
                            myTankConfig.Interval = "*** Empty ***";
                        else
                            myTankConfig.Interval = dr[intervalOrdinal].ToString();
                        if (dr[diagCallDayMaskOrdinal] == DBNull.Value)
                            myTankConfig.DiagCallDayMask =0;
                        else
                            myTankConfig.DiagCallDayMask = Convert.ToInt32(dr[diagCallDayMaskOrdinal]);
                        if (dr[dataLogDeltaOrdinal] == DBNull.Value)
                            myTankConfig.DataLogDelta =0;
                        else
                            myTankConfig.DataLogDelta = Convert.ToInt32(dr[dataLogDeltaOrdinal]);
                        if (dr[usageDeltaOrdinal] == DBNull.Value)
                            myTankConfig.UsageDelta =0;
                        else
                            myTankConfig.UsageDelta = Convert.ToInt32(dr[usageDeltaOrdinal]);
                        if (dr[wakeIntervalOrdinal] == DBNull.Value)
                            myTankConfig.WakeInterval =0;
                        else
                            myTankConfig.WakeInterval = Convert.ToInt32(dr[wakeIntervalOrdinal]);
                        if (dr[startTimeOrdinal] == DBNull.Value)
                            myTankConfig.StartTime =DateTime.Now;
                        else
                            myTankConfig.StartTime = Convert.ToDateTime(dr[startTimeOrdinal]);
                        if (dr[highSetPointOrdinal] == DBNull.Value)
                            myTankConfig.HighSetPoint =0;
                        else
                            myTankConfig.HighSetPoint = Convert.ToDecimal(dr[highSetPointOrdinal]);
                        if (dr[lowSetPointOrdinal] == DBNull.Value)
                            myTankConfig.LowSetPoint =0;
                        else
                            myTankConfig.LowSetPoint = Convert.ToDecimal(dr[lowSetPointOrdinal]);
                        if (dr[sensorOffsetOrdinal] == DBNull.Value)
                            myTankConfig.SensorOffset =0;
                        else
                            myTankConfig.SensorOffset = Convert.ToDecimal(dr[sensorOffsetOrdinal]);
                        if (dr[coeffExpOrdinal] == DBNull.Value)
                            myTankConfig.CoeffExp =0;
                        else
                            myTankConfig.CoeffExp = Convert.ToDecimal(dr[coeffExpOrdinal]);
                        if (dr[specGravOrdinal] == DBNull.Value)
                            myTankConfig.SpecGrav =0;
                        else
                            myTankConfig.SpecGrav = Convert.ToDecimal(dr[specGravOrdinal]);
                        if (dr[deviceFillDetectDeltaOrdinal] == DBNull.Value)
                            myTankConfig.DeviceFillDetectDelta =0;
                        else
                            myTankConfig.DeviceFillDetectDelta = Convert.ToDecimal(dr[deviceFillDetectDeltaOrdinal]);
                        if (dr[deviceFillHysteresisOrdinal] == DBNull.Value)
                            myTankConfig.DeviceFillHysteresis =0;
                        else
                            myTankConfig.DeviceFillHysteresis = Convert.ToDecimal(dr[deviceFillHysteresisOrdinal]);
                        if (dr[deviceCriticalLowLevelOrdinal] == DBNull.Value)
                            myTankConfig.DeviceCriticalLowLevel =false;
                        else
                            myTankConfig.DeviceCriticalLowLevel = Convert.ToBoolean(dr[deviceCriticalLowLevelOrdinal]);
                        if (dr[deviceLowLevelOrdinal] == DBNull.Value)
                            myTankConfig.DeviceLowLevel =false;
                        else
                            myTankConfig.DeviceLowLevel = Convert.ToBoolean(dr[deviceLowLevelOrdinal]);
                        if (dr[deviceHighLevelOrdinal] == DBNull.Value)
                            myTankConfig.DeviceHighLevel =false;
                        else
                            myTankConfig.DeviceHighLevel = Convert.ToBoolean(dr[deviceHighLevelOrdinal]);
                        if (dr[deviceCriticalHighLevelOrdinal] == DBNull.Value)
                            myTankConfig.DeviceCriticalHighLevel =false;
                        else
                            myTankConfig.DeviceCriticalHighLevel = Convert.ToBoolean(dr[deviceCriticalHighLevelOrdinal]);
                        if (dr[deviceFillDetectOrdinal] == DBNull.Value)
                            myTankConfig.DeviceFillDetect =false;
                        else
                            myTankConfig.DeviceFillDetect = Convert.ToBoolean(dr[deviceFillDetectOrdinal]);
                        if (dr[deviceUsageAlarmOrdinal] == DBNull.Value)
                            myTankConfig.DeviceUsageAlarm =false;
                        else
                            myTankConfig.DeviceUsageAlarm = Convert.ToBoolean(dr[deviceUsageAlarmOrdinal]);
                        if (dr[hasExpectedCallAlarmOrdinal] == DBNull.Value)
                            myTankConfig.HasExpectedCallAlarm =false;
                        else
                            myTankConfig.HasExpectedCallAlarm = Convert.ToBoolean(dr[hasExpectedCallAlarmOrdinal]);
                        if (dr[tankNormallyFillsOrdinal] == DBNull.Value)
                            myTankConfig.TankNormallyFills =false;
                        else
                            myTankConfig.TankNormallyFills = Convert.ToBoolean(dr[tankNormallyFillsOrdinal]);
                        //new code added to accomodate EnableLocation and EnableGPS
                       // D Arcilla
                     //   Oct 2020
                        if (dr[enableLocationOrdinal] == DBNull.Value)
                            myTankConfig.EnableLocation =0;
                        else
                            myTankConfig.EnableLocation = Convert.ToInt32(dr[enableLocationOrdinal]);
                        if (dr[enableGPSOrdinal] == DBNull.Value)
                            myTankConfig.EnableGPS =0;
                        else
                            myTankConfig.EnableGPS = Convert.ToInt32(dr[enableGPSOrdinal]);
                                   
                        myTankConfigs.Add(myTankConfig);
                    }
                    //        dr.Close();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ReadEXCELFile - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void ValidateTheEXCELFile()
        {
            try
            {
                RequiredColumnsCheck();
                if (this.HaveError && !wroteErrorFile)
                    WriteErrorReport();
                if (!this.HaveError)
                    DataTypeCheck();
                if (this.HaveError && !wroteErrorFile)
                    WriteErrorReport();
                if (!this.HaveError)
                {
                    if (checkRTUCondition)
                    {
                        ValidateTankIDRTUNumber();
                    }
                }
                if (this.HaveError && !wroteErrorFile)
                    WriteErrorReport();
                if (!this.HaveError)
                {
                    ValidateTankScope();
                    if (this.HaveError)
                        WriteErrorReport();
                }
                if (this.HaveError && !wroteErrorFile)
                    WriteErrorReport();
                if (!this.HaveError)
                {
                    BulkConfigValueChecks();
                }
                if (this.HaveError && !wroteErrorFile)
                    WriteErrorReport();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
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
                //foreach (TankConfig aTankConfig in myTankConfigs)
                //{
                //    aTankConfig.TankIDRTUNumberCheck();
                //    if (aTankConfig.HaveError)
                //        this.HaveError = true;
                //}
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
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
                            i = i + 1;
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
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
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
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                    //aTankConfig.IntegerClassCheck("TankID");
                    //aTankConfig.IntegerClassCheck("RTUNumber");
                    //aTankConfig.DecimalClassCheck("TankHgt");
                    //aTankConfig.DecimalClassCheck("TankCap");
                    //aTankConfig.DecimalClassCheck("CapacityLimit");
                    //aTankConfig.DecimalClassCheck("TankMinimum");
                    //aTankConfig.IntegerClassCheck("ReorderUsage");
                    //aTankConfig.IntegerClassCheck("SafetyStockUsage");
                    //aTankConfig.DateTimeClassCheck("StartTime");
                    //aTankConfig.IntegerClassCheck("CallDay");
                    //aTankConfig.IntegerClassCheck("Interval");
                    //aTankConfig.IntegerClassCheck("DiagCallDayMask");
                    //aTankConfig.IntegerClassCheck("Callsperday");
                    //aTankConfig.DecimalClassCheck("HighSetPoint");
                    //aTankConfig.DecimalClassCheck("LowSetPoint");
                    //aTankConfig.DecimalClassCheck("SensorOffset");
                    //aTankConfig.DecimalClassCheck("CoeffExp");
                    //aTankConfig.DecimalClassCheck("SpecGrav");
                    //aTankConfig.IntegerClassCheck("LowLowLevel");
                    //aTankConfig.IntegerClassCheck("LowLevel");
                    //aTankConfig.IntegerClassCheck("HighLevel");
                    //aTankConfig.IntegerClassCheck("HighHighLevel");
                    //aTankConfig.DecimalClassCheck("FillDetectDelta");
                    //aTankConfig.DecimalClassCheck("ShortFillDelta");
                    //aTankConfig.IntegerClassCheck("VolumeDelta");
                    //aTankConfig.IntegerClassCheck("RateChangeDelta");
                    //aTankConfig.BoolClassCheck("DeviceCriticalLowLevel");
                    //aTankConfig.BoolClassCheck("DeviceLowLevel");
                    //aTankConfig.BoolClassCheck("DeviceHighLevel");
                    //aTankConfig.BoolClassCheck("DeviceCriticalHighLevel");
                    //aTankConfig.BoolClassCheck("DeviceFillDetect");
                    //aTankConfig.DecimalClassCheck("DeviceFillDetectDelta");
                    //aTankConfig.DecimalClassCheck("DeviceFillHysteresis");
                    //aTankConfig.IntegerClassCheck("DataLogDelta");
                    //aTankConfig.IntegerClassCheck("UsageDelta");
                    //aTankConfig.IntegerClassCheck("WakeInterval");
                    //aTankConfig.BoolClassCheck("DeviceUsageAlarm");
                    //aTankConfig.BoolClassCheck("HasExpectedCallAlarm");
                    //aTankConfig.BoolClassCheck("TankNormallyFills");
                    //aTankConfig.BoolClassCheck("EnableGPS");
                    //aTankConfig.BoolClassCheck("EnableLocation");
                    //if (aTankConfig.HaveError)
                    //    this.HaveError = true;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
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
                    //aTankConfig.GetCurrentDeviceID();
                    //if (!aTankConfig.ValidateRTUNumber())
                    //{
                    //    aTankConfig.HaveError = true;
                    //    this.HaveError = true;
                    //    aTankConfig.StatusMessage = "TankID/RTU Number mismatch";
                    //    aTankConfig.BadColumn = "RTUNumber";
                    //    aTankConfig.BadColumnValue = aTankConfig.RTUNumber;
                    //}
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
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
              //  bool matchfound = false;
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                  //  matchfound = false;
                  //  aTankConfig.GetCurrentTankConfigInfo();
                    //if (!this.SuperUser)
                    //{
                    //    foreach (int userOrganization in myUserOrganizations)
                    //    {
                    //        if (aTankConfig.OrganizationID == userOrganization)
                    //        {
                    //            matchfound = true;
                    //            break;
                    //        }
                    //    }
                    //    if (!matchfound)
                    //    {
                    //        this.HaveError = true;
                    //        aTankConfig.HaveError = true;
                    //        aTankConfig.StatusMessage = "TankID not within User scope";
                    //        aTankConfig.BadColumn = "OrganizationID";
                    //        aTankConfig.BadColumnValue = aTankConfig.OrganizationID.ToString();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
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
               // foreach (var row in  )
                //{
                //    if (!aTankConfig.HaveError)
                //    {
                //        aTankConfig.TankNameCheck();
                //        aTankConfig.TankHgtCheck();
                //        aTankConfig.TankCapCheck();
                //        aTankConfig.CapcityLimitCheck();
                //        aTankConfig.TankMinimumCheck();
                //        aTankConfig.ReorderUsageCheck();
                //        aTankConfig.SafetyStockCheck();
                //        aTankConfig.StartTimeCheck();
                //        aTankConfig.CallDayCheck();
                //        aTankConfig.DiagCallDayMaskCheck();
                //        aTankConfig.IntervalCheck();
                //        aTankConfig.CallsPerDayCheck();
                //        aTankConfig.SensorOffsetCheck();
                //        aTankConfig.CoeffExpCheck();
                //        aTankConfig.SpecGravCheck();
                //        aTankConfig.LowLowLevelCheck();
                //        aTankConfig.LowLevelCheck();
                //        aTankConfig.HighLevelCheck();
                //        aTankConfig.HighHighLevelCheck();
                //        aTankConfig.FillDetectDeltaCheck();
                //        aTankConfig.ShortFillDeltaCheck();
                //        aTankConfig.VolumeDeltaCheck();
                //        aTankConfig.RateChangeDeltaCheck();
                //        aTankConfig.DeviceFillDetectDeltaCheck();
                //        aTankConfig.DeviceFillHysteresisCheck();
                //        aTankConfig.DataLogDeltaCheck();
                //        aTankConfig.UsageDeltaCheck();
                //        aTankConfig.WakeIntervalCheck();
                //        aTankConfig.EnableGPSCheck();
                //        aTankConfig.EnableLocationCheck();
                //    }
                //    if (aTankConfig.HaveError)
                //    {
                //        this.HaveError = true;
                //        WriteErrorReport();
                //    }
                //}
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                 FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at BuildConfigValuesCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        public void ApplyTankConfigChanges()
        {
            int recordsToProcessPerSecond = this.RecordThrottle;
           // int processedRecordCount = 0;
            int i = 0;
          //  int throttleamount = 0;
            try
            {
                foreach (TankConfig aTankConfig in myTankConfigs)
                {
                   // statusReportWriter = new FileWriter(this.StatusFileName);
                    sb.Length = 0;
                    if (this.RecordThrottle == 0)
                    {
                        //if (aTankConfig.PerformUpdate)
                        //{
                        //    if (aTankConfig.Add())
                        //        aTankConfig.StatusMessage = "Successful Update";
                        //    else
                        //        aTankConfig.StatusMessage = "Update Failed";
                        //    sb.Append(DateTime.Now.ToString().PadRight(25));
                        //    sb.Append(aTankConfig.TankID.PadRight(20));
                        //    sb.Append(aTankConfig.StatusMessage);
                        //    //statusReportWriter.Write(sb.ToString());
                        //    //statusReportWriter.Close();
                        //    i = i + 1;
                        //}
                    }
                    if (this.RecordThrottle != 0)
                    {
                        //if (aTankConfig.PerformUpdate)
                        //{
                        //    if (processedRecordCount >= recordsToProcessPerSecond)
                        //    {
                        //        processedRecordCount = 0;
                        //        throttleamount = this.ThrottleAmount * 1000;
                        //        Utilities.Throttle(throttleamount);
                        //    }
                        //    if (aTankConfig.Add())
                        //        aTankConfig.StatusMessage = "Successful Update";
                        //    else
                        //        aTankConfig.StatusMessage = "Update Failed";
                        //    processedRecordCount = processedRecordCount + 1;
                        //    sb.Append(DateTime.Now.ToString().PadRight(25));
                        //    sb.Append(aTankConfig.TankID.PadRight(20));
                        //    sb.Append(aTankConfig.StatusMessage);
                        //    statusReportWriter.Write(sb.ToString());
                        //    statusReportWriter.Close();
                        //    i = i + 1;
                        //}
                    }
                    this.CurrentEXCELCount = i;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                FileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(FileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ApplyTankConfigChanges - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        public int GetCurrentEXCELCount()
        {
            return this.CurrentEXCELCount;
        }

        public string TestDLL()
        {
            return "hello";
        }
    }

}