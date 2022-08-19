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
        public enum Env
        {
            Development, Production
        }

        public Env Environment { get; set; }

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

    }
}
