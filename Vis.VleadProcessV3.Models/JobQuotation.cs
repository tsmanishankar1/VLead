using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class JobQuotation
{
    public int Id { get; set; }

    public int Wftid { get; set; }

    public int Wfmid { get; set; }

    public string JobId { get; set; } = null!;

    public int? ProcessId { get; set; }

    public int QuotationRaisedby { get; set; }

    public DateTime QuotationraisedOn { get; set; }

    public int StatusId { get; set; }

    public int? EstimationTime { get; set; }

    public int? ScopeId { get; set; }

    public DateTime? DateofDelivery { get; set; }

    public int? StitchCount { get; set; }

    public decimal? Amount { get; set; }

    public string? Comments { get; set; }

    public int? Validity { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Scope? Scope { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual ProcessWorkFlowMaster Wfm { get; set; } = null!;

    public virtual ProcessWorkFlowTran Wft { get; set; } = null!;
}
