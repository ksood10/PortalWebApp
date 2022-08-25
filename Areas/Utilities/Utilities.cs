using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Linq;
using System.Threading;




namespace PortalWebApp.Utilities { 
    class Utilities
    {
        public static float psiPerCubicInch = 27.729623F;

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
          //  bool noerrors = false;
            bool tankupdateerror = false;
            string fileName = string.Empty;
            string errorfilepath = AppDomain.CurrentDomain.BaseDirectory + "ErrorFile";
            //TankData.DBTableAdapters.TankConfigTableAdapter tankConfigTA = null;
            //TankData.DB.TankConfigDataTable tankConfigDT = null;
            //TankData.DB.TankConfigRow newTankConfigDR = null;
           // int newTankConfigID = 0;
            try
            {
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
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                fileName = errorfilepath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                //FileWriter errorWriter = new FileWriter(fileName);
                //errorWriter.Write("****************************");
                //errorWriter.Write(DateTime.Now.ToString());
                //errorWriter.Write("Error at TankConfig.Add - ");
                //errorWriter.Write(ex.Message);
                //errorWriter.Close();
            }
            return tankupdateerror;
        }

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
