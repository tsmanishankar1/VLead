using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TrayDetailMovementTran
{
    public int Id { get; set; }

    public int? Tdmid { get; set; }

    public int? Jid { get; set; }

    public string? JobId { get; set; }

    public DateTime? JobDate { get; set; }

    public string? FileName { get; set; }

    public int? Wfmid { get; set; }

    public int? ClientId { get; set; }

    public string? ClientShortName { get; set; }

    public int? DepartmentId { get; set; }

    public string? Department { get; set; }

    public int? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public int? StatusId { get; set; }

    public string? Process { get; set; }

    public string? Status { get; set; }

    public string? CurrentTray { get; set; }

    public string? CurrentStatus { get; set; }

    public int? EstimatedTime { get; set; }

    public int? WorkedTime { get; set; }

    public int? BreakTime { get; set; }

    public long? StitchCount { get; set; }

    public int? ScopeId { get; set; }

    public string? Scope { get; set; }

    public int? ClassificationId { get; set; }

    public string? Classification { get; set; }

    public string? JobStatus { get; set; }

    public string? CustomerJobType { get; set; }

    public string? CcintakenBy { get; set; }

    public DateTime? CcintakenDate { get; set; }

    public string? UploadedBy { get; set; }

    public DateTime? UploadedUtc { get; set; }

    public string? QueryRaisedBy { get; set; }

    public DateTime? QueryRaisedDate { get; set; }

    public string? QueryResponseBy { get; set; }

    public DateTime? QueryResponseDate { get; set; }

    public string? AllocatedBy { get; set; }

    public DateTime? AllocatedDate { get; set; }

    public int? AllocatedToEmpId { get; set; }

    public string? AllocatedTo { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? CompletedDateTime { get; set; }

    public bool? IsBench { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual Scope? ScopeNavigation { get; set; }

    public virtual TrayDetailMovement? Tdm { get; set; }
}
