using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class TotalNofreshfiles_Result
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string? JobId { get; set; }
        public System.DateTime JobDate { get; set; }
        public string? JobDescription { get; set; }
        public int JobStatusId { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string ?ClientJobId { get; set; }
        public Nullable<System.DateTime> ClientJobIddate { get; set; }
        public string? Remarks { get; set; }
        public Nullable<System.DateTime> JobClosedUTC { get; set; }
        public string? PoNo { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public string? FileName { get; set; }
        public Nullable<int> FileattachmentId { get; set; }
        public Nullable<int> FileInwardTypeId { get; set; }
        public string? Username { get; set; }
        public string? SalesPersonName { get; set; }
        public string? CustomerName { get; set; }
        public string? Temp { get; set; }
        public string? Style { get; set; }
        public string? ProjectCode { get; set; }
        public string? TeamCode { get; set; }
        public string? SchoolName { get; set; }
        public string? Color { get; set; }
        public string? Gender { get; set; }
        public string? LogoDimensionWidth { get; set; }
        public string? LogoDimensionsLength { get; set; }
        public string? ApparelLogoLocation { get; set; }
        public string? ImprintColors1 { get; set; }
        public string? ImprintColors2 { get; set; }
        public string? ImprintColors3 { get; set; }
        public string? VirtualProof { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string? JobReferenceId { get; set; }
        public Nullable<int> RemovalReasonsId { get; set; }
        public string? ParentJobId { get; set; }
        public string? BatchNo { get; set; }
        public string? CustomerJobType { get; set; }
        public string? FileUploadPath { get; set; }
        public Nullable<int> ClientOrderId { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public bool ClientRevision { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public Nullable<bool> IsConvertDepartment { get; set; }
        public Nullable<System.DateTime> QueryJobDate { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public Nullable<int> JobCategoryId { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string? CommentsToClient { get; set; }
        public Nullable<int> IsAutoUploadCount { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public Nullable<bool> IsWaiver { get; set; }
        public Nullable<bool> IsBillable { get; set; }
        public Nullable<bool> IsSpecialPrice { get; set; }
        public Nullable<bool> IsQuatation { get; set; }
        public Nullable<int> PricingTypeId { get; set; }
        public string? BatchDate { get; set; }
        public Nullable<int> CCId { get; set; }
        public string? CCEmailId { get; set; }
    }
}
