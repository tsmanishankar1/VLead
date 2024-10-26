using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class DPRViewModel
    {
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string TrayDetailName { get; set; }
        public string CustomerType { get; set; }
        public Nullable<int> Classification { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Status { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        //public IEnumerable<DPRViewModel> dprvm { get; set; }
    }
}
