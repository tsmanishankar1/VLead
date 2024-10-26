using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public partial class ReceivableExt
{
    public int Id { get; set; }

    public int ReceivableId { get; set; }

    public string ReceiptMode { get; set; } = null!;

    public string? TransactionNo { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public string BankName { get; set; } = null!;
    [JsonIgnore]
    public virtual Receivable? Receivable { get; set; } = null;
}
