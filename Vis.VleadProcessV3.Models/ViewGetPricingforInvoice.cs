using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetPricingforInvoice
{
    public int Id { get; set; }

    public string JobId { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public int? ScopeId { get; set; }

    public int? ClientId { get; set; }

    public long? StitchCount { get; set; }

    public int? EstimatedTime { get; set; }

    public int DepartmentId { get; set; }

    public string? BillingCycleType { get; set; }

    public string? DateofUpload { get; set; }

    public string? Description { get; set; }

    public int Wfmid { get; set; }

    public string Department { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public DateTime FileReceivedDate { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public DateTime? EstdateofUpload { get; set; }

    public decimal? SpecialPrice { get; set; }

    public int Jid { get; set; }
}
