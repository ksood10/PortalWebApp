using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalWebApp.Models
{
    [Table("StrapChart")]
    public class StrapChart
    {
        [Key]
        public int ChartID { get; set; }
        [Display(Name = "Available Charts", Description = "Available Charts")]  
        public string ChartDesc { get; set; }
        public string TankHgt { get; set; }
        public string TankCap { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime Stamp { get; set; }
        public bool Active { get; set; }

        public static List<Strap> StrapList  {get;set;}
        [Display(Name = "Organization", Description = "Organization")]
        public static int UserOrgID { get; internal set; }
    }

    public  class Strap
    {
        public int OrganizationID { get; set; }
        public int ChartID { get; set; }
        public string ChartDesc { get; set; }
        
    }

}
