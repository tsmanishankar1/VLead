using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;
public partial class CustomerVsManager
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int? ManagerId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Manager { get; set; }
}
