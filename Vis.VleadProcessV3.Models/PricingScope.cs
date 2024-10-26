using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class PricingScope
{
    public int Id { get; set; }

    public int PricingId { get; set; }

    public int ScopeId { get; set; }

    public string? EstimatedTime { get; set; }

    public decimal? RatePerHour { get; set; }

    public decimal? RateperFile { get; set; }

    public int? FromRange { get; set; }

    public int? ToRange { get; set; }

    public decimal? Price { get; set; }

    public decimal? MaxPrice { get; set; }

    public string? ModeOfPricing { get; set; }

    public virtual Pricing Pricing { get; set; } = null!;

    public virtual Scope Scope { get; set; } = null!;
}
