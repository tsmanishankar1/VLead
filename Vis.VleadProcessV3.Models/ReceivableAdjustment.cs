using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;


    public partial class ReceivableAdjustment
{
    public int Id { get; set; }

    public int ReceivableId { get; set; }

    public bool IsInvoiceAdjustment { get; set; }

    public int? InvoiceId { get; set; }

    public string? InvoiceNo { get; set; }

    public decimal? AdjustmentAmount { get; set; }

    public string? ReferenceNo { get; set; }

    public decimal? AvailableAdvance { get; set; }

    public bool? IsAvailableAdvance { get; set; }

    public string? Crdr { get; set; }

    public bool? IsTallyIntegrated { get; set; }

    public virtual InvoiceMaster? Invoice { get; set; }
    [JsonIgnore]
    public virtual Receivable? Receivable { get; set; } = null;
}


public  class ReceivableAdjustment1
{
    public string? InvoiceNo { get; set; }
    
}