using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetInvoiceMaster
{
    public int Id { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public DateTime? EstinvoiceDate { get; set; }

    public DateTime ReferenceDate { get; set; }

    public int ClientId { get; set; }

    public string? ShortName { get; set; }

    public string Description { get; set; } = null!;

    public decimal? ProductValue { get; set; }

    public decimal Discount { get; set; }

    public decimal InvoiceValue { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? PrimaryDateTime { get; set; }

    public DateTime? SecondaryDateTime { get; set; }

    public int? BillingCycleId { get; set; }

    public decimal? ArtInvoiceAmount { get; set; }

    public decimal? DigiInvoiceAmount { get; set; }

    public decimal? RoundOff { get; set; }

    public decimal? ArtWaiver { get; set; }

    public decimal? DigiWaiver { get; set; }

    public decimal? Waiver { get; set; }

    public decimal? ArtDiscountAmount { get; set; }

    public decimal? DigiDiscountAmount { get; set; }

    public decimal? ReceivableValue { get; set; }

    public decimal? TotalInvoiceValue { get; set; }

    public int? TransactionId { get; set; }

    public bool? IsTallyIntegrated { get; set; }

    public string? PaymentMode { get; set; }

    public decimal? ArtPayableAmount { get; set; }

    public int? ArtFileCount { get; set; }

    public decimal? DigiPayableAmount { get; set; }

    public int? DigiFileCount { get; set; }
}
