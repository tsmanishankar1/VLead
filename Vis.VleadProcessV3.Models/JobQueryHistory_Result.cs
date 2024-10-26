using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class JobQueryHistory_Result
    {
        public string? PreviousProcessName { get; set; }
        public string? JobStatus { get; set; }
        public string? ProcessName { get; set; }
        public Nullable<System.DateTime> CreatedUTC { get; set; }
        public string? EmployeeName { get; set; }
        public string? Comments { get; set; }
    }
}
