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
            if (dt == null) HaveEXCELReadError = true;
            if(!HaveEXCELReadError) {
                GetColumnOrdinals(dt);
                using (var con = new SqlConnection(model.Environment))
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
            }
            else
            {
                TempData["ButtonValue"] = "Excel File Read error !";
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            ViewBag.Message = "File Imported and excel data saved into database";
            TempData["RowsToProcess"] = dt.Rows.Count.ToString();
            TempData["ButtonValue"] = "Excel File Imported !";
          //  return View("BulkConfig");
            return RedirectToAction("BulkConfig");

        }
    }
}
