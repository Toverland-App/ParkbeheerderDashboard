using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkbeheerderDashboard.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public int AttractionId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }

    }

}
