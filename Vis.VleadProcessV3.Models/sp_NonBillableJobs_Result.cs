namespace Vis.VleadProcessV3.Models
{
    public partial class sp_NonBillableJobs_Result
    {
        public int Id { get; set; }
        public string? JobId { get; set; }
        public System.DateTime JobDate { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public string? FileName { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string? Department { get; set; }
        public string? JobStatus { get; set; }
        public string? Customer { get; set; }
        public string? Scope { get; set; }
        public string? NonBillableStatus { get; set; }
    }
}
