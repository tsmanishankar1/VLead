using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewShiftVsEmployeeShift
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public int? ShiftVsEmployeeId { get; set; }

    public int? Id { get; set; }

    public int? ShiftId { get; set; }

    public string? Description { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }
}
