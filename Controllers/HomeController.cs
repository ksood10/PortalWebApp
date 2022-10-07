using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PortalWebApp.Data;
using PortalWebApp.Hubs;
using PortalWebApp.Interface;
using PortalWebApp.Models;
using PortalWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static PortalWebApp.Utilities.Util;

namespace PortalWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortalWebAppContext _databaseContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment Environment;
        public bool HaveEXCELReadError = false;
        private BulkConfiguratorQueue myBulkConfigurator;
        BulkUpdate model;

        // private int totalEXCELCount;
        private readonly IHubContext<ProgressHub> _notificationHubContext;
        private readonly IHubContext<ProgressUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        internal string ConnectionString { get; set; }
        internal string ErrorFilePath { get; set; }

        internal string ErrorFileName { get; set; }

        internal string StatusFilePath { get; set; }

        internal string FileName { get; set; }

        public HomeController(IHubContext<ProgressHub> notificationHubContext, 
                                IHubContext<ProgressUserHub> notificationUserHubContext, 
                                IUserConnectionManager userConnectionManager, 
                                ILogger<HomeController> logger, 
                                PortalWebAppContext databaseContext, 
                                IWebHostEnvironment _environment)
        {
            _logger = logger;        
            _databaseContext = databaseContext;           
            Environment = _environment;
            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
           
        }

       [Authorize]
        public IActionResult Index()
        {
           return View();
        }

       

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

        private void ProcessAllEXCELRecords()
        {
            try
            {
                myBulkConfigurator.ApplyTankConfigChanges(_notificationHubContext);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }


        [HttpPost("ImportExcelFile")]
        public IActionResult ImportExcelFile(List<IFormFile> files)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var x = formFile.OpenReadStream();
                    var s = x.Length;
                }
            }
            return View();
        }

        [HttpPost]
        public  IActionResult Index(IFormFile file1, int ThrottleNum, int ThrottleDuration, bool RTU)
        {
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);
            var pathString = MainPath +"\\"+ file1.FileName;
             model = new BulkUpdate();
            if (!System.IO.File.Exists(pathString))
            {
                using FileStream fs = System.IO.File.Create(pathString);
                Stream origFileStream = file1.OpenReadStream();
                origFileStream.CopyToAsync(fs);
            }

            string ret, realConn;
            var userid = Convert.ToInt32(TempData.Peek("Userid"));
            if (TempData.Peek("Environment").ToString() == "ProdString")
                realConn = Env.Prod.Value;
            else
                realConn = Env.Dev.Value;

            myBulkConfigurator = new BulkConfiguratorQueue(realConn, file1.FileName, userid, ThrottleNum, ThrottleDuration, RTU, _notificationHubContext);
            if (myBulkConfigurator.HaveEXCELReadError)
                model.StatusString = "Excel File read error";
            else
            {
                if (!myBulkConfigurator.HaveError)
                {
                    ProcessAllEXCELRecords();
                    model.StatusString = "Bulk Import .. DONE";
                }
                else
                    model.StatusString = "Excel File read error";
            }

            return View("BulkConfig",model);
        }
    }
}

