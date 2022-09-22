using PortalWebApp.Areas.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace PortalWebApp.Utilities
{
    public class Util
    {
        public static float psiPerCubicInch = 27.729623F;
        public static int TankIDOrdinal { get; set; }
        public static int TankNameOrdinal { get; set; }
        public static int RtuNumberOrdinal { get; set; }
        public static int TankHgtOrdinal  { get; set; }
        public static int TankCapOrdinal  { get; set; }
        public static int CapacityLimitOrdinal  { get; set; }
        public static int TankMinimumOrdinal  { get; set; }
        public static int ReorderUsageOrdinal  { get; set; }
        public static int SafetyStockUsageOrdinal  { get; set; }
        public static int LowLowLevelOrdinal { get; set; }
        public static int LowLevelOrdinal  { get; set; }
        public static int HighLevelOrdinal  { get; set; }
        public static int HighHighLevelOrdinal { get; set; }
        public static int FillDetectDeltaOrdinal { get; set; }
        public static int ShortFillDeltaOrdinal { get; set; }
        public static int VolumeDeltaOrdinal { get; set; }
        public static int RateChangeDeltaOrdinal { get; set; }
        public static int CallsperdayOrdinal { get; set; }
        public static int CallDayOrdinal { get; set; }
        public static int IntervalOrdinal { get; set; }
        public static int DiagCallDayMaskOrdinal { get; set; }
        public static int DataLogDeltaOrdinal { get; set; }
        public static int UsageDeltaOrdinal { get; set; }
        public static int WakeIntervalOrdinal { get; set; }
        public static int StartTimeOrdinal { get; set; }
        public static int HighSetPointOrdinal { get; set; }
        public static int LowSetPointOrdinal { get; set; }
        public static int SensorOffsetOrdinal { get; set; }
        public static int CoeffExpOrdinal { get; set; }
        public static int SpecGravOrdinal { get; set; }
        public static int DeviceFillDetectDeltaOrdinal { get; set; }
        public static int DeviceFillHysteresisOrdinal { get; set; }
        public static int DeviceCriticalLowLevelOrdinal { get; set; }
        public static int DeviceLowLevelOrdinal { get; set; }
        public static int DeviceHighLevelOrdinal { get; set; }
        public static int DeviceCriticalHighLevelOrdinal { get; set; }
        public static int DeviceFillDetectOrdinal { get; set; }
        public static int DeviceUsageAlarmOrdinal { get; set; }
        public static int HasExpectedCallAlarmOrdinal { get; set; }
        public static int TankNormallyFillsOrdinal { get; set; }
        public static int EnableGPSOrdinal { get; set; }
        public static int EnableLocationOrdinal { get; set; }

        public sealed class Env
        {
            public static readonly Env Dev = new Env("Server=TankdataLSN1\\TankData;Database=TankData_TDG;User ID=EmailManager;pwd=tanklink5410");
            public static readonly Env Prod = new Env("Server=PRODUCTION");

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
                _ = ex.Message;
                return true;
            }
            return multipleInstances;
        }



        public static SqlParameter[] GetSqlParams(DataRow dr)
        {
            var prm= new SqlParameter[]
            {
                new SqlParameter("@TankID", SqlDbType.Int)                      { Value = dr[TankIDOrdinal] },
                new SqlParameter("@RTUNumber", SqlDbType.NVarChar)              { Value = dr[RtuNumberOrdinal] },
                new SqlParameter("@TankHgt", SqlDbType.Decimal)                 { Value = (dr[TankHgtOrdinal] == DBNull.Value) ? 0 : dr[TankHgtOrdinal] },
                new SqlParameter("@TankCap", SqlDbType.Decimal)                 { Value = (dr[TankCapOrdinal] == DBNull.Value) ? 0 : dr[TankCapOrdinal] },
                new SqlParameter("@CapacityLimit", SqlDbType.Decimal)           { Value = (dr[CapacityLimitOrdinal] == DBNull.Value) ? 0 : dr[CapacityLimitOrdinal] },
                new SqlParameter("@TankMinimum", SqlDbType.Decimal)             { Value = (dr[TankMinimumOrdinal] == DBNull.Value) ? 0 : dr[TankMinimumOrdinal] },

                new SqlParameter("@ReorderUsage", SqlDbType.Int)                { Value = (dr[ReorderUsageOrdinal] == DBNull.Value) ? 0 : dr[ReorderUsageOrdinal] },
                new SqlParameter("@SafetyStockUsage", SqlDbType.Int)            { Value = (dr[SafetyStockUsageOrdinal] == DBNull.Value) ? 0 : dr[SafetyStockUsageOrdinal] },
                new SqlParameter("@Callsperday", SqlDbType.Int)                 { Value = (dr[CallsperdayOrdinal] == DBNull.Value) ? 0 : dr[CallsperdayOrdinal] },
                new SqlParameter("@CallDay", SqlDbType.Int)                     { Value = (dr[CallDayOrdinal] == DBNull.Value) ? 0 : dr[CallDayOrdinal] },

                new SqlParameter("@StartTime", SqlDbType.DateTime)              { Value = (dr[StartTimeOrdinal] == DBNull.Value) ? 0 : dr[StartTimeOrdinal] },
                new SqlParameter("@Interval", SqlDbType.NVarChar)               { Value = (dr[IntervalOrdinal] == DBNull.Value) ? 0 : dr[IntervalOrdinal] },
                new SqlParameter("@DiagCallDayMask", SqlDbType.Int)             { Value = (dr[DiagCallDayMaskOrdinal] == DBNull.Value) ? 0 : dr[DiagCallDayMaskOrdinal] },
                new SqlParameter("@HighSetPoint", SqlDbType.Decimal)            { Value = (dr[HighSetPointOrdinal] == DBNull.Value) ? 0 : dr[HighSetPointOrdinal] },
                new SqlParameter("@LowSetPoint", SqlDbType.Decimal)             { Value = (dr[LowSetPointOrdinal] == DBNull.Value) ? 0 : dr[LowSetPointOrdinal] },

                new SqlParameter("@SensorOffset", SqlDbType.Decimal)            { Value = (dr[SensorOffsetOrdinal] == DBNull.Value) ? 0 : dr[SensorOffsetOrdinal] },
                new SqlParameter("@CoeffExp", SqlDbType.Decimal)                { Value = (dr[CoeffExpOrdinal] == DBNull.Value) ? 0 : dr[CoeffExpOrdinal] },
                new SqlParameter("@SpecGrav", SqlDbType.Decimal)                { Value = (dr[SpecGravOrdinal] == DBNull.Value) ? 0 : dr[SpecGravOrdinal] },
                new SqlParameter("@LowLowLevel", SqlDbType.Int)                 { Value = (dr[LowLowLevelOrdinal] == DBNull.Value) ? 0 : dr[LowLowLevelOrdinal] },
                new SqlParameter("@LowLevel", SqlDbType.Int)                    { Value = (dr[LowLevelOrdinal] == DBNull.Value) ? 0 : dr[LowLevelOrdinal] },

                new SqlParameter("@HighLevel", SqlDbType.Int)                       { Value = (dr[HighLevelOrdinal] == DBNull.Value) ? 0 : dr[HighLevelOrdinal] },
                new SqlParameter("@HighHighLevel", SqlDbType.Int)               { Value = (dr[HighHighLevelOrdinal] == DBNull.Value) ? 0 : dr[HighHighLevelOrdinal] },
                new SqlParameter("@ShortFillDelta", SqlDbType.Decimal)          { Value = (dr[ShortFillDeltaOrdinal] == DBNull.Value) ? 0 : dr[ShortFillDeltaOrdinal] },
                new SqlParameter("@FillDetectDelta", SqlDbType.Decimal)         { Value = (dr[FillDetectDeltaOrdinal] == DBNull.Value) ? 0 : dr[FillDetectDeltaOrdinal] },
                new SqlParameter("@VolumeDelta", SqlDbType.Int)                 { Value = (dr[VolumeDeltaOrdinal] == DBNull.Value) ? 0 : dr[VolumeDeltaOrdinal] },

                new SqlParameter("@RateChangeDelta", SqlDbType.Int)                 { Value = (dr[RateChangeDeltaOrdinal] == DBNull.Value) ? 0 : dr[RateChangeDeltaOrdinal] },
                new SqlParameter("@DeviceCriticalLowLevel", SqlDbType.Bit)      { Value = (dr[DeviceCriticalLowLevelOrdinal] == DBNull.Value) ? 0 : dr[DeviceCriticalLowLevelOrdinal] },
                new SqlParameter("@DeviceLowLevel", SqlDbType.Bit)              { Value = (dr[DeviceLowLevelOrdinal] == DBNull.Value) ? 0 : dr[DeviceLowLevelOrdinal] },
                new SqlParameter("@DeviceHighLevel", SqlDbType.Bit)             { Value = (dr[DeviceHighLevelOrdinal] == DBNull.Value) ? 0 : dr[DeviceHighLevelOrdinal] },
                new SqlParameter("@DeviceCriticalHighLevel", SqlDbType.Bit)     { Value = (dr[DeviceCriticalHighLevelOrdinal] == DBNull.Value) ? 0 : dr[DeviceCriticalHighLevelOrdinal] },
                
                new SqlParameter("@DeviceFillDetect", SqlDbType.Bit)                { Value = (dr[DeviceFillDetectOrdinal] == DBNull.Value) ? 0 : dr[DeviceFillDetectOrdinal] },
                new SqlParameter("@DeviceFillDetectDelta", SqlDbType.Decimal)   { Value = (dr[DeviceFillDetectDeltaOrdinal] == DBNull.Value) ? 0 : dr[DeviceFillDetectDeltaOrdinal] },
                new SqlParameter("@DeviceFillHysteresis", SqlDbType.Bit)        { Value = (dr[DeviceFillHysteresisOrdinal] == DBNull.Value) ? 0 : dr[DeviceFillHysteresisOrdinal] },
                new SqlParameter("@DataLogDelta", SqlDbType.Int)                { Value = (dr[DataLogDeltaOrdinal] == DBNull.Value) ? 0 : dr[DataLogDeltaOrdinal] },
                new SqlParameter("@UsageDelta", SqlDbType.Bit)                  { Value = (dr[UsageDeltaOrdinal] == DBNull.Value) ? 0 : dr[UsageDeltaOrdinal] },
                new SqlParameter("@WakeInterval", SqlDbType.Int)                { Value = (dr[WakeIntervalOrdinal] == DBNull.Value) ? 0 : dr[WakeIntervalOrdinal] },
               // new SqlParameter("@DeviceUsageAlarm ", SqlDbType.Bit),        { Value = (dr[tankHgtOrdinal] == DBNull.Value) ? 0 : dr[tankHgtOrdinal] },
                new SqlParameter("@HasExpectedCallAlarm", SqlDbType.Bit)        { Value = (dr[HasExpectedCallAlarmOrdinal] == DBNull.Value) ? 0 : dr[HasExpectedCallAlarmOrdinal] },
                new SqlParameter("@TankNormallyFills", SqlDbType.Bit)           { Value = (dr[TankNormallyFillsOrdinal] == DBNull.Value) ? 0 : dr[TankHgtOrdinal] },
                new SqlParameter("@EnableGPS", SqlDbType.Bit)                   { Value = (dr[EnableGPSOrdinal] == DBNull.Value) ? 0 : dr[EnableGPSOrdinal] },
                new SqlParameter("@EnableLocation", SqlDbType.Bit)              { Value = (dr[EnableLocationOrdinal] == DBNull.Value) ? 0 : dr[EnableLocationOrdinal] }
        };
            return prm;
        }
  

        public static void GetColumnOrdinals(DataTable dt)
        {
             TankIDOrdinal = dt.Columns["TankID"].Ordinal;
             TankNameOrdinal = dt.Columns["TankName"].Ordinal;
             RtuNumberOrdinal = dt.Columns["RTUNumber"].Ordinal;
             TankHgtOrdinal = dt.Columns["TankHgt"].Ordinal;
             TankCapOrdinal = dt.Columns["TankCap"].Ordinal;
             CapacityLimitOrdinal = dt.Columns["CapacityLimit"].Ordinal;
             TankMinimumOrdinal = dt.Columns["TankMinimum"].Ordinal;
             ReorderUsageOrdinal = dt.Columns["ReOrderUsage"].Ordinal;
             SafetyStockUsageOrdinal = dt.Columns["SafetyStockUsage"].Ordinal;
             LowLowLevelOrdinal = dt.Columns["LowLowLevel"].Ordinal;
             LowLevelOrdinal = dt.Columns["LowLevel"].Ordinal;
             HighLevelOrdinal = dt.Columns["HighLevel"].Ordinal;
             HighHighLevelOrdinal = dt.Columns["HighHighLevel"].Ordinal;
             FillDetectDeltaOrdinal = dt.Columns["FillDetectDelta"].Ordinal;
             ShortFillDeltaOrdinal = dt.Columns["ShortFillDelta"].Ordinal;
             VolumeDeltaOrdinal = dt.Columns["VolumeDelta"].Ordinal;
             RateChangeDeltaOrdinal = dt.Columns["RateChangeDelta"].Ordinal;
             CallsperdayOrdinal = dt.Columns["Callsperday"].Ordinal;
             CallDayOrdinal = dt.Columns["CallDay"].Ordinal;
             IntervalOrdinal = dt.Columns["Interval"].Ordinal;
             DiagCallDayMaskOrdinal = dt.Columns["DiagCallDayMask"].Ordinal;
             DataLogDeltaOrdinal = dt.Columns["DataLogDelta"].Ordinal;
             UsageDeltaOrdinal = dt.Columns["UsageDelta"].Ordinal;
             WakeIntervalOrdinal = dt.Columns["WakeInterval"].Ordinal;
             StartTimeOrdinal = dt.Columns["StartTime"].Ordinal;
             HighSetPointOrdinal = dt.Columns["HighSetPoint"].Ordinal;
             LowSetPointOrdinal = dt.Columns["LowSetPoint"].Ordinal;
             SensorOffsetOrdinal = dt.Columns["SensorOffset"].Ordinal;
             CoeffExpOrdinal = dt.Columns["CoeffExp"].Ordinal;
             SpecGravOrdinal = dt.Columns["SpecGrav"].Ordinal;
             DeviceFillDetectDeltaOrdinal = dt.Columns["DeviceFillDetectDelta"].Ordinal;
             DeviceFillHysteresisOrdinal = dt.Columns["DeviceFillHysteresis"].Ordinal;
             DeviceCriticalLowLevelOrdinal = dt.Columns["DeviceCriticalLowLevel"].Ordinal;
             DeviceLowLevelOrdinal = dt.Columns["DeviceLowLevel"].Ordinal;
             DeviceHighLevelOrdinal = dt.Columns["DeviceHighLevel"].Ordinal;
             DeviceCriticalHighLevelOrdinal = dt.Columns["DeviceCriticalHighLevel"].Ordinal;
             DeviceFillDetectOrdinal = dt.Columns["DeviceFillDetect"].Ordinal;
             DeviceUsageAlarmOrdinal = dt.Columns["DeviceUsageAlarm"].Ordinal;
             HasExpectedCallAlarmOrdinal = dt.Columns["HasExpectedCallAlarm"].Ordinal;
             TankNormallyFillsOrdinal = dt.Columns["TankNormallyFills"].Ordinal;
            //new code added to accomodate EnableLocation and EnableGPS+   D Arcilla    Oct 2020
             EnableLocationOrdinal = dt.Columns["EnableLocation"].Ordinal;
             EnableGPSOrdinal = dt.Columns["EnableGPS"].Ordinal;
        }
       

        public static int MinutesToMilliseconds(int minutes)
        {
            int milliseconds;
            try
            {
                milliseconds = minutes * 60000;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
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
                _ = ex.Message;
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
                _ = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToInt(string mystring)
        {
            bool isValid;
            try
            {
                int testInteger = int.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToDecimal(string mystring)
        {
            bool isValid;
            try
            {
                decimal.TryParse(mystring, out decimal testDecimal);
                isValid = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                isValid = false;
            }
            return isValid;
        }


        public static bool ConvertStringToBool(string mystring)
        {
            bool isValid;
            try
            {
                bool testBool = bool.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToFloat(string mystring)
        {
            bool isValid;
            try
            {
                float testFloat = float.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToBigInt(string mystring)
        {
            bool isValid;
            try
            {
                long testInteger = Int64.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                isValid = false;
            }
            return isValid;
        }

        public static bool ConvertStringToDateTime(string mystring)
        {
            _ = new DateTime();
            bool isValid;
            try
            {
                DateTime testDateTime = DateTime.Parse(mystring);
                isValid = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
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
                _ = ex.Message;
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
                _ = ex.Message;
            }
        }

        //modified to include EnableGPS and EnableLocation

        public static bool UpdateTankConfig(string conn, int tankid, int tankconfigid, int userid, string tankname, string tankhgt, string tankcap,
                                            string capacitylimit, string limitcapacityflag, string tankminimum, string reorderusage,
                                            string safetystockusage, string starttime, string callsperday, string callday, string interval,
                                            string diagcalldaymask, string highsetpoint, string lowsetpoint, string sensoroffset,
                                            string coeffexp, string specgrav, string lowlowlevel, string lowlevel, string highlevel,
                                            string highhighlevel, string filldetectdelta, string shortfilldelta, string volumedelta,
                                            string ratechangedelta, string devicecriticallowlevel, string devicelowlevel,
                                            string devicehighlevel, string devicecriticalhighlevel, string devicefilldetect,
                                            string devicefilldetectdelta, string devicefillhysteresis, string datalogdelta,
                                            string usagedelta, string wakeinterval, string deviceusagealarm, string hasexpectedcallalarm,
                                            string tanknormallyfills, string enablegps, string enablelocation)

        {
            bool tankupdateerror = false;
            string errorfilepath = AppDomain.CurrentDomain.BaseDirectory + "ErrorFile";
            try
            {
                TankData.DB.TankConfigRow currentTankConfigDR = GetCurrentTankConfig(conn, tankconfigid);
                TankData.DBTableAdapters.TankConfigTableAdapter tankConfigTA;
                using (tankConfigTA = new TankData.DBTableAdapters.TankConfigTableAdapter())
                {
                    tankConfigTA.Connection.ConnectionString = conn;
                    TankData.DB.TankConfigDataTable tankConfigDT;
                    using (tankConfigDT = new TankData.DB.TankConfigDataTable())
                    {
                        tankConfigDT.Clear();
                        TankData.DB.TankConfigRow newTankConfigDR = tankConfigDT.NewTankConfigRow();
                        newTankConfigDR.Copy(currentTankConfigDR, userid);
                        newTankConfigDR.BeginEdit();
                        if (tankname != "*** Empty ***")
                            newTankConfigDR.TankName = tankname;
                        if (tankhgt != "*** Empty ***")
                            newTankConfigDR.TankHgt = decimal.Parse(tankhgt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (tankcap != "*** Empty ***")
                            newTankConfigDR.TankCap = decimal.Parse(tankcap, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (capacitylimit != "*** Empty ***")
                            newTankConfigDR.CapacityLimit = decimal.Parse(capacitylimit);
                        if (!string.IsNullOrEmpty(limitcapacityflag))
                            newTankConfigDR.LimitCapacityFlag = bool.Parse(limitcapacityflag);
                        if (tankminimum != "*** Empty ***")
                            newTankConfigDR.TankMinimum = decimal.Parse(tankminimum, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (reorderusage != "*** Empty ***")
                            newTankConfigDR.ReorderUsage = int.Parse(reorderusage);
                        if (safetystockusage != "*** Empty ***")
                            newTankConfigDR.SafetyStockUsage = int.Parse(safetystockusage);
                        if (starttime != "*** Empty ***")
                            newTankConfigDR.StartTime = DateTime.Parse(starttime);
                        if (callsperday != "*** Empty ***")
                            newTankConfigDR.Callsperday = int.Parse(callsperday);
                        if (callday != "*** Empty ***")
                            newTankConfigDR.CallDay = int.Parse(callday);
                        if (interval != "*** Empty ***")
                            newTankConfigDR.Interval = int.Parse(interval);
                        if (diagcalldaymask != "*** Empty ***")
                            newTankConfigDR.DiagCallDayMask = int.Parse(diagcalldaymask);
                        if (highsetpoint != "*** Empty ***")
                            newTankConfigDR.HighSetPoint = decimal.Parse(highsetpoint, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (lowsetpoint != "*** Empty ***")
                            newTankConfigDR.LowSetPoint = decimal.Parse(lowsetpoint, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (sensoroffset != "*** Empty ***")
                            newTankConfigDR.SensorOffset = decimal.Parse(sensoroffset, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (coeffexp != "*** Empty ***")
                            newTankConfigDR.CoeffExp = decimal.Parse(coeffexp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (specgrav != "*** Empty ***")
                            newTankConfigDR.SpecGrav = decimal.Parse(specgrav, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (lowlowlevel != "*** Empty ***")
                            newTankConfigDR.LowLowLevel = int.Parse(lowlowlevel);
                        if (lowlevel != "*** Empty ***")
                            newTankConfigDR.LowLevel = int.Parse(lowlevel);
                        if (highlevel != "*** Empty ***")
                            newTankConfigDR.HighLevel = int.Parse(highlevel);
                        if (highhighlevel != "*** Empty ***")
                            newTankConfigDR.HighHighLevel = int.Parse(highhighlevel);
                        if (filldetectdelta != "*** Empty ***")
                            newTankConfigDR.FillDetectDelta = decimal.Parse(filldetectdelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (shortfilldelta != "*** Empty ***")
                            newTankConfigDR.ShortFillDelta = decimal.Parse(shortfilldelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (volumedelta != "*** Empty ***")
                            newTankConfigDR.VolumeDelta = int.Parse(volumedelta);
                        if (ratechangedelta != "*** Empty ***")
                            newTankConfigDR.RateChangeDelta = int.Parse(ratechangedelta);
                        if (devicecriticallowlevel != "*** Empty ***")
                            newTankConfigDR.DeviceCriticalLowLevel = bool.Parse(devicecriticallowlevel);
                        if (devicelowlevel != "*** Empty ***")
                            newTankConfigDR.DeviceLowLevel = bool.Parse(devicelowlevel);
                        if (devicehighlevel != "*** Empty ***")
                            newTankConfigDR.DeviceHighLevel = bool.Parse(devicehighlevel);
                        if (devicecriticalhighlevel != "*** Empty ***")
                            newTankConfigDR.DeviceCriticalHighLevel = bool.Parse(devicecriticalhighlevel);
                        if (devicefilldetect != "*** Empty ***")
                            newTankConfigDR.DeviceFillDetect = bool.Parse(devicefilldetect);
                        if (devicefilldetectdelta != "*** Empty ***")
                            newTankConfigDR.DeviceFillDetectDelta = decimal.Parse(devicefilldetectdelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (devicefillhysteresis != "*** Empty ***")
                            newTankConfigDR.DeviceFillHysteresis = decimal.Parse(devicefillhysteresis, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        if (datalogdelta != "*** Empty ***")
                            newTankConfigDR.DataLogDelta = int.Parse(datalogdelta);
                        if (usagedelta != "*** Empty ***")
                            newTankConfigDR.UsageDelta = int.Parse(usagedelta);
                        if (wakeinterval != "*** Empty ***")
                            newTankConfigDR.WakeInterval = int.Parse(wakeinterval);
                        if (deviceusagealarm != "*** Empty ***")
                            newTankConfigDR.DeviceUsageAlarm = bool.Parse(deviceusagealarm);
                        if (hasexpectedcallalarm != "*** Empty ***")
                            newTankConfigDR.HasExpectedCallAlarm = bool.Parse(hasexpectedcallalarm);
                        if (tanknormallyfills != "*** Empty ***")
                            newTankConfigDR.TankNormallyFills = bool.Parse(tanknormallyfills);
                        if (enablegps != "*** Empty ***")
                            newTankConfigDR.EnableGPS = bool.Parse(enablegps);
                        if (enablelocation != "*** Empty ***")
                            newTankConfigDR.EnableLocation = bool.Parse(enablelocation);
                        newTankConfigDR.EndEdit();
                        tankConfigDT.Rows.Add(newTankConfigDR);
                        tankConfigTA.Update(tankConfigDT);
                        int newTankConfigID = newTankConfigDR.TankConfigId;
                        bool noerrors = false;
                        tankupdateerror = UpdateTank(noerrors, conn, tankid, userid, newTankConfigID);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                string fileName = errorfilepath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankConfig.Add - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
            return tankupdateerror;
        }

        public static TankData.DB.TankConfigRow  GetCurrentTankConfig(string conn, int tankconfigid)
        {
            TankData.DB.TankConfigRow currentTankConfigDR = null;
            try
            {
                TankData.DBTableAdapters.TankConfigTableAdapter tankConfigTA;
                using (tankConfigTA = new TankData.DBTableAdapters.TankConfigTableAdapter())
                {
                    tankConfigTA.Connection.ConnectionString = conn; ;
                    TankData.DB.TankConfigDataTable tankConfigDT;
                    using (tankConfigDT = new TankData.DB.TankConfigDataTable())
                    {
                        tankConfigDT.Clear();
                        tankConfigTA.FillByID(tankConfigDT, tankconfigid);
                        foreach (TankData.DB.TankConfigRow dr in tankConfigDT.Rows)
                        {
                            currentTankConfigDR = dr;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return currentTankConfigDR;
        }

        public static bool UpdateTank(bool goterror, string conn, int tankid, int userid, int newtankconfigid)
        {
            bool successfulupdate;
            try
            {
                if (!goterror)
                {
                    TankData.DBTableAdapters.TankTableAdapter tankTA;
                    using (tankTA = new TankData.DBTableAdapters.TankTableAdapter())
                    {
                        tankTA.Connection.ConnectionString = conn;
                        TankData.DB.TankDataTable tankDT;
                        using (tankDT = new TankData.DB.TankDataTable())
                        {
                            tankDT.Clear();
                            tankTA.FillByID(tankDT, tankid);
                            tankDT[0].TankConfigID = newtankconfigid;
                            tankDT[0].ModifiedBy = userid;
                            tankDT[0].ModifiedOn = DateTime.Now.ToUniversalTime();
                            tankTA.Update(tankDT);
                        }
                    }
                    successfulupdate = true;
                }
                else
                    successfulupdate = false;
            }
            catch (Exception ex)
            {
                successfulupdate = false;
                _ = ex.Message;
            }
            return successfulupdate;
        }

        public static int ValidateUser(string connectionstring, string username)
        {
            int userID = -1;
            try
            {
                using (var con = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand("select userid from [user] where superuser=1 and username = @username", con))
                    {
                        con.Open();
                        cmd.Parameters.Add(new SqlParameter("username", username));
                        userID = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                userID = -1;
            }
           
            return userID;
        }

        public static string GetPassword(string connectionstring, int userid)
        {
            string encryptedPassword = string.Empty;
            string passwordSalt = string.Empty;
            string userPassword = string.Empty;
            try
            {
                using (var con = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand("select password, passwordsalt from[user] u where userid = @userid", con))
                    {
                        con.Open();
                        cmd.Parameters.Add(new SqlParameter("userid", userid));
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            int saltOrdinal = reader.GetOrdinal("passwordsalt");
                            int passwordOrdinal = reader.GetOrdinal("password");
                            while (reader.Read())
                            {
                                encryptedPassword = reader.GetString(passwordOrdinal);
                                passwordSalt = reader.GetString(saltOrdinal);
                            }
                            reader.Close();
                            Adage.Functions.Encryption.Decryptor dec = new Adage.Functions.Encryption.Decryptor(Adage.Functions.Encryption.EncryptionAlogrithm.Rijndael)
                            {
                                IV = System.Convert.FromBase64String(passwordSalt)
                            };
                            byte[] tmpPass = dec.Decrypt(System.Convert.FromBase64String(encryptedPassword), Constants.IV());
                            userPassword = System.Text.Encoding.ASCII.GetString(tmpPass);
                        }
                        else
                            userPassword = string.Empty;
                    }
                }
               
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                userPassword = string.Empty;
            }
            return userPassword;
        }

    }

    public class Constants
    {
        //This is the Initialization vector we use for SupplyNetTankLink - Do not change this.
        public static byte[] IV()
        {
            byte[] InitializationVector = { 121, 241, 71, 121, 44, 234, 242, 1, 34, 72, 50, 43, 76, 123, 20, 25 };
            return InitializationVector;
        }
    }

}

