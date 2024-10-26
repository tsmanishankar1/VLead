using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class spAttendanceData_Result
    {
        public long Id { get; set; }
        public string? StaffId { get; set; }
        public string? ShiftId { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public double DayAccount { get; set; }
        public string? ShiftType { get; set; }
        public string? ShiftInDateTime { get; set; }
        public string? ShiftOutDateTime { get; set; }
        public string? ActualShiftId { get; set; }
        public DateTime? ActualInDateTime { get; set; }
        public DateTime? ActualOutDateTime { get; set; }
        public string? AttendanceStatus { get; set; }
    }
}
