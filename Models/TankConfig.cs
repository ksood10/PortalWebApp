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
        public int TankName                         { get; set; }
        public int UserTankNumber                   { get; set; }
        public int TransportStatusID                { get; set; }
        public int ServicePlanID                    { get; set; }
        public int UserDefined1                     { get; set; }
        public int UserDefined2                     { get; set; }
        public int UserDefined3                     { get; set; }
        public int UserDefined4                     { get; set; }
        public int RatePlanID                       { get; set; }
        public int InstallationTypeID               { get; set; }
        public int InstallationStatus               { get; set; }
        public int InstallDate                      { get; set; }
        public int ServiceDate                      { get; set; }
        public int Region                           { get; set; }
        public int Route                            { get; set; }
        public int ChartID                          { get; set; }
        public int UnitOfMeasureID                  { get; set; }
        public int TankHgt                          { get; set; }
        public int TankCap                          { get; set; }
        public int LimitCapacityFlag                { get; set; }
        public int CapacityLimit                    { get; set; }
        public int TankNormallyFills                { get; set; }
        public int ProdDesc                         { get; set; }
        public int UserProductNumber                { get; set; }
        public int SpecGrav                         { get; set; }
        public int CoeffExp                         { get; set; }
        public int SensorOffset                     { get; set; }
        public int LowSetPoint                      { get; set; }
        public int HighSetPoint                     { get; set; }
        public int TempOffset                       { get; set; }
        public int PulseValue                       { get; set; }
        public int HighHighLevel                    { get; set; }
        public int HighLevel                        { get; set; }
        public int LowLevel                         { get; set; }
        public int LowLowLevel                      { get; set; }
        public int FillDetectDelta                  { get; set; }
        public int ShortFillDelta                   { get; set; }
        public int HighTemp                         { get; set; }
        public int LowTemp                          { get; set; }
        public int TankSensorTypeID                 { get; set; }
        public int TankSensorLength                 { get; set; }
        public int TankSensorDesc                   { get; set; }
        public int TankSensorNumber                 { get; set; }
        public int Callsperday                      { get; set; }
        public int CallDay                          { get; set; }
        public int DiagCallDayMask                  { get; set; }
        public int UsageDelta                       { get; set; }
        public int WakeInterval                     { get; set; }
        public int TankNum                          { get; set; }
        public int UpdateInventory                  { get; set; }
        public int DeviceUsageAlarm                 { get; set; }
        public int DeviceCriticalHighLevel          { get; set; }
        public int DeviceHighLevel                  { get; set; }
        public int DeviceLowLevel                   { get; set; }
        public int DeviceCriticalLowLevel           { get; set; }
        public int DeviceFillDetect                 { get; set; }
        public int DeviceHighTemp                   { get; set; }
        public int DeviceLowTemp                    { get; set; }
        public int HasExpectedCallAlarm             { get; set; }
        public int ExpectedCallInterval             { get; set; }
        public int Active                           { get; set; }
        public int CreatedBy                        { get; set; }
        public int CreatedOn                        { get; set; }
        public int ModifiedBy                       { get; set; }
        public int ModifiedOn                       { get; set; }
        public int Stamp                            { get; set; }
        public int GroupId                          { get; set; }
        public int DeviceSuspiciousFilter           { get; set; }
        public int DeviceSuspiciousDelta            { get; set; }
        public int DeviceFillDetectDelta            { get; set; }
        public int DeviceFillHysteresis             { get; set; }
        public int DataLogDelta                     { get; set; }
        public int EnableDeliveryReport             { get; set; }
        public int VolumeDelta                      { get; set; }
        public int VaporSensorRange                 { get; set; }
        public int ProductSensorRange               { get; set; }
        public int ForecastDailyUsage               { get; set; }
        public int TankMinimum                      { get; set; }
        public int ReorderUsage                     { get; set; }
        public int SafetyStockUsage                 { get; set; }
        public int DistributionLocationID           { get; set; }
        public int EnableLocation                   { get; set; }
        public int EnableGPS                        { get; set; }
        public int RateChangeDelta                  { get; set; }
        public int ProductID                        { get; set; }

    }
    
}
