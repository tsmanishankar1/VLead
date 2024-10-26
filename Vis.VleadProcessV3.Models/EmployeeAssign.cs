using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;
public class EmployeeAssign1 {

    public int EmployeeId { get; set; }

    public int? BenchTypeId { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }
}

public partial class EmployeeAssign
{
    public int Id { get; set; }

    public int Wftid { get; set; }

    public int? Wfmid { get; set; }

    public int EmployeeId { get; set; }

    public int? ProcessId { get; set; }

    public DateTime AllocattedDate { get; set; }

    public int EstimatedTime { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? TotalTimeTaken { get; set; }

    public int? DeviationTime { get; set; }

    public bool IsActive { get; set; }

    public int? ErrorCategoryId { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? ClientId { get; set; }

    public int? BenchTypeId { get; set; }

    public int? TrainningId { get; set; }

    public int? Jid { get; set; }

    public string? CommentsToClient { get; set; }

    public bool? IsBench { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ErrorCategory? ErrorCategory { get; set; }

    [ForeignKey("Jid")]
    public virtual JobOrder? JobOrder { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }

    public virtual ProcessWorkFlowMaster? Wfm { get; set; }

    public virtual ProcessWorkFlowTran Wft { get; set; } = null!;
}
