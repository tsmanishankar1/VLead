using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetConfirmedGeneratedInvoice
{
    public int Id { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal? RoundOff { get; set; }

    public decimal? ProductValue { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public string JobId { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public string? Scope { get; set; }

    public decimal? Rate { get; set; }

    public string? Department { get; set; }

    public long? StitchCount { get; set; }

    public int? ClientId { get; set; }

    public string? ShortName { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Qty { get; set; }

    public decimal? Value { get; set; }

    public DateTime? Est { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public decimal? ArtInvoiceAmount { get; set; }

    public decimal? DigiInvoiceAmount { get; set; }

    public decimal InvoiceValue { get; set; }

    public decimal? Waiver { get; set; }

    public decimal? ArtDiscountAmount { get; set; }

    public decimal? DigiDiscountAmount { get; set; }

    public decimal? ArtWaiver { get; set; }

    public decimal? DigiWaiver { get; set; }

    public decimal Discount { get; set; }

    public decimal? TotalInvoiceValue { get; set; }

    public string? PoNo { get; set; }

    public bool? IsWaiver { get; set; }

    public string? SalesPersonName { get; set; }

    public string? CurrencyMode { get; set; }
}
