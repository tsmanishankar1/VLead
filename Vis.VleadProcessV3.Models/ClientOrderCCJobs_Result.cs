using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class ClientOrderCCJobs_Result
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string? FileName { get; set; }
        public string? CustomerType { get; set; }
        public string? CustomerClassification { get; set; }
        public string? PONo { get; set; }
        public string? Instruction { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public string? SalesPersonName { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> RemovalReasonId { get; set; }
        public int WorkType { get; set; }
        public int JobStatusId { get; set; }
        public string? ParentJobId { get; set; }
        public string? JobReferenceId { get; set; }
        public string? ReceiptMode { get; set; }
        public string? FileUploadPath { get; set; }
        public bool IsCompleted { get; set; }
        public string? Suggestion { get; set; }
        public Nullable<int> QuotationStatus { get; set; }
        public string? ClientName { get; set; }
        public string? TransactionType { get; set; }
        public string? DepartmentName { get; set; }
        public Nullable<bool> IsConverted { get; set; }
        public Nullable<int> CCId { get; set; }
        public string? CCEmailId { get; set; }
        public int FileInwardTypeId { get; set; }
    }
}
