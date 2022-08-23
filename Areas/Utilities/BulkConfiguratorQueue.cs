using System;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Threading;
using PortalWebApp.Models;
using PortalWebApp.Data;
using System.Collections.Generic;

namespace PortalWebApp.Utilities
{
    public class BulkConfiguratorQueue
    {
        private string connectionString;
        private string oleDBConnectionString;
        private string oleDB = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
        private string fileName;
        private string filePath;
        private int recordThrottle;
        private int throttleAmount;
        private int userID;
        private int organizationID;
        private int userOrganizationID;
        private StringBuilder sb = new StringBuilder();
        private List<Organization> myUserOrganizations;
        private List<TankConfig> myTankConfigs;
        private TankConfig myTankConfig;
      //  private FileWriter statusReportWriter;
        private bool haveError;
        private bool haveEXCELReadError;
        private bool superUser;
        private bool invalidUser;
        private string statusMessage;
        private string errorFileName;
        private string errorFilePath;
        private string statusFilePath;
        private bool checkRTUCondition;
        private bool wroteErrorFileHeadings;
        private bool wroteErrorFile;
        private bool wroteStatusFileHeadings;
     //   private ExcelWorksheet errorReportSheet;
     //   private ExcelWorksheet statusReportSheet;
        private int totalEXCELCount;
        private int currentEXCELCount;
        private Thread myThread;

        #region Properties

        internal string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        internal string StatusFilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        internal string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        internal string ErrorFilePath
        {
            get
            {
                return errorFilePath;
            }
            set
            {
                errorFilePath = value;
            }
        }

        internal string ErrorFileName
        {
            get
            {
                return errorFileName;
            }
            set
            {
                errorFileName = value;
            }
        }

        internal string StatusFileName
        {
            get
            {
                return statusFilePath;
            }
            set
            {
                statusFilePath = value;
            }
        }

        internal string OleDBConnectionString
        {
            get
            {
                return oleDBConnectionString;
            }
            set
            {
                oleDBConnectionString = value;
            }
        }

        public int TotalEXCELCount
        {
            get
            {
                return totalEXCELCount;
            }
            set
            {
                totalEXCELCount = value;
            }
        }

        public int CurrentEXCELCount
        {
            get
            {
                return currentEXCELCount;
            }
            set
            {
                currentEXCELCount = value;
            }
        }

        internal int RecordThrottle
        {
            get
            {
                return recordThrottle;
            }
            set
            {
                recordThrottle = value;
            }
        }

        internal int ThrottleAmount
        {
            get
            {
                return throttleAmount;
            }
            set
            {
                throttleAmount = value;
            }
        }

        internal int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }

        internal int OrganizationID
        {
            get
            {
                return organizationID;
            }
            set
            {
                organizationID = value;
            }
        }

        internal int UserOrganizationID
        {
            get
            {
                return userOrganizationID;
            }
            set
            {
                userOrganizationID = value;
            }
        }

        public bool HaveError
        {
            get
            {
                return haveError;
            }
            set
            {
                haveError = value;
            }
        }

        public bool HaveEXCELReadError
        {
            get
            {
                return haveEXCELReadError;
            }
            set
            {
                haveEXCELReadError = value;
            }
        }

        internal bool InvalidUser
        {
            get
            {
                return invalidUser;
            }
            set
            {
                invalidUser = value;
            }
        }

        internal bool SuperUser
        {
            get
            {
                return superUser;
            }
            set
            {
                superUser = value;
            }
        }

        internal string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                statusMessage = value;
            }
        }

        #endregion

        public BulkConfiguratorQueue()
        {
        }

        private PortalWebAppContext _databaseContext;
        public BulkConfiguratorQueue(string conn, string excelfilename, int userid, int recordthrottle, int throttleamount, bool checkrtu, PortalWebAppContext databaseContext)
        {
            _databaseContext = databaseContext;
            this.ConnectionString = conn;
            this.FileName = excelfilename;
            this.OleDBConnectionString = BuildOleDbConnectionString();
            this.UserID = userid;
            this.RecordThrottle = recordthrottle;
            this.ThrottleAmount = throttleamount;
            wroteErrorFileHeadings = false;
            checkRTUCondition = checkrtu;
            //this.ErrorFilePath = "C:\\BulkConfig\\ErrorFile\\";
            this.ErrorFilePath = AppDomain.CurrentDomain.BaseDirectory + "ErrorFile";
            //this.ErrorFileName = this.ErrorFilePath + "Errors_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
            this.ErrorFileName = this.ErrorFilePath + "\\" + "Errors_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
            //this.StatusFilePath = "C:\\BulkConfig\\SummaryFile\\";
            this.StatusFilePath = AppDomain.CurrentDomain.BaseDirectory + "SummaryFile";
            //this.StatusFileName = this.StatusFilePath + "Summary_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".txt";
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
                    this.TotalEXCELCount = myTankConfigs.Count;
                    //myThread = new Thread(new ThreadStart(ValidateTheEXCELFile));
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
            bool directoryExists = false;
            try
            {
                directoryExists = Directory.Exists(this.ErrorFilePath);
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

        private void GetUserOrganization()
        {

            bool hasglobalorgsecurity = false;
            int paramUserId = 0;
            try
            {
                var userList = (from user in _databaseContext.User
                                where user.UserId == paramUserId
                                orderby user.UserId
                                select new
                                {
                                    OrganizationID = user.OrganizationID,
                                    HasGlobalOrgSecurity = user.HasGlobalOrgSecurity
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

             //   sb.Append("select organizationid, hasglobalorgsecurity ");
              //  sb.Append("from [user] ");
               // sb.Append("where userid = @userid");
                //SqlParameter[] paramArray = new SqlParameter[1];
                //paramArray[0] = DAL.Parameter("@userid", this.UserID);
                //SqlDataReader dr = DAL.ReturnDataReader(sb.ToString(), this.ConnectionString, paramArray, true, 20);
                //if (dr.HasRows)
                //{
                //    while (dr.Read())
                //    {
                //        this.UserOrganizationID = dr.GetInt32(0);
                //        hasglobalorgsecurity = dr.GetBoolean(1);
                //    }
                //    dr.Close();
                //    if (this.UserOrganizationID == 10 && hasglobalorgsecurity)
                        this.SuperUser = true;
               // }
                //else
                //{
                //    this.InvalidUser = true;
                //    this.HaveError = true;
                //}
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
               // FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at GetUserOrganization - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
            }
        }

        private void BuildListOfUserOrganizations()
        {
            try
            {
                this.UserOrganizationID = 10;
                myUserOrganizations = new List<Organization>();
             
                var userList = (from orgTree in _databaseContext.OrganizationTree
                                where orgTree.OrgID == this.UserOrganizationID
                                select new { ChildOrgID=orgTree.ChildOrgID }).ToList();
                
                //sb.Length = 0;
                //sb.Append("select childorgid ");
                //sb.Append("from organizationtree ");
                //sb.Append("where orgid = @organizationid");
                //SqlParameter[] paramArray = new SqlParameter[1];
                //paramArray[0] = DAL.Parameter("@organizationid", this.UserOrganizationID);
                //SqlDataReader dr = DAL.ReturnDataReader(sb.ToString(), this.ConnectionString, paramArray, true, 20);
                //while (dr.Read())
                //{
                //    myUserOrganizations.Add(dr.GetInt32(0));
                //}
                //dr.Close();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at BuildListOfUserOrganizations - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
            }
        }

        private void ReadEXCELFile()
        {
            string result = string.Empty;
            string test = string.Empty;
            try
            {
                sb.Length = 0;
                sb.Append("select [TankID], [RTUNumber], [TankName], [TankHgt],	[TankCap], [CapacityLimit], ");
                sb.Append("[TankMinimum], [ReorderUsage], [SafetyStockUsage], [StartTime], [Callsperday], [CallDay], ");
                sb.Append("[Interval], [DiagCallDayMask], [HighSetPoint], [LowSetPoint], [SensorOffset], [CoeffExp], ");
                sb.Append("[SpecGrav], [LowLowLevel], [LowLevel], [HighLevel], [HighHighLevel], [FillDetectDelta], ");
                sb.Append("[ShortFillDelta], [VolumeDelta],	[RateChangeDelta], [DeviceCriticalLowLevel], [DeviceLowLevel], ");
                sb.Append("[DeviceHighLevel], [DeviceCriticalHighLevel], [DeviceFillDetect], [DeviceFillDetectDelta], ");
                sb.Append("[DeviceFillHysteresis], [DataLogDelta], [UsageDelta], [WakeInterval], [DeviceUsageAlarm], ");
                sb.Append("[HasExpectedCallAlarm], [TankNormallyFills] ");
                //new code added to accomodate EnableLocation and EnableGPS
                //D Arcilla
                //Oct 2020
                sb.Append(",[EnableGPS], [EnableLocation] ");
                sb.Append("from [Sheet1$]");
              //  OleDbDataReader dr = DAL.ReturnOleDbDataReader2(sb.ToString(), this.OleDBConnectionString, null, true, ref result);
                //MessageBox.Show("Error: " + result);
                //if (dr == null)
                //{
                //    this.HaveEXCELReadError = true;
                //}
                //if (!this.HaveEXCELReadError)
                //{
                //    int tankIDOrdinal = dr.GetOrdinal("TankID");
                //    int tankNameOrdinal = dr.GetOrdinal("TankName");
                //    int rtuNumberOrdinal = dr.GetOrdinal("RTUNumber");
                //    int tankHgtOrdinal = dr.GetOrdinal("TankHgt");
                //    int tankCapOrdinal = dr.GetOrdinal("TankCap");
                //    int capacityLimitOrdinal = dr.GetOrdinal("CapacityLimit");
                //    int tankMinimumOrdinal = dr.GetOrdinal("TankMinimum");
                //    int reorderUsageOrdinal = dr.GetOrdinal("ReOrderUsage");
                //    int safetyStockUsageOrdinal = dr.GetOrdinal("SafetyStockUsage");
                //    int lowLowLevelOrdinal = dr.GetOrdinal("LowLowLevel");
                //    int lowLevelOrdinal = dr.GetOrdinal("LowLevel");
                //    int highLevelOrdinal = dr.GetOrdinal("HighLevel");
                //    int highHighLevelOrdinal = dr.GetOrdinal("HighHighLevel");
                //    int fillDetectDeltaOrdinal = dr.GetOrdinal("FillDetectDelta");
                //    int shortFillDeltaOrdinal = dr.GetOrdinal("ShortFillDelta");
                //    int volumeDeltaOrdinal = dr.GetOrdinal("VolumeDelta");
                //    int rateChangeDeltaOrdinal = dr.GetOrdinal("RateChangeDelta");
                //    int callsPerDayOrdinal = dr.GetOrdinal("CallsPerDay");
                //    int callDayOrdinal = dr.GetOrdinal("CallDay");
                //    int intervalOrdinal = dr.GetOrdinal("Interval");
                //    int diagCallDayMaskOrdinal = dr.GetOrdinal("DiagCallDayMask");
                //    int dataLogDeltaOrdinal = dr.GetOrdinal("DataLogDelta");
                //    int usageDeltaOrdinal = dr.GetOrdinal("UsageDelta");
                //    int wakeIntervalOrdinal = dr.GetOrdinal("WakeInterval");
                //    int startTimeOrdinal = dr.GetOrdinal("StartTime");
                //    int highSetPointOrdinal = dr.GetOrdinal("HighSetPoint");
                //    int lowSetPointOrdinal = dr.GetOrdinal("LowSetPoint");
                //    int sensorOffsetOrdinal = dr.GetOrdinal("SensorOffset");
                //    int coeffExpOrdinal = dr.GetOrdinal("CoeffExp");
                //    int specGravOrdinal = dr.GetOrdinal("SpecGrav");
                //    int deviceFillDetectDeltaOrdinal = dr.GetOrdinal("DeviceFillDetectDelta");
                //    int deviceFillHysteresisOrdinal = dr.GetOrdinal("DeviceFillHysteresis");
                //    int deviceCriticalLowLevelOrdinal = dr.GetOrdinal("DeviceCriticalLowLevel");
                //    int deviceLowLevelOrdinal = dr.GetOrdinal("DeviceLowLevel");
                //    int deviceHighLevelOrdinal = dr.GetOrdinal("DeviceHighLevel");
                //    int deviceCriticalHighLevelOrdinal = dr.GetOrdinal("DeviceCriticalHighLevel");
                //    int deviceFillDetectOrdinal = dr.GetOrdinal("DeviceFillDetect");
                //    int deviceUsageAlarmOrdinal = dr.GetOrdinal("DeviceUsageAlarm");
                //    int hasExpectedCallAlarmOrdinal = dr.GetOrdinal("HasExpectedCallAlarm");
                //    int tankNormallyFillsOrdinal = dr.GetOrdinal("TankNormallyFills");
                //    //new code added to accomodate EnableLocation and EnableGPS+
                //    //D Arcilla
                //    //Oct 2020
                //    int enableLocationOrdinal = dr.GetOrdinal("EnableLocation");
                //    int enableGPSOrdinal = dr.GetOrdinal("EnableGPS");
                //    if (dr.HasRows)
                //    {
                //        myTankConfigs = new List<TankConfig>();
                //        while (dr.Read())
                //        {
                //          //  myTankConfig = new TankConfig(this.ConnectionString, this.UserID);
                //          //  myTankConfig.ErrorFilePath = this.ErrorFilePath;
                //           // myTankConfig.CheckRTUCondition = checkRTUCondition;
                //            //if (dr.GetValue(tankIDOrdinal) == DBNull.Value)
                //            //    myTankConfig.TankID = "*** Empty ***";
                //            //else
                //            //    myTankConfig.TankID = dr.GetValue(tankIDOrdinal).ToString();
                //            //if (dr.GetValue(rtuNumberOrdinal) == DBNull.Value)
                //            //    myTankConfig.RTUNumber = "*** Empty ***";
                //            //else
                //            //    myTankConfig.RTUNumber = dr.GetValue(rtuNumberOrdinal).ToString();
                //            //if (dr.GetValue(tankNameOrdinal) == DBNull.Value)
                //            //    myTankConfig.TankName = "*** Empty ***";
                //            //else
                //            //    myTankConfig.TankName = dr.GetValue(tankNameOrdinal).ToString();
                //            //if (dr.GetValue(tankHgtOrdinal) == DBNull.Value)
                //            //    myTankConfig.TankHgt = "*** Empty ***";
                //            //else
                //            //    myTankConfig.TankHgt = dr.GetValue(tankHgtOrdinal).ToString();
                //            //if (dr.GetValue(tankCapOrdinal) == DBNull.Value)
                //            //    myTankConfig.TankCap = "*** Empty ***";
                //            //else
                //            //    myTankConfig.TankCap = dr.GetValue(tankCapOrdinal).ToString();
                //            //if (dr.GetValue(capacityLimitOrdinal) == DBNull.Value)
                //            //    myTankConfig.CapacityLimit = "*** Empty ***";
                //            //else
                //            //    myTankConfig.CapacityLimit = dr.GetValue(capacityLimitOrdinal).ToString();
                //            //if (dr.GetValue(tankMinimumOrdinal) == DBNull.Value)
                //            //    myTankConfig.TankMinimum = "*** Empty ***";
                //            //else
                //            //    myTankConfig.TankMinimum = dr.GetValue(tankMinimumOrdinal).ToString();
                //            //if (dr.GetValue(reorderUsageOrdinal) == DBNull.Value)
                //            //    myTankConfig.ReorderUsage = "*** Empty ***";
                //            //else
                //            //    myTankConfig.ReorderUsage = dr.GetValue(reorderUsageOrdinal).ToString();
                //            //if (dr.GetValue(safetyStockUsageOrdinal) == DBNull.Value)
                //            //    myTankConfig.SafetyStockUsage = "*** Empty ***";
                //            //else
                //            //    myTankConfig.SafetyStockUsage = dr.GetValue(safetyStockUsageOrdinal).ToString();
                //            //if (dr.GetValue(lowLowLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.LowLowLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.LowLowLevel = dr.GetValue(lowLowLevelOrdinal).ToString();
                //            //if (dr.GetValue(lowLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.LowLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.LowLevel = dr.GetValue(lowLevelOrdinal).ToString();
                //            //if (dr.GetValue(highLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.HighLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.HighLevel = dr.GetValue(highLevelOrdinal).ToString();
                //            //if (dr.GetValue(highHighLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.HighHighLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.HighHighLevel = dr.GetValue(highHighLevelOrdinal).ToString();
                //            //if (dr.GetValue(fillDetectDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.FillDetectDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.FillDetectDelta = dr.GetValue(fillDetectDeltaOrdinal).ToString();
                //            //if (dr.GetValue(shortFillDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.ShortFillDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.ShortFillDelta = dr.GetValue(shortFillDeltaOrdinal).ToString();
                //            //if (dr.GetValue(volumeDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.VolumeDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.VolumeDelta = dr.GetValue(volumeDeltaOrdinal).ToString();
                //            //if (dr.GetValue(rateChangeDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.RateChangeDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.RateChangeDelta = dr.GetValue(rateChangeDeltaOrdinal).ToString();
                //            //if (dr.GetValue(callsPerDayOrdinal) == DBNull.Value)
                //            //    myTankConfig.CallsPerDay = "*** Empty ***";
                //            //else
                //            //    myTankConfig.CallsPerDay = dr.GetValue(callsPerDayOrdinal).ToString(); ;
                //            //if (dr.GetValue(callDayOrdinal) == DBNull.Value)
                //            //    myTankConfig.CallDay = "*** Empty ***";
                //            //else
                //            //    myTankConfig.CallDay = dr.GetValue(callDayOrdinal).ToString();
                //            //if (dr.GetValue(intervalOrdinal) == DBNull.Value)
                //            //    myTankConfig.Interval = "*** Empty ***";
                //            //else
                //            //    myTankConfig.Interval = dr.GetValue(intervalOrdinal).ToString();
                //            //if (dr.GetValue(diagCallDayMaskOrdinal) == DBNull.Value)
                //            //    myTankConfig.DiagCallDayMask = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DiagCallDayMask = dr.GetValue(diagCallDayMaskOrdinal).ToString();
                //            //if (dr.GetValue(dataLogDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.DataLogDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DataLogDelta = dr.GetValue(dataLogDeltaOrdinal).ToString();
                //            //if (dr.GetValue(usageDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.UsageDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.UsageDelta = dr.GetValue(usageDeltaOrdinal).ToString();
                //            //if (dr.GetValue(wakeIntervalOrdinal) == DBNull.Value)
                //            //    myTankConfig.WakeInterval = "*** Empty ***";
                //            //else
                //            //    myTankConfig.WakeInterval = dr.GetValue(wakeIntervalOrdinal).ToString();
                //            //if (dr.GetValue(startTimeOrdinal) == DBNull.Value)
                //            //    myTankConfig.StartTime = "*** Empty ***";
                //            //else
                //            //    myTankConfig.StartTime = dr.GetValue(startTimeOrdinal).ToString();
                //            //if (dr.GetValue(highSetPointOrdinal) == DBNull.Value)
                //            //    myTankConfig.HighSetPoint = "*** Empty ***";
                //            //else
                //            //    myTankConfig.HighSetPoint = dr.GetValue(highSetPointOrdinal).ToString();
                //            //if (dr.GetValue(lowSetPointOrdinal) == DBNull.Value)
                //            //    myTankConfig.LowSetPoint = "*** Empty ***";
                //            //else
                //            //    myTankConfig.LowSetPoint = dr.GetValue(lowSetPointOrdinal).ToString();
                //            //if (dr.GetValue(sensorOffsetOrdinal) == DBNull.Value)
                //            //    myTankConfig.SensorOffset = "*** Empty ***";
                //            //else
                //            //    myTankConfig.SensorOffset = dr.GetValue(sensorOffsetOrdinal).ToString();
                //            //if (dr.GetValue(coeffExpOrdinal) == DBNull.Value)
                //            //    myTankConfig.CoeffExp = "*** Empty ***";
                //            //else
                //            //    myTankConfig.CoeffExp = dr.GetValue(coeffExpOrdinal).ToString();
                //            //if (dr.GetValue(specGravOrdinal) == DBNull.Value)
                //            //    myTankConfig.SpecGrav = "*** Empty ***";
                //            //else
                //            //    myTankConfig.SpecGrav = dr.GetValue(specGravOrdinal).ToString();
                //            //if (dr.GetValue(deviceFillDetectDeltaOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceFillDetectDelta = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceFillDetectDelta = dr.GetValue(deviceFillDetectDeltaOrdinal).ToString();
                //            //if (dr.GetValue(deviceFillHysteresisOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceFillHysteresis = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceFillHysteresis = dr.GetValue(deviceFillHysteresisOrdinal).ToString();
                //            //if (dr.GetValue(deviceCriticalLowLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceCriticalLowLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceCriticalLowLevel = dr.GetValue(deviceCriticalLowLevelOrdinal).ToString();
                //            //if (dr.GetValue(deviceLowLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceLowLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceLowLevel = dr.GetValue(deviceLowLevelOrdinal).ToString();
                //            //if (dr.GetValue(deviceHighLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceHighLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceHighLevel = dr.GetValue(deviceHighLevelOrdinal).ToString();
                //            //if (dr.GetValue(deviceCriticalHighLevelOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceCriticalHighLevel = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceCriticalHighLevel = dr.GetValue(deviceCriticalHighLevelOrdinal).ToString();
                //            //if (dr.GetValue(deviceFillDetectOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceFillDetect = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceFillDetect = dr.GetValue(deviceFillDetectOrdinal).ToString();
                //            //if (dr.GetValue(deviceUsageAlarmOrdinal) == DBNull.Value)
                //            //    myTankConfig.DeviceUsageAlarm = "*** Empty ***";
                //            //else
                //            //    myTankConfig.DeviceUsageAlarm = dr.GetValue(deviceUsageAlarmOrdinal).ToString();
                //            //if (dr.GetValue(hasExpectedCallAlarmOrdinal) == DBNull.Value)
                //            //    myTankConfig.HasExpectedCallAlarm = "*** Empty ***";
                //            //else
                //            //    myTankConfig.HasExpectedCallAlarm = dr.GetValue(hasExpectedCallAlarmOrdinal).ToString();
                //            //if (dr.GetValue(tankNormallyFillsOrdinal) == DBNull.Value)
                //            //    myTankConfig.TankNormallyFills = "*** Empty ***";
                //            //else
                //            //    myTankConfig.TankNormallyFills = dr.GetValue(tankNormallyFillsOrdinal).ToString();
                //            ////new code added to accomodate EnableLocation and EnableGPS
                //            //D Arcilla
                //            //Oct 2020
                //            //if (dr.GetValue(enableLocationOrdinal) == DBNull.Value)
                //            //    myTankConfig.EnableLocation = "*** Empty ***";
                //            //else
                //            //    myTankConfig.EnableLocation = dr.GetValue(enableLocationOrdinal).ToString();
                //            //if (dr.GetValue(enableGPSOrdinal) == DBNull.Value)
                //            //    myTankConfig.EnableGPS = "*** Empty ***";
                //            //else
                //            //    myTankConfig.EnableGPS = dr.GetValue(enableGPSOrdinal).ToString();
                //            myTankConfigs.Add(myTankConfig);
                //        }
                //        dr.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at ReadEXCELFile - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at ValidateTheEXCELFile - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at RequiredColumnsCheck - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at WriteErrorReport - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at DataTypeCheck - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at ValidateTankIDRTUNumber - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
               // FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at ValidateTankScope - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
               // FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at BuildConfigValuesCheck - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at ApplyTankConfigChanges - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
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