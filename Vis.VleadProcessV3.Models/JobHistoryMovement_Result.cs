using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class JobHistoryMovement_Result
    {
        public int TranId { get; set; }
        public string? JobId { get; set; }
        public System.DateTime JobDate { get; set; }
        public Nullable<System.DateTime> JobDate_QueryDate { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public Nullable<System.DateTime> JobDateEST { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public int StatusId { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public string? FileName { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public bool IsDeleted { get; set; }
        public string? Description { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerJobType { get; set; }
        public string? ProjectCode { get; set; }
        public bool IsActive { get; set; }
        public string? JobDescription { get; set; }
        public int TranMasterId { get; set; }
        public int JId { get; set; }
        public string? Status { get; set; }
        public string? ProcessName { get; set; }
        public string? FileInwardType { get; set; }
        public string? CommentsToClient { get; set; }
    }
}
