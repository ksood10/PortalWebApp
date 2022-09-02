using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebApp.Models
{
    public class BulkUpdate
    {
        public sealed class Env
        {
            public static readonly Env Dev = new Env("Server=TankdataLSN1\\TankData;Database=TankData_TDG;User ID=EmailManager;pwd=tanklink5410");
            public static readonly Env Prod = new Env("Server=Prod");

            private Env(string value)
            {
                Value = value;
            }

            public string Value { get; private set; }
        }

        public string Environment { get; set; }

        public string JavascriptToRun { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public int ThrottleNum { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public int ThrottleDuration { get; set; }

        public bool RTU { get; set; }

        public string FileName { get; set; }
        public IFormFile file1 { get; set; }

        public int TotalRows { get; set; }
        public int RowsProcessed { get; set; }

    }
}
