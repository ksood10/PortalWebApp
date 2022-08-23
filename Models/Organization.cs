using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebApp.Models
{
    [Table("Organization")]
    public class Organization
    {
        [Key]
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
    }
}
