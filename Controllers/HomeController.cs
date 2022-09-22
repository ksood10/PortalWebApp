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
        private readonly PortalWebAppContext _databaseContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment Environment;
        public bool HaveEXCELReadError = false;
        private BulkConfiguratorQueue myBulkConfigurator;
     

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

        [Route("/Home/ImportExcelFile/{conn}/{userid}/{throttlenum}/{throttleduration}/{rtu}/{filename}")]
        [HttpPost]
        public string ImportExcelFile(string conn, int userid, int throttlenum, int throttleduration, bool rtu, string filename)
        {

            var realConn = ""; string ret = "";
            if (conn == "DevString")                realConn = Env.Dev.Value;
            if (conn == "ProdString")               realConn = Env.Prod.Value;

            myBulkConfigurator = new BulkConfiguratorQueue(realConn, filename, userid, throttlenum, throttleduration, rtu, _notificationHubContext);
            if (myBulkConfigurator.HaveEXCELReadError)
                ret = "Excel File read error";
            else
            {
                // totalEXCELCount = myBulkConfigurator.TotalEXCELCount;
                BulkUpdate.StatusString =  "Excel file read!";
                if (!myBulkConfigurator.HaveError)
                {
                    ProcessAllEXCELRecords();
                    ret =  "Excel File Imported !";
                }
                else
                    ret =  "Excel File read error";
            }

            return ret;
        }
    }
}