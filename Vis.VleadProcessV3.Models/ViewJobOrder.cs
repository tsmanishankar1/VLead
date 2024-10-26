using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobOrder
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public string JobDescription { get; set; } = null!;

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

    public string Description { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string? FileInwardType { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string? EmployeeName { get; set; }

    public string? EmployeeCode { get; set; }

    public string CustomerJobType { get; set; } = null!;
}
