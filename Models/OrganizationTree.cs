using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebApp.Models
{
    [Table("OrganizationTree")]
    public class OrganizationTree
    {
        [Key]
        public int OrganizationTreeID { get; set; }
        public int OrgID { get; set; }
        public int ChildOrgID { get; set; }
        public int GenLevel { get; set; }
        public int ParentOrgID { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        //public DateTime Stamp { get; set; }

    }
}
