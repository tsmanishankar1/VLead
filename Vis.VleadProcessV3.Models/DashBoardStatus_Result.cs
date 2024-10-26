using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class DashBoardStatus_Result
    {
        public string? DepartmentName { get; set; }
        public Nullable<int> Received_Files_Today { get; set; }
        public Nullable<int> Received_Files_Till_Date { get; set; }
        public Nullable<int> Completed_Files_Today { get; set; }
        public Nullable<int> Pending_Files_Today { get; set; }
        public Nullable<int> Completed_Files_Till_Date { get; set; }
        public Nullable<int> Pending_Files_Till_Date { get; set; }
        public Nullable<decimal> AvgRevenue { get; set; }
    }
}
