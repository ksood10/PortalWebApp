using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortalWebApp.Data;
using PortalWebApp.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static PortalWebApp.Utilities.Util;

namespace PortalWebApp.Controllers
{
    public class HomeController : Controller
    {
        private PortalWebAppContext _databaseContext;
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment Environment;
        public bool HaveEXCELReadError = false;
        public HomeController(ILogger<HomeController> logger, PortalWebAppContext databaseContext, IWebHostEnvironment _environment)
        {
            _logger = logger;        
            _databaseContext = databaseContext;           
            Environment = _environment;
        }

        [Authorize]
        public IActionResult Index()
        {
           return View();
        }

        public IActionResult check(BulkUpdate model)
        {
            TempData["Status"] = string.Format("Env--{0} :::  User -- {1}::: ThrottleNum -- {2}:::Duration-- {3}:::RTU--{4}:::File--{5}", model.Environment, model.UserID, model.ThrottleNum, model.ThrottleDuration, model.RTU, model.FileName);
            return RedirectToAction("BulkConfig");
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BulkConfig()
        {
            if (TempData.Peek("LoginCheck") != null)
                return View();
            else
                return LocalRedirect("/Home/Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/Home/UploadFiles/{userid}/{throttlenum}/{throttleduration}/{rtu}/{filename}")]
        public string UploadFiles(int userid, int throttlenum, int throttleduration , bool rtu, string filename)
        {
            int x = userid;
            int y = throttlenum;
            int z = throttleduration;
            bool r = rtu;
            string f = filename;
            return "test result :::" +x.ToString() + y.ToString() + z.ToString()+ r.ToString() + f;
        }

        [Route("/Home/ImportExcelFile/{userid}/{throttlenum}/{throttleduration}/{rtu}/{filename}")]
        [HttpPost]
        public IActionResult ImportExcelFile(int userid, int throttlenum, int throttleduration, bool rtu, string filename)
        {
           
            var dt= GetDataTableFromExcelFile(filename); 
            if (dt.Columns.Contains("Error")) HaveEXCELReadError = true;
            if(!HaveEXCELReadError) {
                GetColumnOrdinals(dt);
                try
                {
                    using (var con = new SqlConnection(Env.Dev.Value))
                    {
                        using (var cmd = new SqlCommand(SPBulkInsert, con))
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            foreach (DataRow dr in dt.Rows)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddRange(GetSqlParams(dr));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    TempData["Status"] = "Excel File Imported !";
                }
                catch(Exception e)
                {
                    TempData["Status"] = e.Message;
                }
               
                TempData["RowsToProcess"] = dt.Rows.Count.ToString();
            }
            else     
                TempData["Status"] = "Excel File Read error !";
           
            ViewBag.Message = "File Imported and excel data saved into database"; //if the code reach here means everthing goes fine and excel data is imported into database
            
            return RedirectToAction("BulkConfig");
        }
    }
}