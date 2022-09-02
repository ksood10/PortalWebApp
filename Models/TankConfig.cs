using PortalWebApp.Areas.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PortalWebApp.Utilities;

namespace PortalWebApp.Models
{
    [Table("TankConfig_Test")]
    public class TankConfig
    {
        [Key]
        public int TankID                           { get; set; }
        public string xlTankID { get; set; }
        public string RTUNumber                     { get; set; }
        public DateTime StartTime                   { get; set; }
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
        public decimal TankHgt                          { get; set; }
        public decimal TankCap                          { get; set; }
        public bool LimitCapacityFlag                { get; set; }
        public decimal CapacityLimit                    { get; set; }
        public bool TankNormallyFills                { get; set; }
        public string ProdDesc                         { get; set; }
        public string UserProductNumber                { get; set; }
        public decimal SpecGrav                         { get; set; }
        public decimal CoeffExp                         { get; set; }
        public decimal SensorOffset                     { get; set; }
        public decimal LowSetPoint                      { get; set; }
        public decimal HighSetPoint                     { get; set; }
        public decimal TempOffset                       { get; set; }
        public decimal PulseValue                       { get; set; }
        public int HighHighLevel                    { get; set; }
        public int HighLevel                        { get; set; }
        public int LowLevel                         { get; set; }
        public int LowLowLevel                      { get; set; }
        public decimal FillDetectDelta                  { get; set; }
        public decimal ShortFillDelta                   { get; set; }
        public int HighTemp                         { get; set; }
        public int LowTemp                          { get; set; }
        public int TankSensorTypeID                 { get; set; }
        public string TankSensorLength                 { get; set; }
        public string TankSensorDesc                   { get; set; }
        public string TankSensorNumber                 { get; set; }
        public int CallsPerDay                      { get; set; }
        public int CallDay                          { get; set; }
        public int DiagCallDayMask                  { get; set; }
        public int UsageDelta                       { get; set; }
        public int WakeInterval                     { get; set; }
        public string TankNum                          { get; set; }
        public bool UpdateInventory                  { get; set; }
        public bool DeviceUsageAlarm                 { get; set; }
        public bool DeviceCriticalHighLevel          { get; set; }
        public bool DeviceHighLevel                  { get; set; }
        public bool DeviceLowLevel                   { get; set; }
        public bool DeviceCriticalLowLevel           { get; set; }
        public bool DeviceFillDetect                 { get; set; }
        public bool DeviceHighTemp                   { get; set; }
        public bool DeviceLowTemp                    { get; set; }
        public bool HasExpectedCallAlarm             { get; set; }
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
        public decimal DeviceFillDetectDelta            { get; set; }
        public decimal DeviceFillHysteresis             { get; set; }
        public int DataLogDelta                     { get; set; }
        public int EnableDeliveryReport             { get; set; }
        public int VolumeDelta                      { get; set; }
        public decimal VaporSensorRange                 { get; set; }
        public decimal ProductSensorRange               { get; set; }
        public decimal ForecastDailyUsage               { get; set; }
        public decimal TankMinimum                      { get; set; }
        public int ReorderUsage                     { get; set; }
        public int SafetyStockUsage                 { get; set; }
        public int DistributionLocationID           { get; set; }
        public int EnableLocation                   { get; set; }
        public int EnableGPS                        { get; set; }
        public int RateChangeDelta                  { get; set; }
        public int ProductID                        { get; set; }

        public string ConnectionString { get; set; }
        public int UserID { get; set; }
        public string ErrorFilePath { get; set; }
        public bool CheckRTUCondition { get; set; }
        public bool HaveError { get; set; }
        public string StatusMessage { get; set; }
        public string BadColumn { get; private set; }
        public string BadColumnValue { get; private set; }
        public bool PerformUpdate { get; private set; }
        public bool DeviceHasModem { get; private set; }
        public bool DeviceHasGPS { get; private set; }
        public decimal CurrentShortFillDelta { get; private set; }
        public decimal CurrentFillDetectDelta { get; private set; }
        public decimal CurrentCapacityLimit { get; private set; }
        public bool CurrentDeviceFillDetect { get; private set; }
        public decimal CurrentTankCap { get; private set; }
        public string CurrentModelNumber { get; private set; }
        public int CurrentTankConfigID { get; private set; }
       

        private string fileName;
        private string xlReorderUsage;
        private string xlSafetyStockUsage;
        private string xlCallDay;
        private string xlInterval;
        private string xlDiagCallDayMask;
        private string xlLowLowLevel;
        private string xlLowLevel;
        private string xlHighLevel;
        private string xlHighHighLevel;
        private string xlVolumeDelta;
        private string xlRateChangeDelta;
        private string xlDataLogDelta;
        private string xlWakeInterval;
        private string xlUsageDelta;
        private string xlTankHgt;
        private string xlTankCap;
        private string xlCapacityLimit;
        private string xlTankMinimum;
        private string xlFillDetectDelta;
        private string xlShortFillDelta;
        private string xlLowSetPoint;
        private string xlSensorOffset;
        private string xlCoeffExp;
        private string xlSpecGrav;
        private string xlDeviceFillDetectDelta;
        private string xlDeviceFillHysteresis;
        private string xlDeviceLowLevel;
        private string xlDeviceHighLevel;
        private string xlDeviceCriticalHighLevel;
        private string xlEnableGPS;
        private string xlEnableLocation;
        private string xlTankNormallyFills;
        private string xlHasExpectedCallAlarm;
        private string xlDeviceFillDetect;
        private string xlDeviceUsageAlarm;
        private string xlStartTime;
        private string xlLimitCapacityFlag;
        private decimal xlCurrentTankCap;
        private string xlCallsDay;
        private int xlCurrentCallsDay;
        private int xlCurrentInterval;
        private string xlHighSetPoint;
        private string xlDeviceCriticalLowLevel;

        public TankConfig() {
            TankID = 0;
        }
        public TankConfig(string conn, int userid)
        {
            this.ConnectionString = conn;
            this.UserID = userid;
        }


        internal void TankIDRTUNumberCheck()
        {
            try
            {
                if (this.TankID == 0)
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
                string errorMsg = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at TankIDRTUNumberCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
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
                        if (this.xlTankID != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlTankID))
                                break;
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankID Is Not An Integer";
                                this.BadColumn = "TankID";
                                this.BadColumnValue = this.xlTankID;
                                break;
                            }
                        }
                        else
                            break;
                    case "ReorderUsage":
                        if (this.xlReorderUsage != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(xlReorderUsage))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "ReorderUsage Is Not An Integer";
                                this.BadColumn = "ReorderUsage";
                                this.BadColumnValue = this.xlReorderUsage;
                                break;
                            }
                        }
                        else
                            break;
                    case "SafetyStockUsage":
                        if (this.xlSafetyStockUsage != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlSafetyStockUsage))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "SafetyStockUsage Is Not An Integer";
                                this.BadColumn = "SafetyStockUsage";
                                this.BadColumnValue = this.xlSafetyStockUsage;
                                break;
                            }
                        }
                        else
                            break;
                    //case "Callsperday":
                    //    if (this.CallsPerDay != "*** Empty ***")
                    //    {
                    //        if (Utilities.ConvertStringToInt(this.CallDay))
                    //        {
                    //            this.PerformUpdate = true;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            this.HaveError = true;
                    //            this.StatusMessage = "CallsPerDay Is Not An Integer";
                    //            this.BadColumn = "CallsPerDay";
                    //            this.BadColumnValue = this.CallrDay;
                    //            break;
                    //        }
                    //    }
                    //    else
                    //        break;
                    case "CallDay":
                        if (this.xlCallDay != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlCallDay))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "CallDay Is Not An Integer";
                                this.BadColumn = "CallDay";
                                this.BadColumnValue = this.xlCallDay;
                                break;
                            }
                        }
                        else
                            break;
                    case "Interval":
                        if (this.xlInterval != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlInterval))
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
                        if (this.xlDiagCallDayMask != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlDiagCallDayMask))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DiagCallDayMask Is Not An Integer";
                                this.BadColumn = "DiagCallDayMask";
                                this.BadColumnValue = this.xlDiagCallDayMask;
                                break;
                            }
                        }
                        else
                            break;
                    case "LowLowLevel":
                        if (this.xlLowLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlLowLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "LowLowLevel Is Not An Integer";
                                this.BadColumn = "LowLowLevel";
                                this.BadColumnValue = this.xlLowLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "LowLevel":
                        if (this.xlLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "LowLevel Is Not An Integer";
                                this.BadColumn = "LowLevel";
                                this.BadColumnValue = this.xlLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "HighLevel":
                        if (this.xlHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HighLevel Is Not An Integer";
                                this.BadColumn = "HighLevel";
                                this.BadColumnValue = this.xlHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "HighHighLevel":
                        if (this.xlHighHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlHighHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HighHighLevel Is Not An Integer";
                                this.BadColumn = "HighHighLevel";
                                this.BadColumnValue = this.xlHighHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "VolumeDelta":
                        if (this.xlVolumeDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlVolumeDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "VolumeDelta Is Not An Integer";
                                this.BadColumn = "VolumeDelta";
                                this.BadColumnValue = this.xlVolumeDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "RateChangeDelta":
                        if (this.xlRateChangeDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlRateChangeDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "RateChangeDelta Is Not An Integer";
                                this.BadColumn = "RateChangeDelta";
                                this.BadColumnValue = this.xlRateChangeDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "DataLogDelta":
                        if (this.xlDataLogDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlDataLogDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DataLogDelta Is Not An Integer";
                                this.BadColumn = "DataLogDelta";
                                this.BadColumnValue = this.xlDataLogDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "UsageDelta":
                        if (this.xlUsageDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlUsageDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "UsageDelta Is Not An Integer";
                                this.BadColumn = "UsageDelta";
                                this.BadColumnValue = this.xlUsageDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "WakeInterval":
                        if (this.xlWakeInterval != "*** Empty ***")
                        {
                            if (Util.ConvertStringToInt(this.xlWakeInterval))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "WakeInterval Is Not An Integer";
                                this.BadColumn = "WakeInterval";
                                this.BadColumnValue = this.xlWakeInterval;
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
                string errorMsg = ex.Message;
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
                        if (this.xlTankHgt != "*** Empty ***")
                        {
                            decimal tankhgt = 0;
                            if (Util.ConvertStringToDecimal(this.xlTankHgt))
                            {
                                tankhgt = decimal.Parse(this.xlTankHgt);
                                if (tankhgt < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "TankHgt < 0";
                                    this.BadColumn = "TankHgt";
                                    this.BadColumnValue = this.xlTankHgt;
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
                                this.BadColumnValue = this.xlTankHgt;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankCap":
                        if (this.xlTankCap != "*** Empty ***")
                        {
                            decimal tankcap = 0;
                            if (Util.ConvertStringToDecimal(this.xlTankCap))
                            {
                                tankcap = decimal.Parse(this.xlTankCap);
                                if (tankcap < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "TankCap < 0";
                                    this.BadColumn = "TankCap";
                                    this.BadColumnValue = this.xlTankCap;
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
                                this.BadColumnValue = this.xlTankCap;
                                break;
                            }
                        }
                        else
                            break;
                    case "CapacityLimit":
                        if (this.xlCapacityLimit != "*** Empty ***")
                        {
                            decimal capacitylimit = 0;
                            if (Util.ConvertStringToDecimal(this.xlCapacityLimit))
                            {
                                capacitylimit = decimal.Parse(this.xlCapacityLimit);
                                if (capacitylimit < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "CapacityLimit < 0";
                                    this.BadColumn = "CapacityLimit";
                                    this.BadColumnValue = this.xlCapacityLimit;
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
                                this.BadColumnValue = this.xlCapacityLimit;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankMinimum":
                        if (this.xlTankMinimum != "*** Empty ***")
                        {
                            decimal tankminimum = 0;
                            if (Util.ConvertStringToDecimal(this.xlTankMinimum))
                            {
                                tankminimum = decimal.Parse(this.xlTankMinimum);
                                if (tankminimum < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "TankMinimum < 0";
                                    this.BadColumn = "TankMinimum";
                                    this.BadColumnValue = this.xlTankMinimum;
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
                                this.BadColumnValue = this.xlTankMinimum;
                                break;
                            }
                        }
                        else
                            break;
                    case "FillDetectDelta":
                        if (this.xlFillDetectDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlFillDetectDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "FillDetectDelta Is Not A Decimal";
                                this.BadColumn = "FillDetectDelta";
                                this.BadColumnValue = this.xlFillDetectDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "ShortFillDelta":
                        if (this.xlShortFillDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlShortFillDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "ShortFillDelta Is Not A Decimal";
                                this.BadColumn = "ShortFillDelta";
                                this.BadColumnValue = this.xlShortFillDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "HighSetPoint":
                        if (this.xlHighSetPoint != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlHighSetPoint))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HighSetPoint Is Not A Decimal";
                                this.BadColumn = "HighSetPoint";
                                this.BadColumnValue = this.xlHighSetPoint;
                                break;
                            }
                        }
                        else
                            break;
                    case "LowSetPoint":
                        if (this.xlLowSetPoint != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlLowSetPoint))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "LowSetPoint Is Not A Decimal";
                                this.BadColumn = "LowSetPoint";
                                this.BadColumnValue = this.xlLowSetPoint;
                                break;
                            }
                        }
                        else
                            break;
                    case "SensorOffset":
                        if (this.xlSensorOffset != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlSensorOffset))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "SensorOffset Is Not A Decimal";
                                this.BadColumn = "SensorOffset";
                                this.BadColumnValue = this.xlSensorOffset;
                                break;
                            }
                        }
                        else
                            break;
                    case "CoeffExp":
                        if (this.xlCoeffExp != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlCoeffExp))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "CoeffExp Is Not A Decimal";
                                this.BadColumn = "CoeffExp";
                                this.BadColumnValue = this.xlCoeffExp;
                                break;
                            }
                        }
                        else
                            break;
                    case "SpecGrav":
                        if (this.xlSpecGrav != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlSpecGrav))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "SpecGrav Is Not A Decimal";
                                this.BadColumn = "SpecGrav";
                                this.BadColumnValue = this.xlSpecGrav;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceFillDetectDelta":
                        if (this.xlDeviceFillDetectDelta != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDecimal(this.xlDeviceFillDetectDelta))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceFillDetectDelta Is Not A Decimal";
                                this.BadColumn = "DeviceFillDetectDelta";
                                this.BadColumnValue = this.xlDeviceFillDetectDelta;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceFillHysteresis":
                        if (this.xlDeviceFillHysteresis != "*** Empty ***")
                        {
                            decimal devicefillhysteresis = 0;
                            if (Util.ConvertStringToDecimal(this.xlDeviceFillHysteresis))
                            {
                                devicefillhysteresis = decimal.Parse(this.xlDeviceFillHysteresis);
                                if (devicefillhysteresis < 0)
                                {
                                    this.HaveError = true;
                                    this.StatusMessage = "DeviceFillHysteresis < 0";
                                    this.BadColumn = "DeviceFillHysteresis";
                                    this.BadColumnValue = this.xlDeviceFillHysteresis;
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
                                this.BadColumnValue = this.xlDeviceFillHysteresis;
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
                string errorMsg = ex.Message;
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
                        if (this.xlDeviceCriticalLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlDeviceCriticalLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceCriticalLowLevel Is Not A Boolean";
                                this.BadColumn = "DeviceCriticalLowLevel";
                                this.BadColumnValue = this.xlDeviceCriticalLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceLowLevel":
                        if (this.xlDeviceLowLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlDeviceLowLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceLowLevel Is Not A Boolean";
                                this.BadColumn = "DeviceLowLevel";
                                this.BadColumnValue = this.xlDeviceLowLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceHighLevel":
                        if (this.xlDeviceHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlDeviceHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceHighLevel Is Not A Boolean";
                                this.BadColumn = "DeviceHighLevel";
                                this.BadColumnValue = this.xlDeviceHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceCriticalHighLevel":
                        if (this.xlDeviceCriticalHighLevel != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlDeviceCriticalHighLevel))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceCriticalHighLevel Is Not A Boolean";
                                this.BadColumn = "DeviceCriticalHighLevel";
                                this.BadColumnValue = this.xlDeviceCriticalHighLevel;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceFillDetect":
                        if (this.xlDeviceFillDetect != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlDeviceFillDetect))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceFillDetect Is Not A Boolean";
                                this.BadColumn = "DeviceFillDetect";
                                this.BadColumnValue = this.xlDeviceFillDetect;
                                break;
                            }
                        }
                        else
                            break;
                    case "DeviceUsageAlarm":
                        if (this.xlDeviceUsageAlarm != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlDeviceUsageAlarm))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "DeviceUsageAlarm Is Not A Boolean";
                                this.BadColumn = "DeviceUsageAlarm";
                                this.BadColumnValue = this.xlDeviceUsageAlarm;
                                break;
                            }
                        }
                        else
                            break;
                    case "HasExpectedCallAlarm":
                        if (this.xlHasExpectedCallAlarm != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlHasExpectedCallAlarm))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "HasExpectedCallAlarm Is Not A Boolean";
                                this.BadColumn = "HasExpectedCallAlarm";
                                this.BadColumnValue = this.xlHasExpectedCallAlarm;
                                break;
                            }
                        }
                        else
                            break;
                    case "TankNormallyFills":
                        if (this.xlTankNormallyFills != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(this.xlTankNormallyFills))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "TankNormallyFills Is Not A Boolean";
                                this.BadColumn = "TankNormallyFills";
                                this.BadColumnValue = this.xlTankNormallyFills;
                                break;
                            }
                        }
                        else
                            break;
                    case "EnableGPS":
                        if (xlEnableGPS != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(xlEnableGPS))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "EnableGPS Is Not A Boolean";
                                this.BadColumn = "EnableGPS";
                                this.BadColumnValue = xlEnableGPS;
                                break;
                            }
                        }
                        else
                            break;
                    case "EnableLocation":
                        if (xlEnableLocation != "*** Empty ***")
                        {
                            if (Util.ConvertStringToBool(xlEnableLocation))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "EnableLocation Is Not A Boolean";
                                this.BadColumn = "EnableLocation";
                                this.BadColumnValue = xlEnableLocation;
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
                string errorMsg = ex.Message;
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
                        if (this.xlStartTime != "*** Empty ***")
                        {
                            if (Util.ConvertStringToDateTime(this.xlStartTime))
                            {
                                this.PerformUpdate = true;
                                break;
                            }
                            else
                            {
                                this.HaveError = true;
                                this.StatusMessage = "StartTime Is Not A DateTime";
                                this.BadColumn = "StartTime";
                                this.BadColumnValue = this.xlStartTime;
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
                string errorMsg = ex.Message;
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
                if (xlEnableGPS.ToUpper() == "TRUE")
                {
                    if (!DeviceHasGPS || !DeviceHasModem)
                    {
                        HaveError = true;
                        StatusMessage = "Device is not GPS enabled";
                        BadColumn = "EnableGPS";
                        BadColumnValue = xlEnableGPS;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (xlEnableLocation.ToUpper() == "TRUE")
                {
                    if (!DeviceHasModem)
                    {
                        HaveError = true;
                        StatusMessage = "Device has no modem";
                        BadColumn = "EnableLocation";
                        BadColumnValue = xlEnableLocation;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                string errorMsg = ex.Message;
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
                if (this.TankHgt != 0)
                {
                    if (TankHgt < 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "TankHgt < 1";
                        this.BadColumn = "TankHgt";
                        this.BadColumnValue = this.xlTankHgt;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
            decimal tankcap = 0;
            try
            {
                if (this.TankCap != 0)
                {
                   // tankcap = decimal.Parse(TankCap);
                    if (TankCap < 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "TankCap < 1";
                        this.BadColumn = "TankCap";
                        this.BadColumnValue = TankCap.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.CapacityLimit != 0)
                {
                   // capacitylimit = decimal.Parse(this.xlCapacityLimit);
                    //if (this.TankCap != 0)
                    //    tankcapacity = decimal.Parse(TankCap);
                    //else
                    //    tankcapacity = this.xlCurrentTankCap;
                    if (CapacityLimit > TankCap)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "CapacityLimit > TankCap";
                        this.BadColumn = "CapcityLimit";
                        this.BadColumnValue = this.CapacityLimit.ToString();
                    }
                    else if (capacitylimit != tankcapacity)
                        this.LimitCapacityFlag = true;
                    else
                        this.LimitCapacityFlag = false;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                //decimal tankminimum = 0;
              //  decimal tankcapacity = 0;
                if (this.TankMinimum != 0)
                {
                    //tankminimum = decimal.Parse(this.xlTankMinimum);
                    //if (this.xlTankCap != "*** Empty ***")
                    //    tankcapacity = decimal.Parse(this.xlTankCap);
                    //else
                    //    tankcapacity = this.xlCurrentTankCap;
                    if (TankMinimum > TankCap)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "TankMinimum > TankCap";
                        this.BadColumn = "TankMinimum";
                        this.BadColumnValue = this.xlTankMinimum;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.ReorderUsage != 0)
                {
                    if (int.Parse(this.xlReorderUsage) < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ReorderUsage < 0";
                        this.BadColumn = "ReorderUsage";
                        this.BadColumnValue = this.ReorderUsage.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.SafetyStockUsage !=0)
                {
                    if (this.SafetyStockUsage < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SafetyStockUsage < 0";
                        this.BadColumn = "SafetyStockUsage";
                        this.BadColumnValue = this.SafetyStockUsage.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                //DateTime starttime = new DateTime();
                int hour = 0;
                if (this.StartTime != DateTime.MinValue)
                {
                   // starttime = DateTime.Parse(this.StartTime);
                    hour = StartTime.Hour;
                    if (hour < 0 || hour > 23)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid StartTime (Bad Hour)";
                        this.BadColumn = "StartTime";
                        this.BadColumnValue = this.StartTime.ToString(); ;
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
                            this.BadColumnValue = this.xlStartTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
            int callsperday = 0;
            int interval = 0;
            try
            {
                if (this.xlCallsDay != "*** Empty ***")
                    callsperday = int.Parse(this.xlCallsDay);
                else
                    callsperday = this.xlCurrentCallsDay;
                if (this.Interval != "*** Empty ***")
                    interval = int.Parse(this.Interval);
                else
                    interval = this.xlCurrentInterval;
                for (int counter = 1; counter <= callsperday - 1; counter++)
                {
                    maxHour = maxHour + interval;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                string errorMsg = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at IntervalCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void CallsPerDayCheck()
        {
            try
            {
                int callsperday = 0;
                if (this.xlCallsDay != "*** Empty ***")
                {
                    callsperday = int.Parse(this.xlCallsDay);
                    if (callsperday < 1 || callsperday > 23)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid CallsPerDay";
                        this.BadColumn = "CallsPerDay";
                        this.BadColumnValue = this.xlCallsDay;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
                FileWriter errorWriter = new FileWriter(fileName);
                errorWriter.Write("****************************");
                errorWriter.Write(DateTime.Now.ToString());
                errorWriter.Write("Error at CallsPerDayCheck - ");
                errorWriter.Write(ex.Message);
                errorWriter.Close();
            }
        }

        internal void CallDayCheck()
        {
            try
            {
                //int callday = 0;
                if (this.CallDay != 0)
                {
                   // callday = int.Parse(this.xlCallsDay);
                    if (CallDay >= 1 && CallDay <= 127)
                    {
                    }
                    else
                    {
                        this.HaveError = true;
                        this.StatusMessage = "Invalid CallDay Mask";
                        this.BadColumn = "CallDay";
                        this.BadColumnValue = this.CallDay.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
               // int diagcalldaymask = 0;
                if (this.DiagCallDayMask != 0)
                {
                    //diagcalldaymask = int.Parse(this.xlDiagCallDayMask);
                    if (DiagCallDayMask < 1 && DiagCallDayMask > 127)
                    { 
                        this.HaveError = true;
                        this.StatusMessage = "Invalid DiagCallDay Mask";
                        this.BadColumn = "DiagCallDayMask";
                        this.BadColumnValue = this.xlDiagCallDayMask;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.xlSensorOffset != "*** Empty ***")
                {
                    sensoroffset = decimal.Parse(this.xlSensorOffset, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (this.xlTankHgt != "*** Empty ***")
                        tankhgt = decimal.Parse(this.xlTankHgt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    else
                        tankhgt = this.TankHgt;
                    if (sensoroffset <= tankhgt && sensoroffset >= 0)
                    {
                    }
                    else if (sensoroffset < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SensorOffset < 0";
                        this.BadColumn = "SensorOffset";
                        this.BadColumnValue = this.xlSensorOffset;
                    }
                    else
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SensorOffset > TankHgt";
                        this.BadColumn = "SensorOffset";
                        this.BadColumnValue = this.xlSensorOffset;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.xlCoeffExp != "*** Empty ***")
                {
                    coeffexp = decimal.Parse(this.xlCoeffExp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (coeffexp <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "CoeffExp <= 0";
                        this.BadColumn = "CoeffExp";
                        this.BadColumnValue = this.xlCoeffExp;
                    }
                    if (coeffexp >= 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "CoeffExp >= 1";
                        this.BadColumn = "CoeffExp";
                        this.BadColumnValue = this.xlCoeffExp;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
              //  decimal specgrav = 0;
                if (this.SpecGrav != 0)
                {
                   // specgrav = decimal.Parse(this.SpecGrav, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (SpecGrav <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "SpecGrav <= 0";
                        this.BadColumn = "SpecGrav";
                        this.BadColumnValue = this.SpecGrav.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.RateChangeDelta != 0)
                {
                    //ratechangedelta = int.Parse(this.RateChangeDelta);
                    if (RateChangeDelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "RateChangeDelta < 0";
                        this.BadColumn = "RateChangeDelta";
                        this.BadColumnValue = this.RateChangeDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
               // int lowlowlevel = 0;
               // int lowlevel = 0;
                if (this.LowLowLevel != 0)
                {
                   // lowlowlevel = int.Parse(this.LowLowLevel);
                    //if (this.LowLevel != 0)
                    //    lowlevel = int.Parse(this.LowLevel);
                    //else
                    //    lowlevel = this.CurrentLowLevel;
                    if (LowLowLevel > LowLevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLowLevel > LowLevel";
                        this.BadColumn = "LowLowLevel";
                        this.BadColumnValue = this.LowLowLevel.ToString();
                    }
                    if (LowLowLevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLowLevel < 0";
                        this.BadColumn = "LowLowLevel";
                        this.BadColumnValue = this.LowLowLevel.ToString();
                    }
                    if (LowLowLevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLowLevel > 100";
                        this.BadColumn = "LowLowLevel";
                        this.BadColumnValue = this.LowLowLevel.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                //int lowlowlevel = 0;
                //int lowlevel = 0;
                if (this.LowLevel != 0)
                {
                    //lowlevel = int.Parse(this.LowLevel);
                    //if (this.LowLowLevel != "*** Empty ***")
                    //    lowlowlevel = int.Parse(this.LowLowLevel);
                    //else
                    //    lowlowlevel = this.CurrentLowLowLevel;
                    if (LowLowLevel > LowLevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLevel < LowLowLevel";
                        this.BadColumn = "LowLevel";
                        this.BadColumnValue = this.LowLevel.ToString();
                    }
                    if (LowLevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLevel < 0";
                        this.BadColumn = "LowLevel";
                        this.BadColumnValue = this.LowLevel.ToString();
                    }
                    if (LowLevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "LowLevel > 100";
                        this.BadColumn = "LowLevel";
                        this.BadColumnValue = this.LowLevel.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
               // int highhighlevel = 0;
               // int highlevel = 0;
                if (this.HighHighLevel != 0)
                {
                    //highhighlevel = int.Parse(this.HighHighLevel);
                    //if (this.HighLevel != "*** Empty ***")
                    //    highlevel = int.Parse(this.HighLevel);
                    //else
                    //    highlevel = this.CurrentHighLevel;
                    if (HighHighLevel < HighLevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighHighLevel < HighLevel";
                        this.BadColumn = "HighHighLevel";
                        this.BadColumnValue = this.HighHighLevel.ToString();
                    }
                    if (HighHighLevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighHighLevel < 0";
                        this.BadColumn = "HighHighLevel";
                        this.BadColumnValue = this.HighHighLevel.ToString();
                    }
                    if (HighHighLevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighHighLevel > 100";
                        this.BadColumn = "HighHighLevel";
                        this.BadColumnValue = this.HighHighLevel.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                //int highhighlevel = 0;
                //int highlevel = 0;
                if (this.HighLevel != 0)
                {
                    //highlevel = int.Parse(this.HighLevel);
                    //if (this.HighHighLevel != "*** Empty ***")
                    //    highhighlevel = int.Parse(this.HighHighLevel);
                    //else
                    //    highhighlevel = this.CurrentHighHighLevel;
                    if (HighLevel > HighHighLevel)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighLevel > HighHighLevel";
                        this.BadColumn = "HighLevel";
                        this.BadColumnValue = this.HighLevel.ToString();
                    }
                    if (HighLevel < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighLevel < 0";
                        this.BadColumn = "HighLevel";
                        this.BadColumnValue = this.HighLevel.ToString();
                    }
                    if (HighLevel > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "HighLevel > 100";
                        this.BadColumn = "HighLevel";
                        this.BadColumnValue = this.HighLevel.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                //decimal capacitylimit = 0;
                //decimal filldetectdelta = 0;
                //decimal shortfilldelta = 0;
                if (this.FillDetectDelta != 0)
                {
                    //filldetectdelta = decimal.Parse(this.FillDetectDelta);
                    //if (this.TankCap != "*** Empty ***")
                    //    capacitylimit = decimal.Parse(this.CapacityLimit);
                    //else
                    //    capacitylimit = this.CurrentCapacityLimit;
                    if (FillDetectDelta <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "FillDetectDelta <= 0";
                        this.BadColumn = "FillDetectDelta";
                        this.BadColumnValue = this.FillDetectDelta.ToString();
                    }
                    if (FillDetectDelta > CapacityLimit)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "FillDetectDelta > CapacityLimit";
                        this.BadColumn = "FillDetectDelta";
                        this.BadColumnValue = this.FillDetectDelta.ToString();
                    }
                    if (this.ShortFillDelta ==0)
                        ShortFillDelta = this.CurrentShortFillDelta;
                    if (FillDetectDelta <= ShortFillDelta)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "FillDetecDelta <= ShortFillDelta";
                        this.BadColumn = "FillDetectDelta";
                        this.BadColumnValue = this.FillDetectDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                //decimal capacitylimit = 0;
                //decimal filldetectdelta = 0;
                //decimal shortfilldelta = 0;
                if (this.ShortFillDelta != 0)
                {
                    //shortfilldelta = decimal.Parse(this.ShortFillDelta);
                    //if (this.CapacityLimit != "*** Empty ***")
                    //    capacitylimit = decimal.Parse(this.CapacityLimit);
                    //else
                    //    capacitylimit = this.CurrentCapacityLimit;
                    if (ShortFillDelta <= 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ShortFillDelta <= 0";
                        this.BadColumn = "ShortFillDelta";
                        this.BadColumnValue = this.ShortFillDelta.ToString();
                    }
                    if (ShortFillDelta > CapacityLimit)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ShortFillDelta > CapacityLimit";
                        this.BadColumn = "ShortFillDelta";
                        this.BadColumnValue = this.ShortFillDelta.ToString();
                    }
                    if (this.FillDetectDelta==0)
                        FillDetectDelta = this.CurrentFillDetectDelta;
                    if (FillDetectDelta <= ShortFillDelta)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "ShortFillDelta >= FillDetectDelta";
                        this.BadColumn = "ShortFillDelta";
                        this.BadColumnValue = this.ShortFillDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
               // int volumedelta = 0;
                if (this.VolumeDelta != 0)
                {
                    //volumedelta = int.Parse(this.VolumeDelta.ToString());
                    if (VolumeDelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "VolumeDelta < 0";
                        this.BadColumn = "VolumeDelta";
                        this.BadColumnValue = this.VolumeDelta.ToString();
                    }
                    if (VolumeDelta > 100)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "VolumeDelta > 100";
                        this.BadColumn = "VolumeDelta";
                        this.BadColumnValue = this.VolumeDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
            decimal devicefillhysteresis = 0; ;
            decimal devicefilldetectdelta = 0;
            decimal capacitylimit = 0;
            try
            {
                if (this.DeviceFillDetectDelta != 0)
                {
                    //devicefilldetectdelta = decimal.Parse(this.DeviceFillDetectDelta);
                    //if (this.DeviceFillHysteresis != "*** Empty ***")
                    //    devicefillhysteresis = decimal.Parse(this.DeviceFillHysteresis);
                    //else
                    //    devicefillhysteresis = this.CurrentDeviceFillHysteresis;
                    if (this.CapacityLimit == 0)
                    //    capacitylimit = decimal.Parse(this.CapacityLimit);
                    //else
                        CapacityLimit = this.CurrentCapacityLimit;
                    if (DeviceFillDetectDelta < DeviceFillHysteresis)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillDetectDelta < DeviceFillHysteresis";
                        this.BadColumn = "DeviceFillDetectDelta";
                        this.BadColumnValue = this.DeviceFillDetectDelta.ToString();
                    }
                    if (DeviceFillDetectDelta >= CapacityLimit)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillDetectDelta > CapacityLimit";
                        this.BadColumn = "DeviceFillDetectDelta";
                        this.BadColumnValue = this.DeviceFillDetectDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
            decimal devicefillhysteresis = 0; ;
            decimal devicefilldetect = 0;
            decimal tankcap = 0;
            try
            {
                if (this.DeviceFillHysteresis != 0)
                {
                   // devicefillhysteresis = decimal.Parse(this.DeviceFillHysteresis);
                    //if (this.DeviceFillDetect == 0)
                    //    devicefilldetect = decimal.Parse(this.DeviceFillDetect);
                    //else
                        DeviceFillDetect = this.CurrentDeviceFillDetect;
                    if (this.TankCap == 0)
                         TankCap = this.CurrentTankCap;
                    if (DeviceFillDetectDelta  < DeviceFillHysteresis)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillHysteresis > DeviceFillDetect";
                        this.BadColumn = "DeviceFillHysteresis";
                        this.BadColumnValue = this.DeviceFillHysteresis.ToString();
                    }
                    if (DeviceFillHysteresis > TankCap)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DeviceFillHysteresis > TankCap";
                        this.BadColumn = "DeviceFillHysteresis";
                        this.BadColumnValue = this.DeviceFillHysteresis.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
            decimal datalogdelta = 0; ;
            try
            {
                if (this.DataLogDelta !=0)
                {
                  //  datalogdelta = decimal.Parse(this.DataLogDelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (DataLogDelta > (1000))
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DataLogDelta > 1000";
                        this.BadColumn = "DataLogDelta";
                        this.BadColumnValue = this.DataLogDelta.ToString();
                    }
                    if (datalogdelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "DataLogDelta < 0";
                        this.BadColumn = "DataLogDelta";
                        this.BadColumnValue = this.DataLogDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
           // decimal usagedelta = 0; ;
           // decimal tankcap = 0;
            try
            {
                if (this.UsageDelta != 0)
                {
                   // usagedelta = decimal.Parse(this.UsageDelta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    if (this.TankCap == 0)
                    //    tankcap = decimal.Parse(this.TankCap, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                    //else
                        TankCap = CurrentTankCap;
                    if (UsageDelta > 1000)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "UsageDelta > 1000";
                        this.BadColumn = "UsageDelta";
                        this.BadColumnValue = this.UsageDelta.ToString();
                    }
                    if (UsageDelta < 0)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "UsageDelta < 0";
                        this.BadColumn = "UsageDelta";
                        this.BadColumnValue = this.UsageDelta.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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
                if (this.WakeInterval != 0)
                {
                    //wakeinterval = int.Parse(this.WakeInterval);
                    if (this.CurrentModelNumber.Substring(2, 1) == "H" || this.CurrentModelNumber.Substring(2, 1) == "L")
                        tl9xDevice = true;
                    if (!tl9xDevice && WakeInterval > 255)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "WakeInterval > 255";
                        this.BadColumn = "WakeInterval";
                        this.BadColumnValue = this.WakeInterval.ToString();
                    }
                    if (!tl9xDevice && wakeinterval > 1092)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "WakeInterval > 1092";
                        this.BadColumn = "WakeInterval";
                        this.BadColumnValue = this.WakeInterval.ToString();
                    }
                    if (WakeInterval < 1)
                    {
                        this.HaveError = true;
                        this.StatusMessage = "WakeInterval < 1";
                        this.BadColumn = "WakeInterval";
                        this.BadColumnValue = this.WakeInterval.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
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

        //internal bool Add()
        //{
        //    bool successfulupdate = false;
        //    try
        //    {
        //        successfulupdate = Utilities.Utilities.UpdateTankConfig(this.ConnectionString, this.TankID, this.CurrentTankConfigID, this.UserID,
        //                                    this.TankName, this.TankHgt, this.TankCap, this.CapacityLimit,
        //                                    this.LimitCapacityFlag, this.TankMinimum, this.ReorderUsage,
        //                                    this.SafetyStockUsage, this.StartTime, this.CallsPerDay, this.CallDay,
        //                                    this.Interval, this.DiagCallDayMask, this.HighSetPoint, this.LowSetPoint,
        //                                    this.SensorOffset, this.CoeffExp, this.SpecGrav, this.LowLowLevel,
        //                                    this.LowLevel, this.HighLevel, this.HighHighLevel, this.FillDetectDelta,
        //                                    this.ShortFillDelta, this.VolumeDelta, this.RateChangeDelta,
        //                                    this.DeviceCriticalLowLevel, this.DeviceLowLevel, this.DeviceHighLevel,
        //                                    this.DeviceCriticalHighLevel, this.DeviceFillDetect, this.DeviceFillDetectDelta,
        //                                    this.DeviceFillHysteresis, this.DataLogDelta, this.UsageDelta, this.WakeInterval, this.DeviceUsageAlarm,
        //                                    this.HasExpectedCallAlarm, this.TankNormallyFills, EnableGPS, EnableLocation);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMsg = ex.Message;
        //        fileName = this.ErrorFilePath + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + ".TXT";
        //        FileWriter errorWriter = new FileWriter(fileName);
        //        errorWriter.Write("****************************");
        //        errorWriter.Write(DateTime.Now.ToString());
        //        errorWriter.Write("Error at TankConfig.Add - ");
        //        errorWriter.Write(ex.Message);
        //        errorWriter.Close();
        //    }
        //    return successfulupdate;
        //}
    }

}
