using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using PortalWebApp.Data;
using PortalWebApp.Models;
using PortalWebApp.Utilities;

namespace PortalWebApp.Controllers
{
    public class HomeController : Controller
    {
        private PortalWebAppContext _databaseContext;
        private readonly ILogger<HomeController> _logger;
       // private BulkConfiguratorQueue myBulkConfigurator;
        private IWebHostEnvironment Environment;
        private StringValues file_1;

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
           // myBulkConfigurator = new BulkConfiguratorQueue();  string s = myBulkConfigurator.TestDLL(); string connectionString = Properties.Resources.TankDataTestDatabase;
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
        
        //public DataTable GetDataTableFromExcelFile(BulkUpdate model) { 
        //    var filename = Path.GetFileName(model.FileName);
        //    var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

        //    //create directory "Uploads" if it doesn't exists
        //    if (!Directory.Exists(MainPath))
        //        Directory.CreateDirectory(MainPath);
        //    //get file path 
        //     var filePath = Path.Combine(MainPath, filename);
        //     var conString = string.Empty;

        //    switch (Path.GetExtension(filename))
        //    {
        //        case ".xls": //Excel 97-03.
        //            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
        //            break;
        //        case ".xlsx": //Excel 07 and above.
        //            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
        //            break;
        //    }

        //    var dt = new DataTable();
        //    conString = string.Format(conString, filePath);

        //    using (OleDbConnection connExcel = new OleDbConnection(conString))
        //    {
        //        using OleDbCommand cmdExcel = new OleDbCommand();
        //        using OleDbDataAdapter odaExcel = new OleDbDataAdapter();
        //        cmdExcel.Connection = connExcel;

        //        //Get the name of First Sheet.
        //        connExcel.Open();
        //        DataTable dtExcelSchema;
        //        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //        //   string sheetName = "Sheet1$";
        //        connExcel.Close();

        //        //Read Data from First Sheet.
        //        connExcel.Open();
        //        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
        //        odaExcel.SelectCommand = cmdExcel;
        //        odaExcel.Fill(dt);
        //        connExcel.Close();
        //    }
        //    TempData["ButtonValue"] = "Processing ....";
        //    return dt;
        //}

        [HttpPost]
        public IActionResult ImportExcelFile(BulkUpdate model,IFormCollection collection)
        {

             var dt= Util.GetDataTableFromExcelFile(model);

            //your database connection string
            var conString = @"Server=TankdataLSN1\TankData;Database=TankData_TDG;User ID=EmailManager;pwd=tanklink5410;";
            var rws = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (var cmd = new SqlCommand("BulkTankConfig_Insert", con))
                 {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TankID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@RTUNumber", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@TankHgt", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@TankCap", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@CapacityLimit", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@TankMinimum", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@SafetyStockUsage", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@ReorderUsage", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@Callsperday", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@CallDay", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Interval", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@DiagCallDayMask", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@HighSetPoint", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@LowSetPoint", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@SensorOffset", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@CoeffExp", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@SpecGrav", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@LowLowLevel", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@LowLevel", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@HighLevel", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@HighHighLevel", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@ShortFillDelta", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@FillDetectDelta", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@VolumeDelta", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@RateChangeDelta", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@DeviceCriticalLowLevel", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@DeviceLowLevel", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@DeviceHighLevel", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@DeviceCriticalHighLevel", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@DeviceFillDetect", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@DeviceFillDetectDelta", SqlDbType.Decimal));
                    cmd.Parameters.Add(new SqlParameter("@DeviceFillHysteresis", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@DataLogDelta", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@UsageDelta", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@WakeInterval", SqlDbType.Int));
                    //cmd.Parameters.Add(new SqlParameter("@DeviceUsageAlarm ", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@HasExpectedCallAlarm", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@TankNormallyFills", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@EnableGPS", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@EnableLocation", SqlDbType.Bit));

                    var s = collection.Keys;
                    rws = dt.Rows.Count;
                    model.TotalRows = rws; 
                    foreach (DataRow dr in dt.Rows)
                    {
                        cmd.Parameters["@TankID"].Value= dr["TankID"];
                        cmd.Parameters["@RTUNumber"].Value = dr["RTUNumber"];
                        cmd.Parameters["@TankHgt"].Value = (dr["TankHgt"] == DBNull.Value) ? 0 : dr["TankHgt"];
                        cmd.Parameters["@TankCap"].Value = (dr["TankCap"] == DBNull.Value) ? 0 : dr["TankCap"]; 
                        cmd.Parameters["@CapacityLimit"].Value = (dr["CapacityLimit"] == DBNull.Value) ? 0 : dr["CapacityLimit"];
                        cmd.Parameters["@TankMinimum"].Value = (dr["TankMinimum"] == DBNull.Value) ? 0 : dr["TankMinimum"];
                        cmd.Parameters["@ReorderUsage"].Value = (dr["ReorderUsage"] == DBNull.Value) ? 0 : dr["ReorderUsage"]; 
                        cmd.Parameters["@SafetyStockUsage"].Value = (dr["SafetyStockUsage"] == DBNull.Value) ? 0 : dr["SafetyStockUsage"];

                        cmd.Parameters["@Callsperday"].Value = (dr["Callsperday"] == DBNull.Value) ? 0 : dr["Callsperday"];
                        cmd.Parameters["@CallDay"].Value = (dr["CallDay"] == DBNull.Value) ? 0 : dr["CallDay"];
                        cmd.Parameters["@StartTime"].Value = dr["StartTime"];
                                      
                        cmd.Parameters["@Interval"].Value = (dr["Interval"] == DBNull.Value) ? "0" : dr["Interval"];
                        cmd.Parameters["@DiagCallDayMask"].Value = (dr["DiagCallDayMask"] == DBNull.Value) ? 0 : dr["DiagCallDayMask"] ; 
                        cmd.Parameters["@HighSetPoint"].Value = (dr["HighSetPoint"] == DBNull.Value) ? 0 : dr["HighSetPoint"];
                        cmd.Parameters["@LowSetPoint"].Value = (dr["LowSetPoint"] == DBNull.Value) ? 0 : dr["LowSetPoint"];
                        cmd.Parameters["@SensorOffset"].Value = (dr["SensorOffset"] == DBNull.Value) ? 0 : dr["SensorOffset"];
                        cmd.Parameters["@CoeffExp"].Value = (dr["CoeffExp"] == DBNull.Value) ? 0 : dr["CoeffExp"];
                        cmd.Parameters["@SpecGrav"].Value = (dr["SpecGrav"] == DBNull.Value) ? 0 : dr["SpecGrav"];
                        cmd.Parameters["@LowLowLevel"].Value = (dr["LowLowLevel"] == DBNull.Value) ? 0 : dr["LowLowLevel"];
                        cmd.Parameters["@LowLevel"].Value = (dr["LowLevel"] == DBNull.Value) ? 0 : dr["LowLevel"];
                        cmd.Parameters["@HighLevel"].Value = (dr["HighLevel"] == DBNull.Value) ? 0 : dr["HighLevel"];
                        cmd.Parameters["@HighHighLevel"].Value = (dr["HighHighLevel"] == DBNull.Value) ? 0 : dr["HighHighLevel"];
                        cmd.Parameters["@ShortFillDelta"].Value = (dr["ShortFillDelta"] == DBNull.Value) ? 0 : dr["ShortFillDelta"];
                        cmd.Parameters["@FillDetectDelta"].Value = (dr["FillDetectDelta"] == DBNull.Value) ? 0 : dr["FillDetectDelta"];
                        cmd.Parameters["@VolumeDelta"].Value = (dr["VolumeDelta"] == DBNull.Value) ? 0 : dr["VolumeDelta"];
                        cmd.Parameters["@RateChangeDelta"].Value = (dr["RateChangeDelta"] == DBNull.Value) ? 0 : dr["RateChangeDelta"];
                        cmd.Parameters["@DeviceCriticalLowLevel"].Value = (dr["DeviceCriticalLowLevel"] == DBNull.Value) ? 0 : dr["DeviceCriticalLowLevel"]; 
                        cmd.Parameters["@DeviceLowLevel"].Value = (dr["DeviceLowLevel"] == DBNull.Value) ? 0 : dr["DeviceLowLevel"]; 
                        cmd.Parameters["@DeviceHighLevel"].Value = (dr["DeviceHighLevel"] == DBNull.Value) ? 0 : dr["DeviceHighLevel"]; ;
                        cmd.Parameters["@DeviceCriticalHighLevel"].Value = (dr["DeviceCriticalHighLevel"] == DBNull.Value) ? 0 : dr["DeviceCriticalHighLevel"]; 
                        cmd.Parameters["@DeviceFillDetect"].Value = (dr["DeviceFillDetect"] == DBNull.Value) ? 0 : dr["DeviceFillDetect"]; 
                        cmd.Parameters["@DeviceFillDetectDelta"].Value = (dr["DeviceFillDetectDelta"] == DBNull.Value) ? 0 : dr["DeviceFillDetectDelta"]; 
                        cmd.Parameters["@DeviceFillHysteresis"].Value = (dr["DeviceFillHysteresis"] == DBNull.Value) ? 0 : dr["DeviceFillHysteresis"]; 
                        cmd.Parameters["@DataLogDelta"].Value = (dr["DataLogDelta"] == DBNull.Value) ? 0 : dr["DataLogDelta"]; 
                        cmd.Parameters["@UsageDelta"].Value = (dr["UsageDelta"] == DBNull.Value) ? 0 : dr["UsageDelta"]; 
                        cmd.Parameters["@WakeInterval"].Value = (dr["WakeInterval"] == DBNull.Value) ? 0 : dr["WakeInterval"];
                        //cmd.Parameters["@DeviceUsageAlarm"].Value = (dr["DeviceUsageAlarm"] == DBNull.Value) ? 0 : dr["DeviceUsageAlarm"];
                        cmd.Parameters["@HasExpectedCallAlarm"].Value = (dr["HasExpectedCallAlarm"] == DBNull.Value) ? 0 : dr["HasExpectedCallAlarm"]; ;
                        cmd.Parameters["@TankNormallyFills"].Value = (dr["TankNormallyFills"] == DBNull.Value) ? 0 : dr["TankNormallyFills"]; ;
                        cmd.Parameters["@EnableGPS"].Value = (dr["EnableGPS"] == DBNull.Value) ? 0 : dr["EnableGPS"]; ;
                        cmd.Parameters["@EnableLocation"].Value = (dr["EnableLocation"] == DBNull.Value) ? 0 : dr["EnableLocation"]; ;
          
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                       // JavaScript("window.alert('Hello World');");
                    }
                    con.Close();
                }
                //using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con)){ sqlBulkCopy.DestinationTableName = "dbo.TankConfig_Test1";sqlBulkCopy.ColumnMappings.Add("TankID", "TankID");   con.Open(); sqlBulkCopy.WriteToServer(dt);}
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
