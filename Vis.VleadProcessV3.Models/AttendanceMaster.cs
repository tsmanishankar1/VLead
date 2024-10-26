using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class AttendanceMaster
{
    public int AttendanceMasterId { get; set; }

    public DateTime Date { get; set; }

    public bool Active { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }
}
