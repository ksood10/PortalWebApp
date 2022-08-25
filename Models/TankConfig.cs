using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebApp.Models
{
    [Table("TankConfig_Test")]
    public class TankConfig
    {
        [Key]
        public int TankID                           { get; set; }
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
        public int Callsperday                      { get; set; }
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

        public TankConfig() { }
        public TankConfig(string conn, int userid)
        {
            this.ConnectionString = conn;
            this.UserID = userid;
        }
    }
    
}
