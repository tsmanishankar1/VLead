using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class EmployeePerformanceReport
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string PerformanceComments { get; set; }
        public List<LeaveTrack> SpecialLeaves { get; set; }
    }
}
