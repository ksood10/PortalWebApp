using System.ComponentModel;

namespace PortalWebApp.Models
{
    public class Article
    {
        [DisplayName("Total Numer of Tasks")]
        public string totalTasks { get; set; }
        public string userId { get; set; }
    }
}
