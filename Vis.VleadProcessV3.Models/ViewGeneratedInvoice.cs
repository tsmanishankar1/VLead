using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGeneratedInvoice
{
    public string? InvoiceNo { get; set; }

    public decimal? ProductValue { get; set; }

    public int TranId { get; set; }

    public int Id { get; set; }

    public int? BillingCycleId { get; set; }

    public int? EstimatedTime { get; set; }

    public string Description { get; set; } = null!;

    public string? Department { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public long? StitchCount { get; set; }

    public string ShortName { get; set; } = null!;

    public string? ScopeDesc { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal Discount { get; set; }

    public DateTime? EstdateofUpload { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime FileReceivedDate { get; set; }

    public decimal? SpecialPrice { get; set; }

    public int? ScopeId { get; set; }

    public int ClientId { get; set; }

    public decimal InvoiceValue { get; set; }

    public int PricingTypeId { get; set; }

    public int? DepartmentId { get; set; }

    public string JobId { get; set; } = null!;

    public int Jid { get; set; }
}
