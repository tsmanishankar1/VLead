using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Pricing
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int PricingTypeId { get; set; }

    public int CustomerId { get; set; }

    public decimal? ThousandStitchCharge { get; set; }

    public decimal? MaxPricingLimit { get; set; }

    public decimal? PriceCharged { get; set; }

    public decimal? MoreStitchCount { get; set; }

    public decimal? FlatRate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<PricingFileRange> PricingFileRanges { get; } = new List<PricingFileRange>();

    public virtual ICollection<PricingScope> PricingScopes { get; } = new List<PricingScope>();

    public virtual ICollection<PricingStaffing> PricingStaffings { get; } = new List<PricingStaffing>();

    public virtual PricingType PricingType { get; set; } = null!;
}
