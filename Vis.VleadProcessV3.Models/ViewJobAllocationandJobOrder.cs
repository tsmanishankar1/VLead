using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobAllocationandJobOrder
{
    public int Id { get; set; }

    public int Wftid { get; set; }

    public DateTime AllocattedDate { get; set; }

    public int EmployeeId { get; set; }

    public int EstimatedTime { get; set; }

    public bool IsActive { get; set; }

    public string Status { get; set; } = null!;

    public int? ProcessId { get; set; }

    public int Wfmid { get; set; }

    public int DepartmentId { get; set; }

    public string JobId { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string? Process { get; set; }
}
