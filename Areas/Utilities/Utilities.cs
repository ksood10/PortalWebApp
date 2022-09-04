using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Linq;
using System.Threading;
using System.Data.OleDb;
using System.IO;
using PortalWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PortalWebApp.Utilities { 
    public class Util
    {
        public const string SPBulkInsert = "BulkTankConfig_Insert";
        public static float psiPerCubicInch = 27.729623F;
        public static int tankIDOrdinal { get; set; }
        public static int tankNameOrdinal { get; set; }
        public static int rtuNumberOrdinal { get; set; }
        public static int tankHgtOrdinal  { get; set; }
        public static int tankCapOrdinal  { get; set; }
        public static int capacityLimitOrdinal  { get; set; }
        public static int tankMinimumOrdinal  { get; set; }
        public static int reorderUsageOrdinal  { get; set; }
        public static int safetyStockUsageOrdinal  { get; set; }
        public static int lowLowLevelOrdinal { get; set; }
        public static int lowLevelOrdinal  { get; set; }
        public static int highLevelOrdinal  { get; set; }
        public static int highHighLevelOrdinal { get; set; }
        public static int fillDetectDeltaOrdinal { get; set; }
        public static int shortFillDeltaOrdinal { get; set; }
        public static int volumeDeltaOrdinal { get; set; }
        public static int rateChangeDeltaOrdinal { get; set; }
        public static int callsPerDayOrdinal { get; set; }
        public static int callDayOrdinal { get; set; }
        public static int intervalOrdinal { get; set; }
        public static int diagCallDayMaskOrdinal { get; set; }
        public static int dataLogDeltaOrdinal { get; set; }
        public static int usageDeltaOrdinal { get; set; }
        public static int wakeIntervalOrdinal { get; set; }
        public static int startTimeOrdinal { get; set; }
        public static int highSetPointOrdinal { get; set; }
        public static int lowSetPointOrdinal { get; set; }
        public static int sensorOffsetOrdinal { get; set; }
        public static int coeffExpOrdinal { get; set; }
        public static int specGravOrdinal { get; set; }
        public static int deviceFillDetectDeltaOrdinal { get; set; }
        public static int deviceFillHysteresisOrdinal { get; set; }
        public static int deviceCriticalLowLevelOrdinal { get; set; }
        public static int deviceLowLevelOrdinal { get; set; }
        public static int deviceHighLevelOrdinal { get; set; }
        public static int deviceCriticalHighLevelOrdinal { get; set; }
        public static int deviceFillDetectOrdinal { get; set; }
        public static int deviceUsageAlarmOrdinal { get; set; }
        public static int hasExpectedCallAlarmOrdinal { get; set; }
        public static int tankNormallyFillsOrdinal { get; set; }
        public static int enableGPSOrdinal { get; set; }
        public static int enableLocationOrdinal { get; set; }

        public sealed class Env
        {
            public static readonly Env Dev = new Env("Server=TankdataLSN1\\TankData;Database=TankData_TDG;User ID=EmailManager;pwd=tanklink5410");
            public static readonly Env Prod = new Env("Server=Prod");

            private Env(string value)
            {
                Value = value;
            }

            public string Value { get; private set; }
        }

        public static bool ValidateApplication()
        {
            bool multipleInstances = false;
            try
            {
                string myProcess;
                System.Diagnostics.Process[] allProcesses = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in allProcesses)
                {
                    if (p.Id == System.Diagnostics.Process.GetCurrentProcess().Id)
                        myProcess = p.Id.ToString();
                    if (p.ProcessName == System.Diagnostics.Process.GetCurrentProcess().ProcessName && p.Id != System.Diagnostics.Process.GetCurrentProcess().Id)
                        multipleInstances = true;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return true;
            }
            return multipleInstances;
        }

        public static DataTable GetDataTableFromExcelFile(BulkUpdate model)
        {
            var filename = Path.GetFileName(model.FileName);
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            //create directory "Uploads" if it doesn't exists
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);
            //get file path 
            var filePath = Path.Combine(MainPath, filename);
            var conString = string.Empty;

            switch (Path.GetExtension(filename))
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            var dt = new DataTable();
            conString = string.Format(conString, filePath);
            try
            {
                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using OleDbCommand cmdExcel = new OleDbCommand();
                    using OleDbDataAdapter odaExcel = new OleDbDataAdapter();
                    cmdExcel.Connection = connExcel;

                    //Get the name of First Sheet.
                    connExcel.Open();
                    var dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                    odaExcel.SelectCommand = cmdExcel;
                    odaExcel.Fill(dt);
                    connExcel.Close();
                }
            } catch (Exception e)
            {
                dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Error");
                dt.Columns.Add("ErrorDetail");
                var dr = dt.NewRow();
                dr["Error"] = "File Read Error!";
                dr["ErrorDetail"] = e.Message;
                dt.Rows.Add(dr);
                return dt;
            }
           
            return dt;
        }

        public static SqlParameter[] GetSqlParams(DataRow dr)
        {
            var prm= new SqlParameter[]
            {
                new SqlParameter("@TankID", SqlDbType.Int)                      { Value = dr[tankIDOrdinal] },
                new SqlParameter("@RTUNumber", SqlDbType.NVarChar)              { Value = dr[rtuNumberOrdinal] },
                new SqlParameter("@TankHgt", SqlDbType.Decimal)                 { Value = (dr[tankHgtOrdinal] == DBNull.Value) ? 0 : dr[tankHgtOrdinal] },
                new SqlParameter("@TankCap", SqlDbType.Decimal)                 { Value = (dr[tankCapOrdinal] == DBNull.Value) ? 0 : dr[tankCapOrdinal] },
                new SqlParameter("@CapacityLimit", SqlDbType.Decimal)           { Value = (dr[capacityLimitOrdinal] == DBNull.Value) ? 0 : dr[capacityLimitOrdinal] },
                new SqlParameter("@TankMinimum", SqlDbType.Decimal)             { Value = (dr[tankMinimumOrdinal] == DBNull.Value) ? 0 : dr[tankMinimumOrdinal] },

                new SqlParameter("@ReorderUsage", SqlDbType.Int)                { Value = (dr[reorderUsageOrdinal] == DBNull.Value) ? 0 : dr[reorderUsageOrdinal] },
                new SqlParameter("@SafetyStockUsage", SqlDbType.Int)            { Value = (dr[safetyStockUsageOrdinal] == DBNull.Value) ? 0 : dr[safetyStockUsageOrdinal] },
                new SqlParameter("@Callsperday", SqlDbType.Int)                 { Value = (dr[callsPerDayOrdinal] == DBNull.Value) ? 0 : dr[callsPerDayOrdinal] },
                new SqlParameter("@CallDay", SqlDbType.Int)                     { Value = (dr[callDayOrdinal] == DBNull.Value) ? 0 : dr[callDayOrdinal] },

                new SqlParameter("@StartTime", SqlDbType.DateTime)              { Value = (dr[startTimeOrdinal] == DBNull.Value) ? 0 : dr[startTimeOrdinal] },
                new SqlParameter("@Interval", SqlDbType.NVarChar)               { Value = (dr[intervalOrdinal] == DBNull.Value) ? 0 : dr[intervalOrdinal] },
                new SqlParameter("@DiagCallDayMask", SqlDbType.Int)             { Value = (dr[diagCallDayMaskOrdinal] == DBNull.Value) ? 0 : dr[diagCallDayMaskOrdinal] },
                new SqlParameter("@HighSetPoint", SqlDbType.Decimal)            { Value = (dr[highSetPointOrdinal] == DBNull.Value) ? 0 : dr[highSetPointOrdinal] },
                new SqlParameter("@LowSetPoint", SqlDbType.Decimal)             { Value = (dr[lowSetPointOrdinal] == DBNull.Value) ? 0 : dr[lowSetPointOrdinal] },

                new SqlParameter("@SensorOffset", SqlDbType.Decimal)            { Value = (dr[sensorOffsetOrdinal] == DBNull.Value) ? 0 : dr[sensorOffsetOrdinal] },
                new SqlParameter("@CoeffExp", SqlDbType.Decimal)                { Value = (dr[coeffExpOrdinal] == DBNull.Value) ? 0 : dr[coeffExpOrdinal] },
                new SqlParameter("@SpecGrav", SqlDbType.Decimal)                { Value = (dr[specGravOrdinal] == DBNull.Value) ? 0 : dr[specGravOrdinal] },
                new SqlParameter("@LowLowLevel", SqlDbType.Int)                 { Value = (dr[lowLowLevelOrdinal] == DBNull.Value) ? 0 : dr[lowLowLevelOrdinal] },
                new SqlParameter("@LowLevel", SqlDbType.Int)                    { Value = (dr[lowLevelOrdinal] == DBNull.Value) ? 0 : dr[lowLevelOrdinal] },

                new SqlParameter("@HighLevel", SqlDbType.Int)                       { Value = (dr[highLevelOrdinal] == DBNull.Value) ? 0 : dr[highLevelOrdinal] },
                new SqlParameter("@HighHighLevel", SqlDbType.Int)               { Value = (dr[highHighLevelOrdinal] == DBNull.Value) ? 0 : dr[highHighLevelOrdinal] },
                new SqlParameter("@ShortFillDelta", SqlDbType.Decimal)          { Value = (dr[shortFillDeltaOrdinal] == DBNull.Value) ? 0 : dr[shortFillDeltaOrdinal] },
                new SqlParameter("@FillDetectDelta", SqlDbType.Decimal)         { Value = (dr[fillDetectDeltaOrdinal] == DBNull.Value) ? 0 : dr[fillDetectDeltaOrdinal] },
                new SqlParameter("@VolumeDelta", SqlDbType.Int)                 { Value = (dr[volumeDeltaOrdinal] == DBNull.Value) ? 0 : dr[volumeDeltaOrdinal] },

                new SqlParameter("@RateChangeDelta", SqlDbType.Int)                 { Value = (dr[rateChangeDeltaOrdinal] == DBNull.Value) ? 0 : dr[rateChangeDeltaOrdinal] },
                new SqlParameter("@DeviceCriticalLowLevel", SqlDbType.Bit)      { Value = (dr[deviceCriticalLowLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceCriticalLowLevelOrdinal] },
                new SqlParameter("@DeviceLowLevel", SqlDbType.Bit)              { Value = (dr[deviceLowLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceLowLevelOrdinal] },
                new SqlParameter("@DeviceHighLevel", SqlDbType.Bit)             { Value = (dr[deviceHighLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceHighLevelOrdinal] },
                new SqlParameter("@DeviceCriticalHighLevel", SqlDbType.Bit)     { Value = (dr[deviceCriticalHighLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceCriticalHighLevelOrdinal] },
                
                new SqlParameter("@DeviceFillDetect", SqlDbType.Bit)                { Value = (dr[deviceFillDetectOrdinal] == DBNull.Value) ? 0 : dr[deviceFillDetectOrdinal] },
                new SqlParameter("@DeviceFillDetectDelta", SqlDbType.Decimal)   { Value = (dr[deviceFillDetectDeltaOrdinal] == DBNull.Value) ? 0 : dr[deviceFillDetectDeltaOrdinal] },
                new SqlParameter("@DeviceFillHysteresis", SqlDbType.Bit)        { Value = (dr[deviceFillHysteresisOrdinal] == DBNull.Value) ? 0 : dr[deviceFillHysteresisOrdinal] },
                new SqlParameter("@DataLogDelta", SqlDbType.Int)                { Value = (dr[dataLogDeltaOrdinal] == DBNull.Value) ? 0 : dr[dataLogDeltaOrdinal] },
                new SqlParameter("@UsageDelta", SqlDbType.Bit)                  { Value = (dr[usageDeltaOrdinal] == DBNull.Value) ? 0 : dr[usageDeltaOrdinal] },
                new SqlParameter("@WakeInterval", SqlDbType.Int)                { Value = (dr[wakeIntervalOrdinal] == DBNull.Value) ? 0 : dr[wakeIntervalOrdinal] },
               // new SqlParameter("@DeviceUsageAlarm ", SqlDbType.Bit),        { Value = (dr[tankHgtOrdinal] == DBNull.Value) ? 0 : dr[tankHgtOrdinal] },
                new SqlParameter("@HasExpectedCallAlarm", SqlDbType.Bit)        { Value = (dr[hasExpectedCallAlarmOrdinal] == DBNull.Value) ? 0 : dr[hasExpectedCallAlarmOrdinal] },
                new SqlParameter("@TankNormallyFills", SqlDbType.Bit)           { Value = (dr[tankNormallyFillsOrdinal] == DBNull.Value) ? 0 : dr[tankHgtOrdinal] },
                new SqlParameter("@EnableGPS", SqlDbType.Bit)                   { Value = (dr[enableGPSOrdinal] == DBNull.Value) ? 0 : dr[enableGPSOrdinal] },
                new SqlParameter("@EnableLocation", SqlDbType.Bit)              { Value = (dr[enableLocationOrdinal] == DBNull.Value) ? 0 : dr[enableLocationOrdinal] }
        };
            return prm;
        }
  

        public static void GetColumnOrdinals(DataTable dt)
        {
             tankIDOrdinal = dt.Columns["TankID"].Ordinal;
             tankNameOrdinal = dt.Columns["TankName"].Ordinal;
             rtuNumberOrdinal = dt.Columns["RTUNumber"].Ordinal;
             tankHgtOrdinal = dt.Columns["TankHgt"].Ordinal;
             tankCapOrdinal = dt.Columns["TankCap"].Ordinal;
             capacityLimitOrdinal = dt.Columns["CapacityLimit"].Ordinal;
             tankMinimumOrdinal = dt.Columns["TankMinimum"].Ordinal;
             reorderUsageOrdinal = dt.Columns["ReOrderUsage"].Ordinal;
             safetyStockUsageOrdinal = dt.Columns["SafetyStockUsage"].Ordinal;
             lowLowLevelOrdinal = dt.Columns["LowLowLevel"].Ordinal;
             lowLevelOrdinal = dt.Columns["LowLevel"].Ordinal;
             highLevelOrdinal = dt.Columns["HighLevel"].Ordinal;
             highHighLevelOrdinal = dt.Columns["HighHighLevel"].Ordinal;
             fillDetectDeltaOrdinal = dt.Columns["FillDetectDelta"].Ordinal;
             shortFillDeltaOrdinal = dt.Columns["ShortFillDelta"].Ordinal;
             volumeDeltaOrdinal = dt.Columns["VolumeDelta"].Ordinal;
             rateChangeDeltaOrdinal = dt.Columns["RateChangeDelta"].Ordinal;
             callsPerDayOrdinal = dt.Columns["CallsPerDay"].Ordinal;
             callDayOrdinal = dt.Columns["CallDay"].Ordinal;
             intervalOrdinal = dt.Columns["Interval"].Ordinal;
             diagCallDayMaskOrdinal = dt.Columns["DiagCallDayMask"].Ordinal;
             dataLogDeltaOrdinal = dt.Columns["DataLogDelta"].Ordinal;
             usageDeltaOrdinal = dt.Columns["UsageDelta"].Ordinal;
             wakeIntervalOrdinal = dt.Columns["WakeInterval"].Ordinal;
             startTimeOrdinal = dt.Columns["StartTime"].Ordinal;
             highSetPointOrdinal = dt.Columns["HighSetPoint"].Ordinal;
             lowSetPointOrdinal = dt.Columns["LowSetPoint"].Ordinal;
             sensorOffsetOrdinal = dt.Columns["SensorOffset"].Ordinal;
             coeffExpOrdinal = dt.Columns["CoeffExp"].Ordinal;
             specGravOrdinal = dt.Columns["SpecGrav"].Ordinal;
             deviceFillDetectDeltaOrdinal = dt.Columns["DeviceFillDetectDelta"].Ordinal;
             deviceFillHysteresisOrdinal = dt.Columns["DeviceFillHysteresis"].Ordinal;
             deviceCriticalLowLevelOrdinal = dt.Columns["DeviceCriticalLowLevel"].Ordinal;
             deviceLowLevelOrdinal = dt.Columns["DeviceLowLevel"].Ordinal;
             deviceHighLevelOrdinal = dt.Columns["DeviceHighLevel"].Ordinal;
             deviceCriticalHighLevelOrdinal = dt.Columns["DeviceCriticalHighLevel"].Ordinal;
             deviceFillDetectOrdinal = dt.Columns["DeviceFillDetect"].Ordinal;
             deviceUsageAlarmOrdinal = dt.Columns["DeviceUsageAlarm"].Ordinal;
             hasExpectedCallAlarmOrdinal = dt.Columns["HasExpectedCallAlarm"].Ordinal;
             tankNormallyFillsOrdinal = dt.Columns["TankNormallyFills"].Ordinal;
            //new code added to accomodate EnableLocation and EnableGPS+   D Arcilla    Oct 2020
             enableLocationOrdinal = dt.Columns["EnableLocation"].Ordinal;
             enableGPSOrdinal = dt.Columns["EnableGPS"].Ordinal;
        }
       

        public static int MinutesToMilliseconds(int minutes)
        {
            int milliseconds = 0;
            try
            {
                milliseconds = minutes * 60000;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                milliseconds = -1;
            }
            return milliseconds;
        }

        public static bool Connect(string dmzConnString)
        {
            bool connected = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(dmzConnString))
                {
                    conn.Open();
                    conn.Close();
                    connected = true;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                connected = false;
            }
            return connected;
        }

        public static bool CheckStringLength(string mystring, int mylength, bool required)
        {
            bool isValid = true;
            try
            {
                mystring = mystring.TrimStart();
                if (required)
                    if (mystring == string.Empty)
                        isValid = false;
                if (mystring.Length < mylength)
                    isValid = false;
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToInt(string mystring)
        {
            int testInteger = 0;
            bool isValid = false;
            try
            {
                testInteger = int.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToDecimal(string mystring)
        {
            decimal testDecimal = 0;
            bool isValid = false;
            try
            {
                decimal.TryParse(mystring, out testDecimal);
                isValid = true;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }


        public static bool ConvertStringToBool(string mystring)
        {
            bool testBool = false;
            bool isValid = false;
            try
            {
                testBool = bool.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToFloat(string mystring)
        {
            float testFloat = 0;
            bool isValid = false;
            try
            {
                testFloat = float.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToBigInt(string mystring)
        {
            Int64 testInteger = 0;
            bool isValid = false;
            try
            {
                testInteger = Int64.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToDateTime(string mystring)
        {
            DateTime testDateTime = new DateTime();
            bool isValid = false;
            try
            {
                testDateTime = DateTime.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static void WriteLogFile(string filepath, string location)
        {
            try
            {
                string fileName = filepath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write(location);
                //errorWriter.Close();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
        }

        internal static string ReturnString50(string myString)
        {
            if (myString.Length > 50)
            {
                myString = myString.Substring(0, 49);
            }
            return myString;
        }

        internal static string ReturnString25(string myString)
        {
            if (myString.Length > 25)
            {
                myString = myString.Substring(0, 24);
            }
            return myString;
        }

        internal static string ReturnString225(string myString)
        {
            if (myString.Length > 225)
            {
                myString = myString.Substring(0, 224);
            }
            return myString;
        }

        public static int NthOccurence(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        internal static void Throttle(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
        }

        //modified to include EnableGPS and EnableLocation

        //public static bool UpdateTankConfig(string conn, int tankid, int tankconfigid, int userid, string tankname, string tankhgt, string tankcap,
        //                                    string capacitylimit, string limitcapacityflag, string tankminimum, string reorderusage,
        //                                    string safetystockusage, string starttime, string callsperday, string callday, string interval,
        //                                    string diagcalldaymask, string highsetpoint, string lowsetpoint, string sensoroffset,
        //                                    string coeffexp, string specgrav, string lowlowlevel, string lowlevel, string highlevel,
        //                                    string highhighlevel, string filldetectdelta, string shortfilldelta, string volumedelta,
        //                                    string ratechangedelta, string devicecriticallowlevel, string devicelowlevel,
        //                                    string devicehighlevel, string devicecriticalhighlevel, string devicefilldetect,
        //                                    string devicefilldetectdelta, string devicefillhysteresis, string datalogdelta,
        //                                    string usagedelta, string wakeinterval, string deviceusagealarm, string hasexpectedcallalarm,
        //                                    string tanknormallyfills, string enablegps, string enablelocation)

        //{
        //  //  bool noerrors = false;
        //    bool tankupdateerror = false;
        //    string fileName = string.Empty;
        //    string errorfilepath = AppDomain.CurrentDomain.BaseDirectory + "ErrorFile";
        //    TankData.DBTableAdapters.TankConfigTableAdapter tankConfigTA = null;
        //    TankData.DB.TankConfigDataTable tankConfigDT = null;
        //    TankData.DB.TankConfigRow newTankConfigDR = null;
        //    // int newTankConfigID = 0;
        //    try
        //    {
                //TankData.DB.TankConfigRow currentTankConfigDR = GetCurrentTankConfig(conn, tankconfigid);
                //using (tankConfigTA = new TankData.DBTableAdapters.TankConfigTableAdapter())
                //{
                //    tankConfigTA.Connection.ConnectionString = conn;
                //    using (tankConfigDT = new TankData.DB.TankConfigDataTable())
                //    {
                //        tankConfigDT.Clear();
                //        newTankConfigDR = tankConfigDT.NewTankConfigRow();
                //        newTankConfigDR.Copy(currentTankConfigDR, userid);
                //        newTankConfigDR.BeginEdit();
                //        if (tankname != "*** Empty ***")
                //            newTankConfigDR.TankName = tankname;
                //        if (tankhgt != "*** Empty ***")
                //            newTankConfigDR.TankHgt = decimal.Parse(tankhgt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (tankcap != "*** Empty ***")
                //            newTankConfigDR.TankCap = decimal.Parse(tankcap, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (capacitylimit != "*** Empty ***")
                //            newTankConfigDR.CapacityLimit = decimal.Parse(capacitylimit);
                //        if (!string.IsNullOrEmpty(limitcapacityflag))
                //            newTankConfigDR.LimitCapacityFlag = bool.Parse(limitcapacityflag);
                //        if (tankminimum != "*** Empty ***")
                //            newTankConfigDR.TankMinimum = decimal.Parse(tankminimum, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (reorderusage != "*** Empty ***")
                //            newTankConfigDR.ReorderUsage = int.Parse(reorderusage);
                //        if (safetystockusage != "*** Empty ***")
                //            newTankConfigDR.SafetyStockUsage = int.Parse(safetystockusage);
                //        if (starttime != "*** Empty ***")
                //            newTankConfigDR.StartTime = DateTime.Parse(starttime);
                //        if (callsperday != "*** Empty ***")
                //            newTankConfigDR.Callsperday = int.Parse(callsperday);
                //        if (callday != "*** Empty ***")
                //            newTankConfigDR.CallDay = int.Parse(callday);
                //        if (interval != "*** Empty ***")
                //            newTankConfigDR.Interval = int.Parse(interval);
                //        if (diagcalldaymask != "*** Empty ***")
                //            newTankConfigDR.DiagCallDayMask = int.Parse(diagcalldaymask);
                //        if (highsetpoint != "*** Empty ***")
                //            newTankConfigDR.HighSetPoint = decimal.Parse(highsetpoint, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (lowsetpoint != "*** Empty ***")
                //            newTankConfigDR.LowSetPoint = decimal.Parse(lowsetpoint, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (sensoroffset != "*** Empty ***")
                //            newTankConfigDR.SensorOffset = decimal.Parse(sensoroffset, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (coeffexp != "*** Empty ***")
                //            newTankConfigDR.CoeffExp = decimal.Parse(coeffexp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (specgrav != "*** Empty ***")
                //            newTankConfigDR.SpecGrav = decimal.Parse(specgrav, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (lowlowlevel != "*** Empty ***")
                //            newTankConfigDR.LowLowLevel = int.Parse(lowlowlevel);
                //        if (lowlevel != "*** Empty ***")
                //            newTankConfigDR.LowLevel = int.Parse(lowlevel);
                //        if (highlevel != "*** Empty ***")
                //            newTankConfigDR.HighLevel = int.Parse(highlevel);
                //        if (highhighlevel != "*** Empty ***")
                //            newTankConfigDR.HighHighLevel = int.Parse(highhighlevel);
                //        if (filldetectdelta != "*** Empty ***")
                //            newTankConfigDR.FillDetectDelta = decimal.Parse(filldetectdelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (shortfilldelta != "*** Empty ***")
                //            newTankConfigDR.ShortFillDelta = decimal.Parse(shortfilldelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (volumedelta != "*** Empty ***")
                //            newTankConfigDR.VolumeDelta = int.Parse(volumedelta);
                //        if (ratechangedelta != "*** Empty ***")
                //            newTankConfigDR.RateChangeDelta = int.Parse(ratechangedelta);
                //        if (devicecriticallowlevel != "*** Empty ***")
                //            newTankConfigDR.DeviceCriticalLowLevel = bool.Parse(devicecriticallowlevel);
                //        if (devicelowlevel != "*** Empty ***")
                //            newTankConfigDR.DeviceLowLevel = bool.Parse(devicelowlevel);
                //        if (devicehighlevel != "*** Empty ***")
                //            newTankConfigDR.DeviceHighLevel = bool.Parse(devicehighlevel);
                //        if (devicecriticalhighlevel != "*** Empty ***")
                //            newTankConfigDR.DeviceCriticalHighLevel = bool.Parse(devicecriticalhighlevel);
                //        if (devicefilldetect != "*** Empty ***")
                //            newTankConfigDR.DeviceFillDetect = bool.Parse(devicefilldetect);
                //        if (devicefilldetectdelta != "*** Empty ***")
                //            newTankConfigDR.DeviceFillDetectDelta = decimal.Parse(devicefilldetectdelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (devicefillhysteresis != "*** Empty ***")
                //            newTankConfigDR.DeviceFillHysteresis = decimal.Parse(devicefillhysteresis, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                //        if (datalogdelta != "*** Empty ***")
                //            newTankConfigDR.DataLogDelta = int.Parse(datalogdelta);
                //        if (usagedelta != "*** Empty ***")
                //            newTankConfigDR.UsageDelta = int.Parse(usagedelta);
                //        if (wakeinterval != "*** Empty ***")
                //            newTankConfigDR.WakeInterval = int.Parse(wakeinterval);
                //        if (deviceusagealarm != "*** Empty ***")
                //            newTankConfigDR.DeviceUsageAlarm = bool.Parse(deviceusagealarm);
                //        if (hasexpectedcallalarm != "*** Empty ***")
                //            newTankConfigDR.HasExpectedCallAlarm = bool.Parse(hasexpectedcallalarm);
                //        if (tanknormallyfills != "*** Empty ***")
                //            newTankConfigDR.TankNormallyFills = bool.Parse(tanknormallyfills);
                //        if (enablegps != "*** Empty ***")
                //            newTankConfigDR.EnableGPS = bool.Parse(enablegps);
                //        if (enablelocation != "*** Empty ***")
                //            newTankConfigDR.EnableLocation = bool.Parse(enablelocation);
                //        newTankConfigDR.EndEdit();
                //        tankConfigDT.Rows.Add(newTankConfigDR);
                //        tankConfigTA.Update(tankConfigDT);
                //        newTankConfigID = newTankConfigDR.TankConfigId;
                //        noerrors = false;
                //        tankupdateerror = UpdateTank(noerrors, conn, tankid, userid, newTankConfigID);
                //    }
                //}
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMsg = ex.Message;
        //        fileName = errorfilepath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
        //        //FileWriter errorWriter = new FileWriter(fileName);
        //        //errorWriter.Write("****************************");
        //        //errorWriter.Write(DateTime.Now.ToString());
        //        //errorWriter.Write("Error at TankConfig.Add - ");
        //        //errorWriter.Write(ex.Message);
        //        //errorWriter.Close();
        //    }
        //    return tankupdateerror;
        //}

        public static /*TankData.DB.TankConfigRow*/ void  GetCurrentTankConfig(string conn, int tankconfigid)
        {
            //TankData.DB.TankConfigRow currentTankConfigDR = null;
            //TankData.DB.TankConfigDataTable tankConfigDT = null;
            //TankData.DBTableAdapters.TankConfigTableAdapter tankConfigTA = null;
            try
            {
                //using (tankConfigTA = new TankData.DBTableAdapters.TankConfigTableAdapter())
                //{
                //    tankConfigTA.Connection.ConnectionString = conn; ;
                //    using (tankConfigDT = new TankData.DB.TankConfigDataTable())
                //    {
                //        tankConfigDT.Clear();
                //        tankConfigTA.FillByID(tankConfigDT, tankconfigid);
                //        foreach (TankData.DB.TankConfigRow dr in tankConfigDT.Rows)
                //        {
                //            currentTankConfigDR = dr;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
          //  return currentTankConfigDR;
        }

        public static bool UpdateTank(bool goterror, string conn, int tankid, int userid, int newtankconfigid)
        {
          //  TankData.DBTableAdapters.TankTableAdapter tankTA = null;
          //  TankData.DB.TankDataTable tankDT = null;
            bool successfulupdate = false;
            try
            {
                if (!goterror)
                {
                    //using (tankTA = new TankData.DBTableAdapters.TankTableAdapter())
                    //{
                    //    tankTA.Connection.ConnectionString = conn;
                    //    using (tankDT = new TankData.DB.TankDataTable())
                    //    {
                    //        tankDT.Clear();
                    //        tankTA.FillByID(tankDT, tankid);
                    //        tankDT[0].TankConfigID = newtankconfigid;
                    //        tankDT[0].ModifiedBy = userid;
                    //        tankDT[0].ModifiedOn = DateTime.Now.ToUniversalTime();
                    //        tankTA.Update(tankDT);
                    //    }
                    //}
                    successfulupdate = true;
                }
                else
                    successfulupdate = false;
            }
            catch (Exception ex)
            {
                successfulupdate = false;
                string errorMsg = ex.Message;
            }
            return successfulupdate;
        }

    }
}
