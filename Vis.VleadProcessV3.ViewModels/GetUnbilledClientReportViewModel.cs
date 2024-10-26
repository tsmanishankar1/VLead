using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class GetUnbilledClientReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public string category { get; set; }
        public string Department { get; set; }
        public int statusId { get; set; }
    }
}
