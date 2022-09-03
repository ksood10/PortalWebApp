using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using PortalWebApp.Data;
using PortalWebApp.Models;
using PortalWebApp.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static PortalWebApp.Utilities.Util;

namespace PortalWebApp.Controllers
{
    public class HomeController : Controller
    {
        private PortalWebAppContext _databaseContext;
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment Environment;
        private StringValues file_1;
        int rws = 0;

        public bool HaveEXCELReadError = false;

        public HomeController(ILogger<HomeController> logger, PortalWebAppContext databaseContext, IWebHostEnvironment _environment)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }   

        public IActionResult check(IFormFile postedFile, string button, BulkUpdate model)
        {
           // myBulkConfigurator = new BulkConfiguratorQueue();  string s = myBulkConfigurator.TestDLL();  connectionString = Properties.Resources.TankDataTestDatabase;
            var filename = Path.GetFileName(model.FileName);
            TempData["ButtonValue"] = string.Format("Env--{0} :::  User -- {1}::: ThrottleNum -- {2}:::Duration-- {3}:::RTU--{4}:::File--{5}", model.Environment, model.UserID, model.ThrottleNum, model.ThrottleDuration, model.RTU, model.FileName);
            return RedirectToAction("BulkConfig");
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BulkConfig()
        {
            var userList = (from user in _databaseContext.User
                            where user.OrganizationID == 10
                            orderby user.UserId
                            select new SelectListItem()
                            {
                                Text = user.AbbreviatedName + " (" + user.UserId.ToString() + ")",
                                Value = user.UserId.ToString()
                            }).ToList();
            userList.Insert(0, new SelectListItem()
            {
                Text = "---------Select-------------",
                Value = string.Empty
            });
            ViewBag.ListofUser = userList;
            return View();
        }
        [HttpPost]
        public IActionResult Index(UserViewModel userViewModel)
        {
            return View(userViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

        [HttpPost]
        public IActionResult ImportExcelFile(BulkUpdate model)
        {
            var dt= GetDataTableFromExcelFile(model);
            if (dt == null)               this.HaveEXCELReadError = true;
           
            if(!HaveEXCELReadError) {
                GetColumnOrdinals(dt);
                using (var con = new SqlConnection(model.Environment))
                {
                    using (var cmd = new SqlCommand("BulkTankConfig_Insert", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(GetSqlParams());
                     

                        rws = dt.Rows.Count;
                        model.TotalRows = rws;
                       
                        foreach (DataRow dr in dt.Rows)
                        {
                            cmd.Parameters["@TankID"].Value = dr[tankIDOrdinal];
                            cmd.Parameters["@RTUNumber"].Value = dr[rtuNumberOrdinal];
                            cmd.Parameters["@TankHgt"].Value = (dr[tankHgtOrdinal] == DBNull.Value) ? 0 : dr[tankHgtOrdinal];
                            cmd.Parameters["@TankCap"].Value = (dr[tankCapOrdinal] == DBNull.Value) ? 0 : dr[tankCapOrdinal];
                            cmd.Parameters["@CapacityLimit"].Value = (dr[capacityLimitOrdinal] == DBNull.Value) ? 0 : dr[capacityLimitOrdinal];
                            cmd.Parameters["@TankMinimum"].Value = (dr[tankMinimumOrdinal] == DBNull.Value) ? 0 : dr[tankMinimumOrdinal];
                            cmd.Parameters["@ReorderUsage"].Value = (dr[reorderUsageOrdinal] == DBNull.Value) ? 0 : dr[reorderUsageOrdinal];
                            cmd.Parameters["@SafetyStockUsage"].Value = (dr[safetyStockUsageOrdinal] == DBNull.Value) ? 0 : dr[safetyStockUsageOrdinal];

                            cmd.Parameters["@Callsperday"].Value = (dr[callsPerDayOrdinal] == DBNull.Value) ? 0 : dr[callsPerDayOrdinal];
                            cmd.Parameters["@CallDay"].Value = (dr[callDayOrdinal] == DBNull.Value) ? 0 : dr[callDayOrdinal];
                            cmd.Parameters["@StartTime"].Value = dr[startTimeOrdinal];

                            cmd.Parameters["@Interval"].Value = (dr[intervalOrdinal] == DBNull.Value) ? "0" : dr[intervalOrdinal];
                            cmd.Parameters["@DiagCallDayMask"].Value = (dr[diagCallDayMaskOrdinal] == DBNull.Value) ? 0 : dr[diagCallDayMaskOrdinal];
                            cmd.Parameters["@HighSetPoint"].Value = (dr[highSetPointOrdinal] == DBNull.Value) ? 0 : dr[highSetPointOrdinal];
                            cmd.Parameters["@LowSetPoint"].Value = (dr[lowSetPointOrdinal] == DBNull.Value) ? 0 : dr[lowSetPointOrdinal];
                            cmd.Parameters["@SensorOffset"].Value = (dr[sensorOffsetOrdinal] == DBNull.Value) ? 0 : dr[sensorOffsetOrdinal];
                            cmd.Parameters["@CoeffExp"].Value = (dr[coeffExpOrdinal] == DBNull.Value) ? 0 : dr[coeffExpOrdinal];
                            cmd.Parameters["@SpecGrav"].Value = (dr[specGravOrdinal] == DBNull.Value) ? 0 : dr[specGravOrdinal];
                            cmd.Parameters["@LowLowLevel"].Value = (dr[lowLowLevelOrdinal] == DBNull.Value) ? 0 : dr[lowLowLevelOrdinal];
                            cmd.Parameters["@LowLevel"].Value = (dr[lowLevelOrdinal] == DBNull.Value) ? 0 : dr[lowLevelOrdinal];
                            cmd.Parameters["@HighLevel"].Value = (dr[highLevelOrdinal] == DBNull.Value) ? 0 : dr[highLevelOrdinal];
                            cmd.Parameters["@HighHighLevel"].Value = (dr[highHighLevelOrdinal] == DBNull.Value) ? 0 : dr[highHighLevelOrdinal];
                            cmd.Parameters["@ShortFillDelta"].Value = (dr[shortFillDeltaOrdinal] == DBNull.Value) ? 0 : dr[shortFillDeltaOrdinal];
                            cmd.Parameters["@FillDetectDelta"].Value = (dr[fillDetectDeltaOrdinal] == DBNull.Value) ? 0 : dr[fillDetectDeltaOrdinal];
                            cmd.Parameters["@VolumeDelta"].Value = (dr[volumeDeltaOrdinal] == DBNull.Value) ? 0 : dr[volumeDeltaOrdinal];
                            cmd.Parameters["@RateChangeDelta"].Value = (dr[rateChangeDeltaOrdinal] == DBNull.Value) ? 0 : dr[rateChangeDeltaOrdinal];
                            cmd.Parameters["@DeviceCriticalLowLevel"].Value = (dr[deviceCriticalLowLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceCriticalLowLevelOrdinal];
                            cmd.Parameters["@DeviceLowLevel"].Value = (dr[deviceLowLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceLowLevelOrdinal];
                            cmd.Parameters["@DeviceHighLevel"].Value = (dr[deviceHighLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceHighLevelOrdinal]; ;
                            cmd.Parameters["@DeviceCriticalHighLevel"].Value = (dr[deviceCriticalHighLevelOrdinal] == DBNull.Value) ? 0 : dr[deviceCriticalHighLevelOrdinal];
                            cmd.Parameters["@DeviceFillDetect"].Value = (dr[deviceFillDetectOrdinal] == DBNull.Value) ? 0 : dr[deviceFillDetectOrdinal];
                            cmd.Parameters["@DeviceFillDetectDelta"].Value = (dr[deviceFillDetectDeltaOrdinal] == DBNull.Value) ? 0 : dr[deviceFillDetectDeltaOrdinal];
                            cmd.Parameters["@DeviceFillHysteresis"].Value = (dr[deviceFillHysteresisOrdinal] == DBNull.Value) ? 0 : dr[deviceFillHysteresisOrdinal];
                            cmd.Parameters["@DataLogDelta"].Value = (dr[dataLogDeltaOrdinal] == DBNull.Value) ? 0 : dr["DataLogDelta"];
                            cmd.Parameters["@UsageDelta"].Value = (dr[usageDeltaOrdinal] == DBNull.Value) ? 0 : dr[usageDeltaOrdinal];
                            cmd.Parameters["@WakeInterval"].Value = (dr[wakeIntervalOrdinal] == DBNull.Value) ? 0 : dr[wakeIntervalOrdinal];
                            //cmd.Parameters["@DeviceUsageAlarm"].Value = (dr["DeviceUsageAlarm"] == DBNull.Value) ? 0 : dr["DeviceUsageAlarm"];
                            cmd.Parameters["@HasExpectedCallAlarm"].Value = (dr[hasExpectedCallAlarmOrdinal] == DBNull.Value) ? 0 : dr[hasExpectedCallAlarmOrdinal] ;
                            cmd.Parameters["@TankNormallyFills"].Value = (dr[tankNormallyFillsOrdinal] == DBNull.Value) ? 0 : dr[tankNormallyFillsOrdinal]; ;
                            cmd.Parameters["@EnableGPS"].Value = (dr[enableGPSOrdinal] == DBNull.Value) ? 0 : dr[enableGPSOrdinal]; ;
                            cmd.Parameters["@EnableLocation"].Value = (dr[enableLocationOrdinal] == DBNull.Value) ? 0 : dr[enableLocationOrdinal]; ;

                

                            cmd.ExecuteNonQuery();

                            // JavaScript("window.alert('Hello World');");
                        }
                        con.Close();
                    }
                }
                
            }
            else
            {
                TempData["ButtonValue"] = "Excel File Read error !";
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            ViewBag.Message = "File Imported and excel data saved into database";
            TempData["RowsToProcess"] = rws.ToString();
            TempData["ButtonValue"] = "Excel File Imported !";
          //  return View("BulkConfig");
            return RedirectToAction("BulkConfig");

        }
    }
}
