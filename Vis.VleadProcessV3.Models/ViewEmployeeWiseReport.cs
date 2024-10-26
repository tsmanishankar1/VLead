using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewEmployeeWiseReport
{
    public string JobId { get; set; } = null!;

    public DateTime FileReceivedDate { get; set; }

    public string ShortName { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public int? ProcessId { get; set; }

    public int? EstimatedTime { get; set; }

    public int? TotalTimeTaken { get; set; }

    public int DepartmentId { get; set; }

    public string? Status { get; set; }

    public DateTime? Est { get; set; }
}
