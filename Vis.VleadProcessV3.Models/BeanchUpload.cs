using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;
public partial class BeanchUpload
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int DivisionId { get; set; }

    public int ShiftId { get; set; }

    public int? Fresh { get; set; }

    public int? Revision { get; set; }

    public int? Qc { get; set; }

    public int? AchievedCount { get; set; }

    public int? ProductionHeadCount { get; set; }

    public int? OtherTeamBenchDeployed { get; set; }

    public string? DeployedDivisionName { get; set; }

    public int? InternalOrCrossTrainingHeadCount { get; set; }

    public string? CommentsForInternalTraining { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateUtc { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
