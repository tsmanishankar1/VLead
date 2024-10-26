using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class BenchScheduler
{
    public int Id { get; set; }

    public int Wfmid { get; set; }

    public int Wftid { get; set; }

    public int EmployeeId { get; set; }

    public DateTime FromDateTime { get; set; }

    public DateTime? ToDateTime { get; set; }

    public int ScheduledBy { get; set; }

    public string? Status { get; set; }

    public int? ClientId { get; set; }

    public int? BenchTypeId { get; set; }

    public int? TrainningId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual Scope? BenchType { get; set; }

    public virtual Customer? Client { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee ScheduledByNavigation { get; set; } = null!;

    public virtual Scope? Trainning { get; set; }

    public virtual ProcessWorkFlowMaster Wfm { get; set; } = null!;

    public virtual ProcessWorkFlowTran Wft { get; set; } = null!;
}
