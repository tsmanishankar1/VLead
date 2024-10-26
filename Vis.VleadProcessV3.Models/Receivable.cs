using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public partial class Receivable
{
    public int Id { get; set; }

    public string? VoucherNo { get; set; } = null;

    public DateTime CollectionDate { get; set; }

    public decimal CollectionAmount { get; set; }

    public string? ReferenceNo { get; set; }

    public DateTime ReferenceDate { get; set; }

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int TransactionId { get; set; }

    public int CustomerId { get; set; }

    public int? RemovalReasonId { get; set; }

    public decimal? ExchangeRate { get; set; }

    public string? DestinationBank { get; set; }

    public bool? IsTallyIntegrated { get; set; }
    [JsonIgnore]
    public virtual Customer? Customer { get; set; } = null;

    public virtual ICollection<ReceivableAdjustment> ReceivableAdjustments { get; } = new List<ReceivableAdjustment>();

    public virtual ICollection<ReceivableExt> ReceivableExts { get; } = new List<ReceivableExt>();

    public virtual RemovalReason? RemovalReason { get; set; }

    [JsonIgnore]
    public virtual TransactionType? Transaction { get; set; } = null;
}


public  class Receivable1
{
    public int Id { get; set; }

    public string VoucherNo { get; set; } = null!;

    public DateTime CollectionDate { get; set; }

    public DateTime ReferenceDate { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int TransactionId { get; set; }

}
