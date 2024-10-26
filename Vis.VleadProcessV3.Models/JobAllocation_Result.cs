using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class JobAllocation_Result
    {
        
        public string JobId { get; set; }
        public System.DateTime JobDate { get; set; }
        public int? JobStatusId { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? FileName { get; set; }
        public string? ShortName { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerJobType { get; set; }
        public string? CustomerClassification { get; set; }
        public string? ProjectCode { get; set; }
        public string? CategoryDesc { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> DivEmpId { get; set; }
        public int TranId { get; set; }
        public int TranMasterId { get; set; }
        public int JId { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string? ScopeDesc { get; set; }
        public string? EmployeeName { get; set; }
        public Nullable<int> EmployeeCount { get; set; }

       
        public byte[]? TimeStamp { get; set; }
        public Nullable<System.DateTime> QueryJobDate { get; set; }
        public Nullable<int> AllocatedEstimatedTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> JobDate_QueryDate { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public Nullable<System.DateTime> JobDateEST { get; set; }
        public Nullable<int> PreviousProcessId { get; set; }
        public string? Name { get; set; }
        public string? ArtistName { get; set; }
        public string? FileInwardType { get; set; }
        public Nullable<bool> IsBulk { get; set; }
        public Nullable<System.DateTime> DateofDelivery { get; set; }
        public Nullable<int> OverAllTime { get; set; }
        public Nullable<int> TrayTime { get; set; }
        public Nullable<int> Balance_time { get; set; }
    }
}
