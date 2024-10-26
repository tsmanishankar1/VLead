using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class IntegrationTran
{
    public int Id { get; set; }

    public int IntegrationMasterId { get; set; }

    public string? Scope { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Value { get; set; }

    public string? InvoiceNumber { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public virtual IntegrationMaster IntegrationMaster { get; set; } = null!;
}
