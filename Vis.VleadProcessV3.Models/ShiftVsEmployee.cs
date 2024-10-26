using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ShiftVsEmployee
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int ShiftId { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Shift Shift { get; set; } = null!;
}
