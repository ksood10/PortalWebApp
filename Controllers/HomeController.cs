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
        private bool wroteErrorFileHeadings;
        private object checkRTUCondition;
        private string errorFilePath;
        private string errorFileName;
        private string filePath;
        private string fileName;
        private string connectionString;
        private BulkConfiguratorQueue myBulkConfigurator;
        private int totalEXCELCount;
        private bool validationError;

        private readonly IHubContext<ProgressHub> _notificationHubContext;
        private readonly IHubContext<ProgressUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        internal string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
        internal string ErrorFilePath
        {
            get
            {
                return errorFilePath;
            }
            set
            {
                errorFilePath = value;
            }
        }

        internal string ErrorFileName
        {
            get
            {
                return errorFileName;
            }
            set
            {
                errorFileName = value;
            }
        }

        internal string StatusFilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        internal string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

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

        public IActionResult User()
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
                string errorMsg = ex.Message;
            }
        }

        [Route("/Home/ImportExcelFile/{conn}/{userid}/{throttlenum}/{throttleduration}/{rtu}/{filename}")]
        [HttpPost]
        public IActionResult ImportExcelFile(string conn, int userid, int throttlenum, int throttleduration, bool rtu, string filename)
        {
            var realConn = "";
            if (conn == "DevString")                realConn = Env.Dev.Value;
            if (conn == "ProdString")               realConn = Env.Prod.Value;

            myBulkConfigurator = new BulkConfiguratorQueue(realConn, filename, userid, throttlenum, throttleduration, rtu, _notificationHubContext);
            if (myBulkConfigurator.HaveEXCELReadError)
                validationError = true;
            else
            {
                totalEXCELCount = myBulkConfigurator.TotalEXCELCount;
                TempData["Status"] = "Excel file read!";
                if (!myBulkConfigurator.HaveError)
                    ProcessAllEXCELRecords();
                else
                    validationError = true;
            }
            TempData["Status"] = "Excel File Imported !";
            return RedirectToAction("BulkConfig");
        }
    }
}