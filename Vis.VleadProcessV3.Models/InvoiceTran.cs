using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class InvoiceTran
{
    public int Id { get; set; }

    public int Imid { get; set; }

    public string? Uom { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Value { get; set; }

    public int PricingTypeId { get; set; }

    public int Createdby { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public string? Description { get; set; }

    public int? ScopeId { get; set; }

    public int? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public int? ScopeGroupId { get; set; }

    public string? ScopeGroupDescription { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public virtual InvoiceMaster Im { get; set; } = null!;

    public virtual ICollection<InvoiceJob> InvoiceJobs { get; } = new List<InvoiceJob>();

    public virtual PricingType PricingType { get; set; } = null!;

    public virtual Scope? Scope { get; set; }
}
