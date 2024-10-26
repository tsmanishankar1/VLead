using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetTranMergeInvoiceCancel
{
    public int Id { get; set; }

    public int Imid { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Value { get; set; }

    public int PricingTypeId { get; set; }

    public string PricingType { get; set; } = null!;

    public int? ScopeId { get; set; }

    public string Scope { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public int? ScopeGroupId { get; set; }

    public string? ScopeGroupDescription { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }
}
