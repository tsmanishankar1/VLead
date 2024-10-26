using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetEmployeesForPendingJob
{
    public int Id { get; set; }

    public int Wftid { get; set; }

    public int EmployeeId { get; set; }

    public int EstimatedTime { get; set; }

    public bool IsActive { get; set; }

    public string Status { get; set; } = null!;

    public int? DeviationTime { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public int? Wfmid { get; set; }

    public int? Jid { get; set; }

    public int? ProcessId { get; set; }

    public string? Remarks { get; set; }
}
