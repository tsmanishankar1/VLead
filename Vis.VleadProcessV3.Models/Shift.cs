using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Shift
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string ShiftType { get; set; } = null!;

    public string FromTime { get; set; } = null!;

    public string ToTime { get; set; } = null!;

    public virtual ICollection<ShiftVsEmployee> ShiftVsEmployees { get; } = new List<ShiftVsEmployee>();
}
