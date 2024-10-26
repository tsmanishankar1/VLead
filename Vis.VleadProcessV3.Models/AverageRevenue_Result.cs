using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class AverageRevenue_Result
    {
        public Nullable<decimal> ArtRevenue { get; set; }
        public Nullable<decimal> DigiRevenue { get; set; }
    }
    [Keyless]
    public partial class TrayWisePendingStatus_Result
    {
        public Nullable<int> Received_Files_Today { get; set; }
        public Nullable<int> Completed_Files_Today { get; set; }
        public Nullable<int> CC { get; set; }
        public Nullable<int> PRO { get; set; }
        public Nullable<int> QUA { get; set; }
        public Nullable<int> PR { get; set; }
        public Nullable<int> BP { get; set; }
        public Nullable<int> SO { get; set; }
        public Nullable<int> Query_Sent { get; set; }
        public Nullable<int> Cancel { get; set; }
    }
    [Keyless]
    public partial class GetDetailedFileCount_Result
    {
        public string? DepartmentName { get; set; }
        public string? CustomerClassificationType { get; set; }
        public string? clientName { get; set; }
        public string? JobStatusDescription { get; set; }
        public int JobstatusId { get; set; }
        public string? CustomerJobType { get; set; }
        public int Received_Files_Till_Date { get; set; }
        public int Outstanding_Pending_Files { get; set; }
        public int Received_Files_Today { get; set; }
        public int Completed_Files_Today { get; set; }
        public int Pending_Files_Today { get; set; }
        public int Query_Sent { get; set; }
        public int Query_Sent_Till_Date { get; set; }
    }
    [Keyless]
    public class DetailedFileCountReport
    {
        public int ClientId { get; set; }
        public DateTime FileReceivedDate { get; set; }
        public int customerClassficationId { get; set; }
        public int jobstatusId { get; set; }
        public int DepartmentId { get; set; }
        public string? LiveTrial { get; set; }
    }
}
