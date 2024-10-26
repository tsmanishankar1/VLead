using System.ComponentModel.DataAnnotations;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ProcessMovement
    {
        public int Id { get; set; }
        public int? ProcessId { get; set; }
        public int StatusId { get; set; }
        public int? SelectedScopeId { get; set; }
        public bool AutoUploadJobs { get; set; }
        public int EmployeeId { get; set; }
        public string? Remarks { get; set; }
        public bool IsBench { get; set; }
        public string JobId { get; set; }
        public int? Value { get; set; } = null;
        public decimal Amount { get; set; }
        public int? StitchCount { get; set; } = null;
        public int? EstimationTime { get; set; } = null;
        public Nullable<System.DateTime> DateofDelivery { get; set; }
        public string Comments { get; set; }
        public int Validity { get; set; }
        public bool CopyFiles { get; set; }
        public int UpdatedBy { get; set; }
        public int JId { get; set; }
        public int EstimatedTime { get; set; }
        public int TranMasterId { get; set; }
        public IEnumerable<ProcessMovement> SelectedRows { get; set; }
        public IEnumerable<ProcessMovement> SelectedEmployees { get; set; }

        public int DepartmentId { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public string CategoryDesc { get; set; }
        public int AllocatedEstimatedTime { get; set; }
        public int TranId { get; set; }
        public string FileInwardType { get; set; }


        public byte[] TimeStamp { get; set; }
        public int? ScopeId { get; set; } = null;
        public int QuotationRaisedby { get; set; }
        public DateTime QuotationraisedOn { get; set; }
        public int ClientId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime FileReceivedDate { get; set; }
        public string CommentsToClient { get; set; }
        public bool? IsJobFilesNotTransfer { get; set; }//
        public int? FileCount { get; set; }
        public List<string>? EditedFileName { get; set; }
    }
}
