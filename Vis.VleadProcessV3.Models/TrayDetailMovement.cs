using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TrayDetailMovement
{
    public int Id { get; set; }

    public DateTime? JobDate { get; set; }

    public string? FileName { get; set; }

    public int? ClientId { get; set; }

    public string? ClientShortName { get; set; }

    public string? Department { get; set; }

    public string? JobStatus { get; set; }

    public string? CustomerJobType { get; set; }

    public string? Scope { get; set; }

    public int? StitchCount { get; set; }

    public string? CurrentTray { get; set; }

    public string? CurrentStatus { get; set; }

    public string? CcintakenBy { get; set; }

    public string? ProductionAllocatedBy { get; set; }

    public int? ProductionAllocatedToEmpId { get; set; }

    public string? ProductionAllocatedTo { get; set; }

    public int? EstimationTime { get; set; }

    public int? ProductionWorkedTime { get; set; }

    public int? ProductionBreakTime { get; set; }

    public DateTime? ProductionCompletedDateTime { get; set; }

    public string? QualityAllocatedBy { get; set; }

    public int? QualityAllocatedToEmpId { get; set; }

    public string? QualityAllocatedTo { get; set; }

    public int? QualityWorkedTime { get; set; }

    public int? QualityBreakTime { get; set; }

    public DateTime? QualityCompletedDateTime { get; set; }

    public string? SoallocatedBy { get; set; }

    public int? SoallocatedToEmpId { get; set; }

    public string? SoallocatedTo { get; set; }

    public int? SoworkedTime { get; set; }

    public DateTime? SocompletedDateTime { get; set; }

    public string? BpallocatedBy { get; set; }

    public int? BpallocatedToEmpId { get; set; }

    public string? BpallocatedTo { get; set; }

    public int? BpworkedTime { get; set; }

    public DateTime? BpcompletedDateTime { get; set; }

    public string? ProofReadingAllocatedBy { get; set; }

    public int? ProofReadingAllocatedToEmpId { get; set; }

    public string? ProofReadingAllocatedTo { get; set; }

    public int? ProofreadingWorkedTime { get; set; }

    public DateTime? ProofReadingCompletedDateTime { get; set; }

    public string? UploadedBy { get; set; }

    public DateTime? UploadedUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? Jid { get; set; }

    public string? JobId { get; set; }

    public int? DepartmentId { get; set; }

    public int? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public int? StatusId { get; set; }

    public int? ClassificationId { get; set; }

    public string? Classification { get; set; }

    public DateTime? CcintakenDate { get; set; }

    public DateTime? ProductionAllocatedDate { get; set; }

    public DateTime? ProductionStartDate { get; set; }

    public DateTime? QualityAllocatedDate { get; set; }

    public DateTime? QualityStartDate { get; set; }

    public DateTime? SoallocatedDate { get; set; }

    public DateTime? SostartDate { get; set; }

    public DateTime? BpallocatedDate { get; set; }

    public DateTime? BpstartDate { get; set; }

    public DateTime? ProofReadingAllocatedDate { get; set; }

    public DateTime? ProofReadingStartDate { get; set; }

    public string? QueryRaisedBy { get; set; }

    public DateTime? QueryRaisedDate { get; set; }

    public string? QueryResponseBy { get; set; }

    public DateTime? QueryResponseDate { get; set; }

    public virtual ICollection<TrayDetailMovementTran> TrayDetailMovementTrans { get; } = new List<TrayDetailMovementTran>();
}
