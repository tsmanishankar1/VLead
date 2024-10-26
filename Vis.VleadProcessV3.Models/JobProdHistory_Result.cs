using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class JobProdHistory_Result
    {
        public string? PreviousProcessName { get; set; }
        public Nullable<int> PreviousProcessId { get; set; }
        public int StatusId { get; set; }
        public Nullable<bool> IsFileCopied { get; set; }
        public string? ProcessName { get; set; }
        public Nullable<System.DateTime> CreatedUTC { get; set; }
        public string? EmployeeName { get; set; }
        public string? WorkingName { get; set; }
        public string? Remarks { get; set; }
        public string? FileUploadPath { get; set; }
        public int TranId { get; set; }
        public int JId { get; set; }
        public int DepartmentId { get; set; }
    }
}
