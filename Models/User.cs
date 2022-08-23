using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebApp.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string AbbreviatedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public int OrganizationID { get; internal set; }
        public bool HasGlobalOrgSecurity { get; set; }
    }
}
