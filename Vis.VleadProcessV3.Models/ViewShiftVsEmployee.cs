using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewShiftVsEmployee
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int ShiftId { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public int MasterShiftId { get; set; }

    public string Description { get; set; } = null!;

    public string ShiftType { get; set; } = null!;

    public string FromTime { get; set; } = null!;

    public string ToTime { get; set; } = null!;

    public DateTime? EffectiveTo { get; set; }
}
