using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewEmployeeWithShift
{
    public int EmployeeId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public string EmployeeDesc { get; set; } = null!;

    public string? ShiftName { get; set; }

    public string? ShiftType { get; set; }

    public string? FromTime { get; set; }

    public string? ToTime { get; set; }

    public string? Description { get; set; }

    public int? Id { get; set; }
}
