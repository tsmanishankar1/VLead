using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCancelledJob
{
    public int Jid { get; set; }

    public DateTime? JobDate { get; set; }

    public string FileName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string JobScopeStatus { get; set; } = null!;

    public DateTime? FileReceivedDate { get; set; }

    public string ActionStatus { get; set; } = null!;

    public DateTime? ActionTakenDate { get; set; }

    public string? ActionDoneBy { get; set; }

    public string? Remarks { get; set; }

    public string ActionInitiatedTray { get; set; } = null!;

    public string ActionInitiatedBy { get; set; } = null!;

    public int? EstimatedTime { get; set; }

    public int WorkedTimeProd { get; set; }

    public int WorkedTimeQc { get; set; }

    public int WorkedTimeProof { get; set; }

    public int WorkedTimeSewout { get; set; }

    public int WorkedTimeBuddy { get; set; }

    public string? Scope { get; set; }

    public int? ClientId { get; set; }

    public int DepartmentId { get; set; }
}
