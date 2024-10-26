using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class UnbilledRevenueReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public string department { get; set; }
        public int?[] ScopeId { get; set; }
    }
}
