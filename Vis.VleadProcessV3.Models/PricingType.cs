using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class PricingType
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? Applicable { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<InvoiceTran> InvoiceTrans { get; } = new List<InvoiceTran>();

    public virtual ICollection<PricingWithScope> PricingWithScopes { get; } = new List<PricingWithScope>();

    public virtual ICollection<Pricing> Pricings { get; } = new List<Pricing>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
