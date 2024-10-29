using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class GetLeaveRequests
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveCategoryId { get; set; }
        public string LeaveCategory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
    }
}
