using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public class JobOrder1
{
    public string? JobId { get; set; } = null!;
    public string? FileName { get; set; } = null!;
    public int? ClientId { get; set; }
    public DateTime? FileReceivedDate { get; set; }

}
public partial class JobOrder2
{
    public int Id { get; set; }
}
    public partial class JobOrder
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public string? JobDescription { get; set; }

    public int JobStatusId { get; set; }

    public int DepartmentId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ClientId { get; set; }

    public string? ClientJobId { get; set; }

    public DateTime? ClientJobIddate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? JobClosedUtc { get; set; }

    public string? PoNo { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public string FileName { get; set; } = null!;

    public int? FileattachmentId { get; set; }

    public int? FileInwardTypeId { get; set; }

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

    public DateTime? DateofUpload { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? JobReferenceId { get; set; }

    public int? RemovalReasonsId { get; set; }

    public string? ParentJobId { get; set; }

    public string? BatchNo { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public string? FileUploadPath { get; set; }

    public int? ClientOrderId { get; set; }

    public DateTime? Podate { get; set; }

    public bool ClientRevision { get; set; }

    public decimal? SpecialPrice { get; set; }

    public bool? IsConvertDepartment { get; set; }

    public DateTime? QueryJobDate { get; set; }

    public int? EstimatedTime { get; set; }

    public int? ScopeId { get; set; }

    public int? JobCategoryId { get; set; }

    public long? StitchCount { get; set; }

    public string? CommentsToClient { get; set; }

    public int? IsAutoUploadCount { get; set; }

    public int? StatusId { get; set; }

    public bool? IsCancelled { get; set; }

    public bool? IsWaiver { get; set; }

    public string? BatchDate { get; set; }

    public bool? IsBillable { get; set; }

    public bool? IsSpecialPrice { get; set; }

    public bool? IsQuatation { get; set; }

    public int? PricingTypeId { get; set; }

    public int? Ccid { get; set; }

    public string? CcemailId { get; set; }

    public DateTime? DateofDownload { get; set; }

    public string? ClientSalesPerson { get; set; }

    public bool? IsMailTriggered { get; set; }

    public DateTime? DateofDelivery { get; set; }

    public int? NonBillableStatusId { get; set; }

    public int? DivisionId { get; set; }

    [ForeignKey("ClientId")]
    public virtual Customer? Customer { get; set; }
    public virtual CustomerContact? Cc { get; set; }
    [ForeignKey("DepartmentId")]
    public virtual Department? Department { get; set; }
    [ForeignKey("EmployeeId")]
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<EmployeeAssign>? EmployeeAssigns { get; } = new List<EmployeeAssign>();

    public virtual ICollection<JobOrderExtension>? JobOrderExtensions { get; } = new List<JobOrderExtension>();

    public virtual ICollection<JobOrderFile>? JobOrderFiles { get; } = new List<JobOrderFile>();

    public virtual ICollection<JobQuery>? JobQueries { get; } = new List<JobQuery>();

    public virtual ICollection<ProcessWorkFlowMaster>? ProcessWorkFlowMasters { get; } = new List<ProcessWorkFlowMaster>();

    public virtual ICollection<ProcessWorkFlowTran>? ProcessWorkFlowTrans { get; } = new List<ProcessWorkFlowTran>();
}
