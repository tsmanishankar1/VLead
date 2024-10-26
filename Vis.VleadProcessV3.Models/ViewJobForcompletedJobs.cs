using System.ComponentModel.DataAnnotations;

namespace Vis.VleadProcessV3.Models
{
    public partial class ViewJobForcompletedJobs
    {
        [Key]
        public int TranId { get; set; }
        public int TransactionId { get; set; }
        public string JobId { get; set; }
        public string CustomerType { get; set; }
        public string CustomerClassification { get; set; }
        public System.DateTime JobDate { get; set; }
        public int JobStatusId { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<System.DateTime> JobDate_QueryDate { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public Nullable<System.DateTime> JobDateEST { get; set; }
        public string FileName { get; set; }
        public Nullable<int> FileInwardTypeId { get; set; }
        public string Username { get; set; }
        public string SalesPersonName { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> ClientOrderId { get; set; }
        public Nullable<int> IsAutoUploadCount { get; set; }
        public string Description { get; set; }
        public string JobStatusDescription { get; set; }
        public string FileInwardType { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string CustomerJobType { get; set; }
        public Nullable<int> PreviousProcessId { get; set; }
        public Nullable<System.DateTime> JobClosedUTC { get; set; }
        public bool IsActive { get; set; }
        public string JobDescription { get; set; }
        public string CommentsToClient { get; set; }
        public int ClientId { get; set; }
        public int TranMasterId { get; set; }
        public int JId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public byte[] TimeStamp { get; set; }
        public string FileInwardMode { get; set; }
        public Nullable<int> OverAllTime { get; set; }
        public Nullable<int> TrayTime { get; set; }
        public Nullable<int> Balance_time { get; set; }
        public string scopetype { get; set; }
        public Nullable<int> SalesEmpId { get; set; }
        public string SalesEmpName { get; set; }
        public Nullable<short> CreditDays { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public Nullable<bool> IsManualUpload { get; set; }
        public Nullable<bool> IsJobFilesNotTransfer { get; set; }//
    }
}
