using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class LeaveTrack
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string LeaveCategory { get; set; } 
        public string Comments { get; set; }
        public string DocumentPath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
