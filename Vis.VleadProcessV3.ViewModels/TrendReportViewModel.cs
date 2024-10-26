using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class TrendReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string DateFormat { get; set; }
        public int DepartmentId { get; set; }
        public int CustomerClassificationId { get; set; }
    }
}
