using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class EmployeeResponse
    {
        public int EmployeeId { get; set; }

        public int? ReportLeader1 { get; set; }

        public int? ReportingLeader2 { get; set; }

        public bool IsDeleted { get; set; }

        public int CoreDivision { get; set; }

    }
}