using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class PricingStaffing
{
    public int Id { get; set; }

    public int PricingId { get; set; }

    public int DesignationId { get; set; }

    public decimal RatePerMonth { get; set; }

    public virtual Designation Designation { get; set; } = null!;

    public virtual Pricing Pricing { get; set; } = null!;
}
