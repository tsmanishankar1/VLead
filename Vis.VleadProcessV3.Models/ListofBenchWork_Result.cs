using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class ListofBenchWork_Result
    {
        public Nullable<System.DateTime> ESTStartDate { get; set; }
        public Nullable<int> TotalTimeTaken { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string? FileName { get; set; }
        public string? DepartmentName { get; set; }
        public string? JobId { get; set; }
        public string? ShortName { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<int> JId { get; set; }
        public int WFTId { get; set; }
        public string? Name { get; set; }
    }
}
