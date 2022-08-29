﻿using System;
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

        [HttpPost]
        public ActionResult RunBulkUpdate(BulkUpdate model, List<IFormFile> postedFiles, IFormCollection collection)
        {
            collection.TryGetValue("file1", out file_1);
            if (ModelState.IsValid)
            {
                TempData["ButtonValue"] = string.Format("{0} button clicked. ", model.UserID);
                //TODO: SubscribeUser(model.Email);
            }

            return RedirectToAction("BulkConfig");
        }

        public IActionResult check(IFormFile postedFile, string button, BulkUpdate model)
        {
           // myBulkConfigurator = new BulkConfiguratorQueue();
          //  string s = myBulkConfigurator.TestDLL();
          // string connectionString = Properties.Resources.TankDataTestDatabase;
            var filename = Path.GetFileName(model.FileName);
            if (!string.IsNullOrEmpty(button))
            {
                TempData["ButtonValue"] = string.Format("Env--{0} :::  User -- {1}::: ThrottleNum -- {2}:::Duration-- {3}:::RTU--{4}:::File--{5}", model.Environment, model.UserID, model.ThrottleNum, model.ThrottleDuration, model.RTU, model.FileName);
                //    myBulkConfigurator = new BulkConfiguratorQueue(connectionString, model.FileName, model.UserID,
                //                                                      model.ThrottleNum, model.ThrottleDuration,            //                                                      model.RTU);
                //
            }
            else
            {
                TempData["ButtonValue"] = "No button click!";
            }
            return RedirectToAction("BulkConfig");
        }

        [Authorize]
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
            var filename = Path.GetFileName(model.FileName);
          //  BulkConfiguratorQueue bulkConfiguratorQueue = new BulkConfiguratorQueue(model, _databaseContext);
            //if (myBulkConfigurator.HaveEXCELReadError)
            //{
            //    statusLBL.ForeColor = Color.Red;
            //    statusLBL.Text = "Problem reading the EXCEL sheet. Make sure all columns are present";
            //    validationError = true;
            //}
            //else
            //{
            //    totalEXCELCount = myBulkConfigurator.TotalEXCELCount;
            //    progressBar1.Maximum = totalEXCELCount;
            //    if (!myBulkConfigurator.HaveError)
            //    {
            //        MessageBox.Show("Data Validation Checks Finished.  Now processing updates");
            //        //ProcessAllEXCELRecords();
            //        th1 = new Thread(new ThreadStart(ProcessAllEXCELRecords));
            //        timer1.Enabled = true;
            //        timer1.Start();
            //        th1.Start();
            //    }
            //    else
            //    {
            //        statusLBL.ForeColor = Color.Red;
            //        statusLBL.Text = "Errors found in EXCEL file. Review error report";
            //        validationError = true;
            //    }
            //}
            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            //create directory "Uploads" if it doesn't exists
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);
            //get file path 
            var filePath = Path.Combine(MainPath, filename);
            //using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))            //{            //    await origstream.CopyToAsync(stream);            //}

            string conString = string.Empty;

            switch (Path.GetExtension(filename))
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            DataTable dt = new DataTable();
            conString = string.Format(conString, filePath);

            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using OleDbCommand cmdExcel = new OleDbCommand();
                using OleDbDataAdapter odaExcel = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet.
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
              //   string sheetName = "Sheet1$";
                connExcel.Close();

                //Read Data from First Sheet.
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                odaExcel.SelectCommand = cmdExcel;
                odaExcel.Fill(dt);
                connExcel.Close();
            }
            //your database connection string
            conString = @"Server=TankdataLSN1\TankData;Database=TankData_TDG;User ID=EmailManager;pwd=tanklink5410;";

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (var cmd = new SqlCommand("BulkTankConfig_Insert", con))
                // using (var cmd = new SqlCommand("INSERT INTO dbo.TankConfig_Test1 (RTUNumber, StartTime, Interval) VALUES (@RTUNumber, @StartTime,@Interval)",con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                   //// @TankConfigId Int OUTPUT , 
                   // cmd.Parameters.Add(new SqlParameter("@TankID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@RTUNumber", SqlDbType.NVarChar));
                    //cmd.Parameters.Add(new SqlParameter("@TankName", SqlDbType.NVarChar));
                    //cmd.Parameters.Add(new SqlParameter("@TankHgt", SqlDbType.Decimal));
                    //cmd.Parameters.Add(new SqlParameter("@TankCap", SqlDbType.Decimal));
                    //cmd.Parameters.Add(new SqlParameter("@CapacityLimit", SqlDbType.Decimal));
                    //cmd.Parameters.Add(new SqlParameter("@TankMinimum", SqlDbType.Decimal));
                    //cmd.Parameters.Add(new SqlParameter("@ReorderUsage", SqlDbType.Int));
                    //cmd.Parameters.Add(new SqlParameter("@SafetyStockUsage", SqlDbType.Int));
                    //cmd.Parameters.Add(new SqlParameter("@ReorderUsage", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                    //cmd.Parameters.Add(new SqlParameter("@Callsperday", SqlDbType.Int));
                    //cmd.Parameters.Add(new SqlParameter("@CallDay", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Interval", SqlDbType.NVarChar));
                   // cmd.Parameters.Add(new SqlParameter("@DiagCallDayMask", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@HighSetPoint", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@LowSetPoint", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@SensorOffset", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@CoeffExp", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@SpecGrav", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@LowLowLevel", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@LowLevel", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@HighLevel", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@HighHighLevel", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@ShortFillDelta", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@FillDetectDelta", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@VolumeDelta", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceCriticalLowLevel", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceLowLevel", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceHighLevel", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceCriticalHighLevel", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceFillDetect", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceFillDetectDelta", SqlDbType.Decimal));
                   // cmd.Parameters.Add(new SqlParameter("@DeviceFillHysteresis", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@DataLogDelta", SqlDbType.Int));
                   // cmd.Parameters.Add(new SqlParameter("@UsageDelta", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@WakeInterval", SqlDbType.Int));
                   //// cmd.Parameters.Add(new SqlParameter("@DeviceUsageAlarm ", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@HasExpectedCallAlarm", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@TankNormallyFills", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@EnableGPS", SqlDbType.Bit));
                   // cmd.Parameters.Add(new SqlParameter("@EnableLocation", SqlDbType.Bit));


                    foreach (DataRow dr in dt.Rows)
                    {
                        //cmd.Parameters["@TankID"].Value= dr["TankID"];
                        cmd.Parameters["@RTUNumber"].Value = dr["RTUNumber"];
                        //cmd.Parameters["@TankName"].Value = dr["TankName"];
                        //cmd.Parameters["@TankHgt"].Value = dr["TankHgt"];
                        //cmd.Parameters["@TankCap"].Value = dr["TankCap"];
                        //cmd.Parameters["@CapacityLimit"].Value = dr["CapacityLimit"];
                        //cmd.Parameters["@TankMinimum"].Value = dr["TankMinimum"];
                        //cmd.Parameters["@ReorderUsage"].Value = dr["ReorderUsage"];
                        //cmd.Parameters["@SafetyStockUsage"].Value = dr["SafetyStockUsage"];
                        //cmd.Parameters["@StartTime"].Value = dr["StartTime"];
                        //cmd.Parameters["@Callsperday"].Value = dr["Callsperday"];
                        //cmd.Parameters["@CallDay"].Value = dr["CallDay"];
                        cmd.Parameters["@StartTime"].Value = dr["StartTime"];
                                      
                        cmd.Parameters["@Interval"].Value = dr["Interval"];
                       // cmd.Parameters["@DiagCallDayMask"].Value = dr["DiagCallDayMask"];
                       // cmd.Parameters["@HighSetPoint"].Value = dr["HighSetPoint"];
                       // cmd.Parameters["@LowSetPoint"].Value = dr["LowSetPoint"];
                       // cmd.Parameters["@SensorOffset"].Value = dr["SensorOffset"];
                       // cmd.Parameters["@CoeffExp"].Value = dr["CoeffExp"];
                       // cmd.Parameters["@SpecGrav"].Value = dr["SpecGrav"];
                       // cmd.Parameters["@LowLowLevel"].Value = dr["LowLowLevel"];
                       // cmd.Parameters["@LowLevel"].Value = dr["LowLevel"];
                       // cmd.Parameters["@HighLevel"].Value = dr["HighLevel"];
                       // cmd.Parameters["@HighHighLevel"].Value = dr["HighHighLevel"];
                       // cmd.Parameters["@ShortFillDelta"].Value = dr["ShortFillDelta"];
                       // cmd.Parameters["@FillDetectDelta"].Value = dr["FillDetectDelta"];
                       // cmd.Parameters["@VolumeDelta"].Value = dr["VolumeDelta"];
                       // cmd.Parameters["@DeviceCriticalLowLevel"].Value = dr["DeviceCriticalLowLevel"];
                       // cmd.Parameters["@DeviceLowLevel"].Value = dr["DeviceLowLevel"];
                       // cmd.Parameters["@DeviceHighLevel"].Value = dr["DeviceHighLevel"];
                       // cmd.Parameters["@DeviceCriticalHighLevel"].Value = dr["DeviceCriticalHighLevel"];
                       // cmd.Parameters["@DeviceFillDetect"].Value = dr["DeviceFillDetect"];
                       // cmd.Parameters["@DeviceFillDetectDelta"].Value = dr["DeviceFillDetectDelta"];
                       // cmd.Parameters["@DeviceFillHysteresis"].Value = dr["DeviceFillHysteresis"];
                       // cmd.Parameters["@DataLogDelta"].Value = dr["DataLogDelta"];
                       // cmd.Parameters["@UsageDelta"].Value = dr["UsageDelta"];
                       // cmd.Parameters["@WakeInterval"].Value = dr["WakeInterval"];
                       //// cmd.Parameters["@DeviceUsageAlarm"].Value = dr["DeviceUsageAlarm"];
                       // cmd.Parameters["@HasExpectedCallAlarm"].Value = dr["HasExpectedCallAlarm"];
                       // cmd.Parameters["@TankNormallyFills"].Value = dr["TankNormallyFills"];
                       // cmd.Parameters["@EnableGPS"].Value = dr["EnableGPS"];
                       // cmd.Parameters["@EnableLocation"].Value = dr["EnableLocation"];

                        
                        //cmd.Parameters.AddWithValue("@StartTime", dr["StartTime"]);
                        //cmd.Parameters.AddWithValue("@Interval", dr["Interval"]);                
                        int rowsAffected = cmd.ExecuteNonQuery();
                      //  con.Close();
                    }
                    con.Close();
                }
                //using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                //{
                //    //Set the database table name.
                //    sqlBulkCopy.DestinationTableName = "dbo.TankConfig_Test1";

                //   // sqlBulkCopy.ColumnMappings.Add("TankID", "TankID");
                //   // sqlBulkCopy.ColumnMappings.Add("RTUNumber", "RTUNumber");
                //    sqlBulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                //    sqlBulkCopy.ColumnMappings.Add("Interval", "Interval");

                //    con.Open();
                //    sqlBulkCopy.WriteToServer(dt);
                //    con.Close();
                //}
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            ViewBag.Message = "File Imported and excel data saved into database";
            TempData["ButtonValue"] = "File Imported !";

            return RedirectToAction("BulkConfig");

        }
    }
}
