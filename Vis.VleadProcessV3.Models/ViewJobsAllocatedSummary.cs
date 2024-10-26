using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobsAllocatedSummary
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int? JobLoadInMinutes { get; set; }

    public int? ProcessId { get; set; }
}
