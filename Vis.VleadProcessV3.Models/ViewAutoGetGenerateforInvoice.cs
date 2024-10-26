using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewAutoGetGenerateforInvoice
{
    public int Id { get; set; }

    public int Jid { get; set; }

    public string JobId { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int? ClientId { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public DateTime? EstdateofUpload { get; set; }

    public string FileName { get; set; } = null!;

    public string? DateofUpload { get; set; }

    public decimal? SpecialPrice { get; set; }

    public int? EstimatedTime { get; set; }

    public int? ScopeId { get; set; }

    public long? StitchCount { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? BillingCycleType { get; set; }

    public string? Description { get; set; }
}
