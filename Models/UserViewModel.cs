using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using static PortalWebApp.Models.BulkUpdate;

namespace PortalWebApp.Models
{
    public class UserViewModel
    {
        [DisplayName("User")]
        public string UserID { get; set; }
        public int ThrottleNum { get; set; }

        public Env Environment { get; set; }

        public UserViewModel()
        {
            Environment = Env.Dev;
        }

        public List<SelectListItem> ListofUser { get; set; }
    }
}

