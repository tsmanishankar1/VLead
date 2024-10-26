using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class JobQuery
{
    public int Id { get; set; }

    public int Wftid { get; set; }

    public int Wfmid { get; set; }

    public int? ProcessId { get; set; }

    public int? QueryRaisedBy { get; set; }

    public DateTime QueryRaisedOn { get; set; }

    public string? Comments { get; set; }

    public int StatusId { get; set; }

    public int? EstimatedTime { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public decimal? SpecialPrice { get; set; }

    public int? ScopeId { get; set; }

    public long? StitchCount { get; set; }

    public string? JobId { get; set; }

    public int? Jid { get; set; }

    public DateTime? DateofDelivery { get; set; }

    public int? Validity { get; set; }

    public string? Status { get; set; }

    public DateTime? QueryDate { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual JobOrder? JidNavigation { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Employee? QueryRaisedByNavigation { get; set; }

    public virtual Scope? Scope { get; set; }

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual Employee? UpdatedByNavigation { get; set; }

    public virtual ProcessWorkFlowMaster Wfm { get; set; } = null!;

    public virtual ProcessWorkFlowTran Wft { get; set; } = null!;
}
 public class Jobqueryfortesting {
    public int Wftid { get; set; }
    public int? Jid { get; set; }
}
