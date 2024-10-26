using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class AttendanceTransaction
{
    public long AttendanceTransactionId { get; set; }

    public int AttendanceMasterId { get; set; }

    public int Eid { get; set; }

    public string Shift { get; set; } = null!;

    public string Status { get; set; } = null!;
}
