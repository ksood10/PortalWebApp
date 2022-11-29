using PortalWebApp.Areas.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PortalWebApp.Utilities;
using System.Data.SqlClient;
using System.Text;

namespace PortalWebApp.Models
{
    [Table("Tank")]
    public class Tank
    {

        private readonly StringBuilder sb = new StringBuilder();
        [Key]
        public string TankID                           { get; set; }
        public string RTUNumber                     { get; set; }
        public string StartTime                   { get; set; }
        public string Interval                      { get; set; }
        public int TankConfigId                     { get; set; }
        public string TankName                      { get; set; }
        public string UserTankNumber                   { get; set; }
        public int TransportStatusID                { get; set; }
        public int ServicePlanID                    { get; set; }
        public string UserDefined1                     { get; set; }
        public string  UserDefined2                     { get; set; }
        public string UserDefined3                     { get; set; }
        public string UserDefined4                     { get; set; }
        public int RatePlanID                       { get; set; }
        public int InstallationTypeID               { get; set; }
        public DateTime InstallationStatus               { get; set; }
        public DateTime InstallDate                      { get; set; }
        public DateTime ServiceDate                      { get; set; }
        public string Region                           { get; set; }
        public string Route                            { get; set; }
        public int ChartID                          { get; set; }
        public int UnitOfMeasureID                  { get; set; }
        public string TankHgt                          { get; set; }
        public string TankCap                          { get; set; }
        public string LimitCapacityFlag                { get; set; }
        public string CapacityLimit                    { get; set; }
        public string TankNormallyFills                { get; set; }
        public string ProdDesc                         { get; set; }
        public string UserProductNumber                { get; set; }
        public string SpecGrav                         { get; set; }
        public string CoeffExp                         { get; set; }
        public string SensorOffset                     { get; set; }
        public string LowSetPoint                      { get; set; }
        public string HighSetPoint                     { get; set; }
        public decimal TempOffset                       { get; set; }
        public decimal PulseValue                       { get; set; }
        public string HighHighLevel                    { get; set; }
        public string HighLevel                        { get; set; }
        public string LowLevel                         { get; set; }
        public string LowLowLevel                      { get; set; }
        public string FillDetectDelta                  { get; set; }
        public string ShortFillDelta                   { get; set; }
        public int HighTemp                         { get; set; }
        public int LowTemp                          { get; set; }
        public int TankSensorTypeID                 { get; set; }
        public string TankSensorLength                 { get; set; }
        public string TankSensorDesc                   { get; set; }
        public string TankSensorNumber                 { get; set; }
        public string Callsperday                      { get; set; }
        public string CallDay                          { get; set; }
        public string DiagCallDayMask                  { get; set; }
        public string UsageDelta                       { get; set; }
        public string WakeInterval                     { get; set; }
        public string TankNum                          { get; set; }
        public bool UpdateInventory                  { get; set; }
        public string DeviceUsageAlarm                 { get; set; }
        public string DeviceCriticalHighLevel          { get; set; }
        public string DeviceHighLevel                  { get; set; }
        public string DeviceLowLevel                   { get; set; }
        public string DeviceCriticalLowLevel           { get; set; }
        public string DeviceFillDetect                 { get; set; }
        public bool DeviceHighTemp                   { get; set; }
        public bool DeviceLowTemp                    { get; set; }
        public string HasExpectedCallAlarm             { get; set; }
        public int ExpectedCallInterval             { get; set; }
        public bool Active                           { get; set; }
        public int CreatedBy                        { get; set; }
        public DateTime CreatedOn                        { get; set; }
        public int ModifiedBy                       { get; set; }
        public DateTime ModifiedOn                       { get; set; }
        public DateTime Stamp                            { get; set; }
        public int GroupId                          { get; set; }
        public bool DeviceSuspiciousFilter           { get; set; }
        public int DeviceSuspiciousDelta            { get; set; }
        public string DeviceFillDetectDelta            { get; set; }
        public string DeviceFillHysteresis             { get; set; }
        public string DataLogDelta                     { get; set; }
        public int EnableDeliveryReport             { get; set; }
        public string VolumeDelta                      { get; set; }
        public decimal VaporSensorRange                 { get; set; }
        public decimal ProductSensorRange               { get; set; }
        public decimal ForecastDailyUsage               { get; set; }
        public string TankMinimum                      { get; set; }
        public string ReorderUsage                     { get; set; }
        public string SafetyStockUsage                 { get; set; }
        public int DistributionLocationID           { get; set; }
        public string EnableLocation                   { get; set; }
        public string EnableGPS                        { get; set; }
        public string RateChangeDelta                  { get; set; }
        public int ProductID                        { get; set; }

        public string ConnectionString { get; set; }
        public int UserID { get; set; }
        public string ErrorFilePath { get; set; }
        public bool CheckRTUCondition { get; set; }
        public bool HaveError { get; set; }
        public string StatusMessage { get; set; }
        public string BadColumn { get;  set; }
        public string BadColumnValue { get; set; }
        public bool PerformUpdate { get;  set; }
        public bool DeviceHasModem { get;  set; }
        public bool DeviceHasGPS { get;  set; }
        public decimal CurrentShortFillDelta { get;  set; }
        public decimal CurrentFillDetectDelta { get;  set; }
        public decimal CurrentCapacityLimit { get;  set; }
        public decimal CurrentDeviceFillDetect { get;  set; }
        public decimal CurrentTankCap { get;  set; }
        public string CurrentModelNumber { get;  set; }
        public int CurrentTankConfigID { get;  set; }
       

        private string fileName;
        private int CurrentInterval;
        public int OrganizationID { get; set; }

        internal int CurrentTankDeviceID { get; set; }

        public decimal CurrentTankHgt { get;  set; }
        public int CurrentLowLevel { get;  set; }
        public int CurrentLowLowLevel { get;  set; }
        public int CurrentHighHighLevel { get;  set; }
        public int CurrentVolumeDelta { get; set; }
        public int CurrentRateChangeDelta { get; private set; }
        public decimal CurrentSensorOffset { get; private set; }
        public decimal CurrentDeviceFillHysteresis { get;  set; }
        public int CurrentHighLevel { get;  set; }
        public int CurrentCallsperday { get;  set; }

        public Tank()
        {
            //TankID = 0;
        }
        public Tank(string conn, int userid)
        {
            this.ConnectionString = conn;
            this.UserID = userid;
        }

        internal void GetCurrentDeviceID()
        {
            try
            {
                using (var con = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select deviceid from tank where tankid = @tankid", con))
                    {
                        con.Open();
                        cmd.Parameters.Add(new SqlParameter("tankid", this.TankID));
                        this.CurrentTankDeviceID = (int)cmd.ExecuteScalar();

                        //valid = bool.Parse(DAL.ReturnScalar(sb.ToString(), this.ConnectionString, paramArray, true, 20).ToString());
                    }
                }
                          }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at GetCurrentDeviceID - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal int GetCurrentTankConfigInfo()
        {
            int organizationid = 0;
            try
            {
                sb.Length = 0;
                sb.Append("select t.organizationid, t.tankconfigid, CapacityLimit, TankCap, ");
                sb.Append("LowLowLevel, LowLevel, HighLevel, FillDetectDelta, ShortFillDelta, ");
                sb.Append("HighHighLevel, VolumeDelta, RateChangeDelta, SensorOffset, DeviceFillHysteresis, ");
                sb.Append("Callsperday, Interval, ModelNumber, HasGPS, HasModem  ");
                sb.Append("from tank t ");
                sb.Append("inner join tankconfig tc on t.tankconfigid = tc.tankconfigid ");
                sb.Append("inner join device d on t.deviceid = d.deviceid ");
                sb.Append("inner join devicetype dt on d.devicetypeid = dt.devicetypeid ");
                sb.Append("where t.tankid = @tankid");

                using (var con = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb.ToString(), con))
                    {
                        con.Open();
                        cmd.Parameters.Add(new SqlParameter("tankid", this.TankID));
                        SqlDataReader dr = cmd.ExecuteReader();

                        //valid = bool.Parse(DAL.ReturnScalar(sb.ToString(), this.ConnectionString, paramArray, true, 20).ToString());
                 

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                this.OrganizationID = dr.GetInt32(0);
                                this.CurrentTankConfigID = dr.GetInt32(1);
                                this.CurrentCapacityLimit = dr.GetDecimal(2);
                                this.CurrentTankCap = dr.GetDecimal(3);
                                this.CurrentLowLowLevel = dr.GetInt32(4);
                                this.CurrentLowLevel = dr.GetInt32(5);
                                this.CurrentHighLevel = dr.GetInt32(6);
                                this.CurrentFillDetectDelta = dr.GetDecimal(7);
                                this.CurrentShortFillDelta = dr.GetDecimal(8);
                                this.CurrentHighHighLevel = dr.GetInt32(9);
                                this.CurrentVolumeDelta = dr.GetInt32(10);
                                this.CurrentRateChangeDelta = dr.GetInt32(11);
                                this.CurrentSensorOffset = dr.GetDecimal(12);
                                this.CurrentDeviceFillHysteresis = dr.GetDecimal(13);
                                this.CurrentCallsperday = dr.GetInt32(14);
                                this.CurrentInterval = dr.GetInt32(15);
                                //this.CurrentDeviceFillDetect = dr.GetDecimal(16);
                                this.CurrentModelNumber = dr.GetString(16);
                                DeviceHasGPS = dr.GetBoolean(17);
                                DeviceHasModem = dr.GetBoolean(18);
                            }
                            dr.Close();
                        }
                        else
                        {
                            this.HaveError = true;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at GetCurrentTankConfigInfo - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
            return organizationid;
        }

        internal void TankIDRTUNumberCheck()
        {
            try
            {
                if (this.TankID == "*** Empty ***")
                {
                    this.HaveError = true;
                    this.StatusMessage = "Missing TankID or value is invalid";
                    this.BadColumn = "TankID";
                    this.BadColumnValue = "*** Empty ***";
                }
                if (this.CheckRTUCondition)
                {
                    if (this.RTUNumber == "*** Empty ***")
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Missing RTUNumber or value is invalid";
                        this.BadColumn = "RTUNumber";
                        this.BadColumnValue = "*** Empty ***";
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankIDRTUNumberCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal bool ValidateRTUNumber()
        {
            bool valid = false;
            try
            {
                using (var con = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("if exists(select 1 from device where deviceid = @deviceid and rtunumber = @rtunumber)select 'true' else select 'false'", con))
                    {
                        con.Open();
                        cmd.Parameters.Add(new SqlParameter("deviceid", this.CurrentTankDeviceID));
                        cmd.Parameters.Add(new SqlParameter("rtunumber", this.RTUNumber));
                        valid = (bool)cmd.ExecuteScalar();
                    //valid = bool.Parse(DAL.ReturnScalar(sb.ToString(), this.ConnectionString, paramArray, true, 20).ToString());
                }
                }

            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ValidateRTUNumber - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
            return valid;
        }


        #region DataType Checks

        internal void IntegerClassCheck(string columnname)
        {
            try
            {
                switch (columnname)
                {
                    case "RTUNumber":
                        if (this.RTUNumber != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.RTUNumber))
                                break;
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "RTUNumber Is Not An Integer";
                                this.BadColumn = "RTUNumber";
                                this.BadColumnValue = this.RTUNumber;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankID":
                        if (this.TankID != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.TankID))
                                break;
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankID Is Not An Integer";
                                this.BadColumn = "TankID";
                                this.BadColumnValue = this.TankID;
                                break;
                            }
                        }
                        else
                            break;
                    case "ReorderUsage":
                        if (this.ReorderUsage != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(ReorderUsage))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "ReorderUsage Is Not An Integer";
                                this.BadColumn = "ReorderUsage";
                                this.BadColumnValue = this.ReorderUsage;
                                break;
                            }
                        }
                        else
                            break;
                    case "SafetyStockUsage":
                        if (this.SafetyStockUsage != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.SafetyStockUsage))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "SafetyStockUsage Is Not An Integer";
                                this.BadColumn = "SafetyStockUsage";
                                this.BadColumnValue = this.SafetyStockUsage;
                                break;
                            }
                        }
                        else
                            break;
                    case "Callsperday":
                        if (this.Callsperday != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.Callsperday))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "Callsperday Is Not An Integer";
                                this.BadColumn = "Callsperday";
                                this.BadColumnValue = this.Callsperday;
                                break;
                            }
                        }
                        else
                            break;
                    case "CallDay":
                        if (this.CallDay != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.CallDay))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "CallDay Is Not An Integer";
                                this.BadColumn = "CallDay";
                                this.BadColumnValue = this.CallDay;
                                break;
                            }
                        }
                        else
                            break;
                    case "Interval":
                        if (this.Interval != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.Interval))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "Interval Is Not An Integer";
                                this.BadColumn = "Interval";
                                this.BadColumnValue = this.Interval;
                                break;
                            }
                        }
                        else
                            break;
                    case "DiagCallDayMask":
                        if (this.DiagCallDayMask != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.DiagCallDayMask))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DiagCallDayMask Is Not An Integer";
                                this.BadColumn = "DiagCallDayMask";
                                this.BadColumnValue = this.DiagCallDayMask;
                                break;
                            }
                        }
                        else
                            break;
                    case "LowLowLevel":
                        if (this.LowLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.LowLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "LowLowLevel Is Not An Integer";
                                this.BadColumn = "LowLowLevel";
                                this.BadColumnValue = this.LowLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "LowLevel":
                        if (this.LowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.LowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "LowLevel Is Not An Integer";
                                this.BadColumn = "LowLevel";
                                this.BadColumnValue = this.LowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "HighLevel":
                        if (this.HighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.HighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HighLevel Is Not An Integer";
                                this.BadColumn = "HighLevel";
                                this.BadColumnValue = this.HighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "HighHighLevel":
                        if (this.HighHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.HighHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HighHighLevel Is Not An Integer";
                                this.BadColumn = "HighHighLevel";
                                this.BadColumnValue = this.HighHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "VolumeDelta":
                        if (this.VolumeDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.VolumeDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "VolumeDelta Is Not An Integer";
                                this.BadColumn = "VolumeDelta";
                                this.BadColumnValue = this.VolumeDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "RateChangeDelta":
                        if (this.RateChangeDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.RateChangeDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "RateChangeDelta Is Not An Integer";
                                this.BadColumn = "RateChangeDelta";
                                this.BadColumnValue = this.RateChangeDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "DataLogDelta":
                        if (this.DataLogDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.DataLogDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DataLogDelta Is Not An Integer";
                                this.BadColumn = "DataLogDelta";
                                this.BadColumnValue = this.DataLogDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "UsageDelta":
                        if (this.UsageDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.UsageDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "UsageDelta Is Not An Integer";
                                this.BadColumn = "UsageDelta";
                                this.BadColumnValue = this.UsageDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "WakeInterval":
                        if (this.WakeInterval != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.WakeInterval))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "WakeInterval Is Not An Integer";
                                this.BadColumn = "WakeInterval";
                                this.BadColumnValue = this.WakeInterval;
                                break;
                            }
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at IntegerClassCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DecimalClassCheck(string columnname)
        {
            try
            {
                switch (columnname)
                {
                    case "TankHgt":
                        if (this.TankHgt != "*** Empty ***")
                        {
                            decimal tankhgt = 0;
                            if (Util.ConvertStringToDecimal(this.TankHgt))
                            {
                                tankhgt = decimal.Parse(this.TankHgt);
                                if (tankhgt < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "TankHgt < 0";
                                    this.BadColumn = "TankHgt";
                                    this.BadColumnValue = this.TankHgt;
                                    break;
                                }
                                else
                                {
                                    this.PerformUpdate = true;
                                    break;
                                }
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankHgt Is Not A Decimal";
                                this.BadColumn = "TankHgt";
                                this.BadColumnValue = this.TankHgt;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankCap":
                        if (this.TankCap != "*** Empty ***")
                        {
                            decimal tankcap = 0;
                            if (Util.ConvertStringToDecimal(this.TankCap))
                            {
                                tankcap = decimal.Parse(this.TankCap);
                                if (tankcap < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "TankCap < 0";
                                    this.BadColumn = "TankCap";
                                    this.BadColumnValue = this.TankCap;
                                    break;
                                }
                                else
                                {
                                    this.PerformUpdate = true;
                                    break;
                                }
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankCap Is Not A Decimal";
                                this.BadColumn = "TankCap";
                                this.BadColumnValue = this.TankCap;
                                break;
                            }
                        }
                        else
                            break;
                    case "CapacityLimit":
                        if (this.CapacityLimit != "*** Empty ***")
                        {
                            decimal capacitylimit = 0;
                            if (Util.ConvertStringToDecimal(this.CapacityLimit))
                            {
                                capacitylimit = decimal.Parse(this.CapacityLimit);
                                if (capacitylimit < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "CapacityLimit < 0";
                                    this.BadColumn = "CapacityLimit";
                                    this.BadColumnValue = this.CapacityLimit;
                                    break;
                                }
                                else
                                {
                                    this.PerformUpdate = true;
                                    break;
                                }
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "CapacityLimit Is Not An Decimal";
                                this.BadColumn = "CapacityLimit";
                                this.BadColumnValue = this.CapacityLimit;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankMinimum":
                        if (this.TankMinimum != "*** Empty ***")
                        {
                            decimal tankminimum = 0;
                            if (Util.ConvertStringToDecimal(this.TankMinimum))
                            {
                                tankminimum = decimal.Parse(this.TankMinimum);
                                if (tankminimum < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "TankMinimum < 0";
                                    this.BadColumn = "TankMinimum";
                                    this.BadColumnValue = this.TankMinimum;
                                    break;
                                }
                                else
                                {
                                    this.PerformUpdate = true;
                                    break;
                                }
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankMinimum Is Not An Decimal";
                                this.BadColumn = "TankMinimum";
                                this.BadColumnValue = this.TankMinimum;
                                break;
                            }
                        }
                        else
                            break;
                    case "FillDetectDelta":
                        if (this.FillDetectDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.FillDetectDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "FillDetectDelta Is Not A Decimal";
                                this.BadColumn = "FillDetectDelta";
                                this.BadColumnValue = this.FillDetectDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "ShortFillDelta":
                        if (this.ShortFillDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.ShortFillDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "ShortFillDelta Is Not A Decimal";
                                this.BadColumn = "ShortFillDelta";
                                this.BadColumnValue = this.ShortFillDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "HighSetPoint":
                        if (this.HighSetPoint != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.HighSetPoint))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HighSetPoint Is Not A Decimal";
                                this.BadColumn = "HighSetPoint";
                                this.BadColumnValue = this.HighSetPoint;
                                break;
                            }
                        }
                        else
                            break;
                    case "LowSetPoint":
                        if (this.LowSetPoint != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.LowSetPoint))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "LowSetPoint Is Not A Decimal";
                                this.BadColumn = "LowSetPoint";
                                this.BadColumnValue = this.LowSetPoint;
                                break;
                            }
                        }
                        else
                            break;
                    case "SensorOffset":
                        if (this.SensorOffset != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.SensorOffset))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "SensorOffset Is Not A Decimal";
                                this.BadColumn = "SensorOffset";
                                this.BadColumnValue = this.SensorOffset;
                                break;
                            }
                        }
                        else
                            break;
                    case "CoeffExp":
                        if (this.CoeffExp != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.CoeffExp))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "CoeffExp Is Not A Decimal";
                                this.BadColumn = "CoeffExp";
                                this.BadColumnValue = this.CoeffExp;
                                break;
                            }
                        }
                        else
                            break;
                    case "SpecGrav":
                        if (this.SpecGrav != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.SpecGrav))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "SpecGrav Is Not A Decimal";
                                this.BadColumn = "SpecGrav";
                                this.BadColumnValue = this.SpecGrav;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceFillDetectDelta":
                        if (this.DeviceFillDetectDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.DeviceFillDetectDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceFillDetectDelta Is Not A Decimal";
                                this.BadColumn = "DeviceFillDetectDelta";
                                this.BadColumnValue = this.DeviceFillDetectDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceFillHysteresis":
                        if (this.DeviceFillHysteresis != "*** Empty ***")
                        {
                            decimal devicefillhysteresis = 0;
                            if (Util.ConvertStringToDecimal(this.DeviceFillHysteresis))
                            {
                                devicefillhysteresis = decimal.Parse(this.DeviceFillHysteresis);
                                if (devicefillhysteresis < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "DeviceFillHysteresis < 0";
                                    this.BadColumn = "DeviceFillHysteresis";
                                    this.BadColumnValue = this.DeviceFillHysteresis;
                                    break;
                                }
                                else
                                {
                                    this.PerformUpdate = true;
                                    break;
                                }
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceFillHysteresis Is Not An Decimal";
                                this.BadColumn = "DeviceFillHysteresis";
                                this.BadColumnValue = this.DeviceFillHysteresis;
                                break;
                            }
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DecimalClassCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void BoolClassCheck(string columnname)
        {
            try
            {
                switch (columnname)
                {
                    case "DeviceCriticalLowLevel":
                        if (this.DeviceCriticalLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.DeviceCriticalLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceCriticalLowLevel Is Not A Boolean";
                                this.BadColumn = "DeviceCriticalLowLevel";
                                this.BadColumnValue = this.DeviceCriticalLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceLowLevel":
                        if (this.DeviceLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.DeviceLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceLowLevel Is Not A Boolean";
                                this.BadColumn = "DeviceLowLevel";
                                this.BadColumnValue = this.DeviceLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceHighLevel":
                        if (this.DeviceHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.DeviceHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceHighLevel Is Not A Boolean";
                                this.BadColumn = "DeviceHighLevel";
                                this.BadColumnValue = this.DeviceHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceCriticalHighLevel":
                        if (this.DeviceCriticalHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.DeviceCriticalHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceCriticalHighLevel Is Not A Boolean";
                                this.BadColumn = "DeviceCriticalHighLevel";
                                this.BadColumnValue = this.DeviceCriticalHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceFillDetect":
                        if (this.DeviceFillDetect != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.DeviceFillDetect))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceFillDetect Is Not A Boolean";
                                this.BadColumn = "DeviceFillDetect";
                                this.BadColumnValue = this.DeviceFillDetect;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceUsageAlarm":
                        if (this.DeviceUsageAlarm != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.DeviceUsageAlarm))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceUsageAlarm Is Not A Boolean";
                                this.BadColumn = "DeviceUsageAlarm";
                                this.BadColumnValue = this.DeviceUsageAlarm;
                                break;
                            }
                        }
                        else
                            break;
                    case "HasExpectedCallAlarm":
                        if (this.HasExpectedCallAlarm != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.HasExpectedCallAlarm))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HasExpectedCallAlarm Is Not A Boolean";
                                this.BadColumn = "HasExpectedCallAlarm";
                                this.BadColumnValue = this.HasExpectedCallAlarm;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankNormallyFills":
                        if (this.TankNormallyFills != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.TankNormallyFills))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankNormallyFills Is Not A Boolean";
                                this.BadColumn = "TankNormallyFills";
                                this.BadColumnValue = this.TankNormallyFills;
                                break;
                            }
                        }
                        else
                            break;
                    case "EnableGPS":
                        if (EnableGPS != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(EnableGPS))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "EnableGPS Is Not A Boolean";
                                this.BadColumn = "EnableGPS";
                                this.BadColumnValue = EnableGPS;
                                break;
                            }
                        }
                        else
                            break;
                    case "EnableLocation":
                        if (EnableLocation != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(EnableLocation))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "EnableLocation Is Not A Boolean";
                                this.BadColumn = "EnableLocation";
                                this.BadColumnValue = EnableLocation;
                                break;
                            }
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at BoolClassCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DateTimeClassCheck(string columnname)
        {
            try
            {
                switch (columnname)
                {
                    case "StartTime":
                        if (this.StartTime != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDateTime(this.StartTime))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "StartTime Is Not A DateTime";
                                this.BadColumn = "StartTime";
                                this.BadColumnValue = this.StartTime;
                                break;
                            }
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DateTimeClassCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        #endregion

        #region Business object validation

        //new method to verify that EnableGPS can be set to true based on Jon
        //Oct 2020
        //D Arcilla
        internal void EnableGPSCheck()
        {
            try
            {
                if (EnableGPS.ToUpper() == "TRUE")
                {
                    if (!DeviceHasGPS || !DeviceHasModem)
                    {
                        HaveError = true;
                        StatusMessage = "Device is not GPS enabled";
                        BadColumn = "EnableGPS";
                        BadColumnValue = EnableGPS;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at EnableGPSCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        //new method to verify that EnableLocation can be set to true based on Jon
        //Oct 2020
        //D Arcilla
        internal void EnableLocationCheck()
        {
            try
            {
                if (EnableLocation.ToUpper() == "TRUE")
                {
                    if (!DeviceHasModem)
                    {
                        HaveError = true;
                        StatusMessage = "Device has no modem";
                        BadColumn = "EnableLocation";
                        BadColumnValue = EnableLocation;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at EnableGPSCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void TankNameCheck()
        {
            try
            {
                if (this.TankName != "*** Empty ***")
                {
                    if (this.TankName.Length > 50)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Tank Name > 50 Characters";
                        this.BadColumn = "TankName";
                        this.BadColumnValue = this.TankName;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankNameCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void TankHgtCheck()
        {
            try
            {
                if (this.TankHgt != "*** Empty ***")
                {
                    if (decimal.Parse(this.TankHgt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) < 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "TankHgt < 1";
                        this.BadColumn = "TankHgt";
                        this.BadColumnValue = this.TankHgt;
                    }
                }
            }
            catch (Exception ex)
            {
               
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankHgtCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void TankCapCheck()
        {
            decimal tankcap;
            try
            {
                if (this.TankCap != "*** Empty ***")
                {
                    tankcap = decimal.Parse(this.TankCap);
                    if (tankcap < 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "TankCap < 1";
                        this.BadColumn = "TankCap";
                        this.BadColumnValue = this.TankCap;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankCapCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void CapcityLimitCheck()
        {
            try
            {
                decimal tankcapacity = 0;
                decimal capacitylimit = 0;
                if (this.CapacityLimit != "*** Empty ***")
                {
                    capacitylimit = decimal.Parse(this.CapacityLimit);
                    if (this.TankCap != "*** Empty ***")
                        tankcapacity = decimal.Parse(this.TankCap);
                    else
                        tankcapacity = this.CurrentTankCap;
                    if (capacitylimit > tankcapacity)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "CapacityLimit > TankCap";
                        this.BadColumn = "CapcityLimit";
                        this.BadColumnValue = this.CapacityLimit;
                    }
                    else if (capacitylimit != tankcapacity)
                        this.LimitCapacityFlag = "True";
                    else
                        this.LimitCapacityFlag = "False";
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at CapacityLimitCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void TankMinimumCheck()
        {
            try
            {
                decimal tankminimum = 0;
                decimal tankcapacity = 0;
                if (this.TankMinimum != "*** Empty ***")
                {
                    tankminimum = decimal.Parse(this.TankMinimum);
                    if (this.TankCap != "*** Empty ***")
                        tankcapacity = decimal.Parse(this.TankCap);
                    else
                        tankcapacity = this.CurrentTankCap;
                    if (tankminimum > tankcapacity)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "TankMinimum > TankCap";
                        this.BadColumn = "TankMinimum";
                        this.BadColumnValue = this.TankMinimum;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankMinimumCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void ReorderUsageCheck()
        {
            try
            {
                if (this.ReorderUsage != "*** Empty ***")
                {
                    if (int.Parse(this.ReorderUsage) < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ReorderUsage < 0";
                        this.BadColumn = "ReorderUsage";
                        this.BadColumnValue = this.ReorderUsage;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ReorderUsageCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void SafetyStockCheck()
        {
            try
            {
                if (this.SafetyStockUsage != "*** Empty ***")
                {
                    if (int.Parse(this.SafetyStockUsage) < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SafetyStockUsage < 0";
                        this.BadColumn = "SafetyStockUsage";
                        this.BadColumnValue = this.SafetyStockUsage;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at SafetyStockCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void StartTimeCheck()
        {
            try
            {
                DateTime starttime = new DateTime();
                int hour = 0;
                if (this.StartTime != "*** Empty ***")
                {
                    starttime = DateTime.Parse(this.StartTime);
                    hour = starttime.Hour;
                    if (hour < 0 || hour > 23)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid StartTime (Bad Hour)";
                        this.BadColumn = "StartTime";
                        this.BadColumnValue = this.StartTime;
                    }
                    else
                    {
                        int maxhour = ValidateStartHour(hour);
                        if (maxhour <= 23)
                        {
                        }
                        else
                        {
                            this.HaveError = true;
                            this.StatusMessage = "Interval StartTime Hour Conflict";
                            this.BadColumn = "StartTime";
                            this.BadColumnValue = this.StartTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at StartTimeCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        private int ValidateStartHour(int hour)
        {
            int maxHour = hour; 
            int interval, callsperday;
            try
            {
                
                if (this.Callsperday != "*** Empty ***")
                    callsperday = int.Parse(this.Callsperday);
                else
                    callsperday = this.CurrentCallsperday;

                if (this.Interval != "*** Empty ***")
                    interval = int.Parse(this.Interval);
                else
                    interval = this.CurrentInterval;
                for (int counter = 1; counter <= callsperday - 1; counter++)
                {
                    maxHour += interval;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ValidateStartHour - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
            return maxHour;
        }

        internal void IntervalCheck()
        {
            try
            {
                int interval = 0;
                if (this.Interval != "*** Empty ***")
                {
                    interval = int.Parse(this.Interval);
                    if (interval < 1 || interval > 23)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid Interval";
                        this.BadColumn = "Interval";
                        this.BadColumnValue = this.Interval;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at IntervalCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void CallsperdayCheck()
        {
            try
            {
                int callsperday = 0;
                if (this.Callsperday != "*** Empty ***")
                {
                    callsperday = int.Parse(this.Callsperday);
                    if (callsperday < 1 || callsperday > 23)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid Callsperday";
                        this.BadColumn = "Callsperday";
                        this.BadColumnValue = this.Callsperday;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at CallsperdayCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void CallDayCheck()
        {
            try
            {
                int callday = 0;
                if (this.CallDay != "*** Empty ***")
                {
                    callday = int.Parse(this.CallDay);
                    if (callday >= 1 && callday <= 127)
                    {
                    }
                    else
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid CallDay Mask";
                        this.BadColumn = "CallDay";
                        this.BadColumnValue = this.CallDay;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at CallDayCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DiagCallDayMaskCheck()
        {
            try
            {
                int diagcalldaymask = 0;
                if (this.DiagCallDayMask != "*** Empty ***")
                {
                    diagcalldaymask = int.Parse(this.DiagCallDayMask);
                    if (diagcalldaymask >= 1 && diagcalldaymask <= 127)
                    {
                    }
                    else
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid DiagCallDay Mask";
                        this.BadColumn = "DiagCallDayMask";
                        this.BadColumnValue = this.DiagCallDayMask;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DiagCallDayMaskCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void SensorOffsetCheck()
        {
            try
            {
                decimal tankhgt = 0;
                decimal sensoroffset = 0;
                if (this.SensorOffset != "*** Empty ***")
                {
                    sensoroffset = decimal.Parse(this.SensorOffset, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (this.TankHgt != "*** Empty ***")
                        tankhgt = decimal.Parse(this.TankHgt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    else
                        tankhgt = this.CurrentTankHgt;
                    if (sensoroffset <= tankhgt && sensoroffset >= 0)
                    {
                    }
                    else if (sensoroffset < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SensorOffset < 0";
                        this.BadColumn = "SensorOffset";
                        this.BadColumnValue = this.SensorOffset;
                    }
                    else
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SensorOffset > TankHgt";
                        this.BadColumn = "SensorOffset";
                        this.BadColumnValue = this.SensorOffset;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at SensorOffsetCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void CoeffExpCheck()
        {
            try
            {
                decimal coeffexp = 0;
                if (this.CoeffExp != "*** Empty ***")
                {
                    coeffexp = decimal.Parse(this.CoeffExp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (coeffexp <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "CoeffExp <= 0";
                        this.BadColumn = "CoeffExp";
                        this.BadColumnValue = this.CoeffExp;
                    }
                    if (coeffexp >= 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "CoeffExp >= 1";
                        this.BadColumn = "CoeffExp";
                        this.BadColumnValue = this.CoeffExp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at CoeffExpCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void SpecGravCheck()
        {
            try
            {
                decimal specgrav = 0;
                if (this.SpecGrav != "*** Empty ***")
                {
                    specgrav = decimal.Parse(this.SpecGrav, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (specgrav <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SpecGrav <= 0";
                        this.BadColumn = "SpecGrav";
                        this.BadColumnValue = this.SpecGrav;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at SpecGravCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void RateChangeDeltaCheck()
        {
            try
            {
                int ratechangedelta = 0;
                if (this.RateChangeDelta != "*** Empty ***")
                {
                    ratechangedelta = int.Parse(this.RateChangeDelta);
                    if (ratechangedelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "RateChangeDelta < 0";
                        this.BadColumn = "RateChangeDelta";
                        this.BadColumnValue = this.RateChangeDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at RateChangeDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void LowLowLevelCheck()
        {
            try
            {
                int lowlowlevel = 0;
                int lowlevel = 0;
                if (this.LowLowLevel != "*** Empty ***")
                {
                    lowlowlevel = int.Parse(this.LowLowLevel);
                    if (this.LowLevel != "*** Empty ***")
                        lowlevel = int.Parse(this.LowLevel);
                    else
                        lowlevel = this.CurrentLowLevel;
                    if (lowlowlevel > lowlevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLowLevel > LowLevel";
                        this.BadColumn = "LowLowLevel";
                        this.BadColumnValue = this.LowLowLevel;
                    }
                    if (lowlowlevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLowLevel < 0";
                        this.BadColumn = "LowLowLevel";
                        this.BadColumnValue = this.LowLowLevel;
                    }
                    if (lowlowlevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLowLevel > 100";
                        this.BadColumn = "LowLowLevel";
                        this.BadColumnValue = this.LowLowLevel;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at LowLowLevelCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void LowLevelCheck()
        {
            try
            {
                int lowlowlevel = 0;
                int lowlevel = 0;
                if (this.LowLevel != "*** Empty ***")
                {
                    lowlevel = int.Parse(this.LowLevel);
                    if (this.LowLowLevel != "*** Empty ***")
                        lowlowlevel = int.Parse(this.LowLowLevel);
                    else
                        lowlowlevel = this.CurrentLowLowLevel;
                    if (lowlowlevel > lowlevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLevel < LowLowLevel";
                        this.BadColumn = "LowLevel";
                        this.BadColumnValue = this.LowLevel;
                    }
                    if (lowlevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLevel < 0";
                        this.BadColumn = "LowLevel";
                        this.BadColumnValue = this.LowLevel;
                    }
                    if (lowlevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLevel > 100";
                        this.BadColumn = "LowLevel";
                        this.BadColumnValue = this.LowLevel;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at LowLevelCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void HighHighLevelCheck()
        {
            try
            {
                int highhighlevel = 0;
                int highlevel = 0;
                if (this.HighHighLevel != "*** Empty ***")
                {
                    highhighlevel = int.Parse(this.HighHighLevel);
                    if (this.HighLevel != "*** Empty ***")
                        highlevel = int.Parse(this.HighLevel);
                    else
                        highlevel = this.CurrentHighLevel;
                    if (highhighlevel < highlevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighHighLevel < HighLevel";
                        this.BadColumn = "HighHighLevel";
                        this.BadColumnValue = this.HighHighLevel;
                    }
                    if (highhighlevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighHighLevel < 0";
                        this.BadColumn = "HighHighLevel";
                        this.BadColumnValue = this.HighHighLevel;
                    }
                    if (highhighlevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighHighLevel > 100";
                        this.BadColumn = "HighHighLevel";
                        this.BadColumnValue = this.HighHighLevel;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at HighHighLevelCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void HighLevelCheck()
        {
            try
            {
                int highhighlevel = 0;
                int highlevel = 0;
                if (this.HighLevel != "*** Empty ***")
                {
                    highlevel = int.Parse(this.HighLevel);
                    if (this.HighHighLevel != "*** Empty ***")
                        highhighlevel = int.Parse(this.HighHighLevel);
                    else
                        highhighlevel = this.CurrentHighHighLevel;
                    if (highlevel > highhighlevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighLevel > HighHighLevel";
                        this.BadColumn = "HighLevel";
                        this.BadColumnValue = this.HighLevel;
                    }
                    if (highlevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighLevel < 0";
                        this.BadColumn = "HighLevel";
                        this.BadColumnValue = this.HighLevel;
                    }
                    if (highlevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighLevel > 100";
                        this.BadColumn = "HighLevel";
                        this.BadColumnValue = this.HighLevel;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at HighLevelCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void FillDetectDeltaCheck()
        {
            try
            {
                decimal capacitylimit = 0;
                decimal filldetectdelta = 0;
                decimal shortfilldelta = 0;
                if (this.FillDetectDelta != "*** Empty ***")
                {
                    filldetectdelta = decimal.Parse(this.FillDetectDelta);
                    if (this.TankCap != "*** Empty ***")
                        capacitylimit = decimal.Parse(this.CapacityLimit);
                    else
                        capacitylimit = this.CurrentCapacityLimit;
                    if (filldetectdelta <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "FillDetectDelta <= 0";
                        this.BadColumn = "FillDetectDelta";
                        this.BadColumnValue = this.FillDetectDelta;
                    }
                    if (filldetectdelta > capacitylimit)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "FillDetectDelta > CapacityLimit";
                        this.BadColumn = "FillDetectDelta";
                        this.BadColumnValue = this.FillDetectDelta;
                    }
                    if (this.ShortFillDelta != "*** Empty ***")
                        shortfilldelta = decimal.Parse(this.ShortFillDelta);
                    else
                        shortfilldelta = this.CurrentShortFillDelta;
                    if (filldetectdelta <= shortfilldelta)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "FillDetecDelta <= ShortFillDelta";
                        this.BadColumn = "FillDetectDelta";
                        this.BadColumnValue = this.FillDetectDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at FillDetectDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void ShortFillDeltaCheck()
        {
            try
            {
                decimal capacitylimit = 0;
                decimal filldetectdelta = 0;
                decimal shortfilldelta = 0;
                if (this.ShortFillDelta != "*** Empty ***")
                {
                    shortfilldelta = decimal.Parse(this.ShortFillDelta);
                    if (this.CapacityLimit != "*** Empty ***")
                        capacitylimit = decimal.Parse(this.CapacityLimit);
                    else
                        capacitylimit = this.CurrentCapacityLimit;
                    if (shortfilldelta <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ShortFillDelta <= 0";
                        this.BadColumn = "ShortFillDelta";
                        this.BadColumnValue = this.ShortFillDelta;
                    }
                    if (shortfilldelta > capacitylimit)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ShortFillDelta > CapacityLimit";
                        this.BadColumn = "ShortFillDelta";
                        this.BadColumnValue = this.ShortFillDelta;
                    }
                    if (this.FillDetectDelta != "*** Empty ***")
                        filldetectdelta = decimal.Parse(this.FillDetectDelta);
                    else
                        filldetectdelta = this.CurrentFillDetectDelta;
                    if (filldetectdelta <= shortfilldelta)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ShortFillDelta >= FillDetectDelta";
                        this.BadColumn = "ShortFillDelta";
                        this.BadColumnValue = this.ShortFillDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at ShortFillDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void VolumeDeltaCheck()
        {
            try
            {
                int volumedelta = 0;
                if (this.VolumeDelta != "*** Empty ***")
                {
                    volumedelta = int.Parse(this.VolumeDelta);
                    if (volumedelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "VolumeDelta < 0";
                        this.BadColumn = "VolumeDelta";
                        this.BadColumnValue = this.VolumeDelta;
                    }
                    if (volumedelta > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "VolumeDelta > 100";
                        this.BadColumn = "VolumeDelta";
                        this.BadColumnValue = this.VolumeDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at VolumeDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DeviceFillDetectDeltaCheck()
        {
            try
            {
                if (this.DeviceFillDetectDelta != "*** Empty ***")
                {
                    decimal capacitylimit, devicefillhysteresis, devicefilldetectdelta;
                    
                    devicefilldetectdelta = decimal.Parse(this.DeviceFillDetectDelta);
                    
                    if (this.DeviceFillHysteresis != "*** Empty ***")
                        devicefillhysteresis = decimal.Parse(this.DeviceFillHysteresis);
                    else
                        devicefillhysteresis = this.CurrentDeviceFillHysteresis;
                    
                    if (this.CapacityLimit != "*** Empty ***")
                        capacitylimit = decimal.Parse(this.CapacityLimit);
                    else
                        capacitylimit = this.CurrentCapacityLimit;
                    if (devicefilldetectdelta < devicefillhysteresis)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillDetectDelta < DeviceFillHysteresis";
                        this.BadColumn = "DeviceFillDetectDelta";
                        this.BadColumnValue = this.DeviceFillDetectDelta;
                    }
                    if (devicefilldetectdelta >= capacitylimit)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillDetectDelta > CapacityLimit";
                        this.BadColumn = "DeviceFillDetectDelta";
                        this.BadColumnValue = this.DeviceFillDetectDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DeviceFillDetectDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DeviceFillHysteresisCheck()
        {
            decimal devicefillhysteresis ,  devicefilldetect , tankcap ;
            try
            {
                if (this.DeviceFillHysteresis != "*** Empty ***")
                {
                    devicefillhysteresis = decimal.Parse(this.DeviceFillHysteresis);
                    if (this.DeviceFillDetect != "*** Empty ***")
                        devicefilldetect = decimal.Parse(this.DeviceFillDetect);
                    else
                        devicefilldetect = this.CurrentDeviceFillDetect;
                    if (this.TankCap != "*** Empty ***")
                        tankcap = decimal.Parse(this.TankCap);
                    else
                        tankcap = this.CurrentTankCap;
                    if (devicefilldetect < devicefillhysteresis)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillHysteresis > DeviceFillDetect";
                        this.BadColumn = "DeviceFillHysteresis";
                        this.BadColumnValue = this.DeviceFillHysteresis;
                    }
                    if (devicefillhysteresis > tankcap)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillHysteresis > TankCap";
                        this.BadColumn = "DeviceFillHysteresis";
                        this.BadColumnValue = this.DeviceFillHysteresis;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DeviceFillHysteresisCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void DataLogDeltaCheck()
        {
            decimal datalogdelta;
            try
            {
                if (this.DataLogDelta != "*** Empty ***")
                {
                    datalogdelta = decimal.Parse(this.DataLogDelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (datalogdelta > (1000))
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DataLogDelta > 1000";
                        this.BadColumn = "DataLogDelta";
                        this.BadColumnValue = this.DataLogDelta;
                    }
                    if (datalogdelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DataLogDelta < 0";
                        this.BadColumn = "DataLogDelta";
                        this.BadColumnValue = this.DataLogDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at DataLogDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void UsageDeltaCheck()
        {
            decimal tankcap, usagedelta;
            try
            {
                if (this.UsageDelta != "*** Empty ***")
                {
                     usagedelta = decimal.Parse(this.UsageDelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);

                    if (this.TankCap != "*** Empty ***")
                        tankcap = decimal.Parse(this.TankCap, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    else
                        tankcap = this.CurrentTankCap;
                    if (usagedelta > 1000)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "UsageDelta > 1000";
                        this.BadColumn = "UsageDelta";
                        this.BadColumnValue = this.UsageDelta;
                    }
                    if (usagedelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "UsageDelta < 0";
                        this.BadColumn = "UsageDelta";
                        this.BadColumnValue = this.UsageDelta;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at UsageDeltaCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void WakeIntervalCheck()
        {
            try
            {
                bool tl9xDevice = false;
                int wakeinterval = 0;
                if (this.WakeInterval != "*** Empty ***")
                {
                    wakeinterval = int.Parse(this.WakeInterval);
                    if (this.CurrentModelNumber.Substring(2, 1) == "H" || this.CurrentModelNumber.Substring(2, 1) == "L")
                        tl9xDevice = true;
                    if (!tl9xDevice && wakeinterval > 255)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "WakeInterval > 255";
                        this.BadColumn = "WakeInterval";
                        this.BadColumnValue = this.WakeInterval;
                    }
                    if (!tl9xDevice && wakeinterval > 1092)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "WakeInterval > 1092";
                        this.BadColumn = "WakeInterval";
                        this.BadColumnValue = this.WakeInterval;
                    }
                    if (wakeinterval < 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "WakeInterval < 1";
                        this.BadColumn = "WakeInterval";
                        this.BadColumnValue = this.WakeInterval;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at WakeIntervalCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }



        #endregion

        internal bool Add()
        {
            bool successfulupdate = false;
            try
            {
                successfulupdate = Util.UpdateTankConfig(this.ConnectionString, int.Parse(this.TankID), this.CurrentTankConfigID, this.UserID,
                                            this.TankName, this.TankHgt, this.TankCap, this.CapacityLimit,
                                            this.LimitCapacityFlag, this.TankMinimum, this.ReorderUsage,
                                            this.SafetyStockUsage, this.StartTime, this.Callsperday, this.CallDay,
                                            this.Interval, this.DiagCallDayMask, this.HighSetPoint, this.LowSetPoint,
                                            this.SensorOffset, this.CoeffExp, this.SpecGrav, this.LowLowLevel,
                                            this.LowLevel, this.HighLevel, this.HighHighLevel, this.FillDetectDelta,
                                            this.ShortFillDelta, this.VolumeDelta, this.RateChangeDelta,
                                            this.DeviceCriticalLowLevel, this.DeviceLowLevel, this.DeviceHighLevel,
                                            this.DeviceCriticalHighLevel, this.DeviceFillDetect, this.DeviceFillDetectDelta,
                                            this.DeviceFillHysteresis, this.DataLogDelta, this.UsageDelta, this.WakeInterval, this.DeviceUsageAlarm,
                                            this.HasExpectedCallAlarm, this.TankNormallyFills, EnableGPS, EnableLocation);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankConfig.Add - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
            return successfulupdate;
        }
    }

}
