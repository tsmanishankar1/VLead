using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class IntegrationMaster
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public string? DocumentNumber { get; set; }

    public DateTime? DocumentDate { get; set; }

    public string ClientName { get; set; } = null!;

    public decimal? ProductValue { get; set; }

    public decimal? Roundoff { get; set; }

    public decimal? WaiverAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TotalValue { get; set; }

    public bool? IsCancelled { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsLinked { get; set; }

    public bool? IsFailed { get; set; }

    public string? FailedReason { get; set; }

    public string? DestinationBank { get; set; }

    public string? Mode { get; set; }

    public string? TransactionNumber { get; set; }

    public DateTime? TransactionDate { get; set; }

    public decimal? Amount { get; set; }

    public string? BankName { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public decimal? ExchangeRate { get; set; }

    public string? CurrencyName { get; set; }

    public string? CurrencySymbol { get; set; }

    public virtual ICollection<IntegrationTran> IntegrationTrans { get; } = new List<IntegrationTran>();
}
