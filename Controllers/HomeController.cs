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
                using (var cmd = new SqlCommand("INSERT INTO dbo.TankConfig_Test1 (RTUNumber, StartTime, Interval) VALUES (@RTUNumber, @StartTime,@Interval)",con))
                {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@RTUNumber",SqlDbType.NChar));
                    cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@Interval", SqlDbType.NVarChar));
                    foreach (DataRow dr in dt.Rows)
                    {
                        cmd.Parameters[0].Value= dr["RTUNumber"];
                        cmd.Parameters[1].Value = dr["StartTime"];
                        cmd.Parameters[2].Value = dr["Interval"];
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
