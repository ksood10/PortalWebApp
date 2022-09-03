using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PortalWebApp.Utilities.Util;

namespace PortalWebApp.Models
{
    public class BulkUpdate
    {
        [Display(Name = "Environment")]
        public string Environment { get; set; }

        [Display(Name = "User ID")]
        [Required(ErrorMessage = "Required")]
        public int UserID { get; set; }

        [Display(Name = "Records To Throttle")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public int ThrottleNum { get; set; }

        [Display(Name = "Throttle Duration")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public int ThrottleDuration { get; set; }

        [Display(Name ="Enable RTU Validation")]
        public bool RTU { get; set; }

        public string FileName { get; set; }
        public IFormFile file1 { get; set; }

        public int TotalRows { get; set; }
        public int RowsProcessed { get; set; }


        public string JavascriptToRun { get; set; }

    }
}
