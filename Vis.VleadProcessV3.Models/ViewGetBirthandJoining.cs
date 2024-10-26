using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetBirthandJoining
{
    public int EmployeeId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public string Department { get; set; } = null!;

    public int DesignationId { get; set; }

    public string Designation { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? Dobdate { get; set; }

    public string? Dobmd { get; set; }

    public string? Dobm { get; set; }

    public string? Dobd { get; set; }

    public DateTime DateOfJoining { get; set; }

    public string? Dojdate { get; set; }

    public string? Dojmd { get; set; }

    public string? Dojy { get; set; }

    public string? Dojm { get; set; }

    public string? Dojd { get; set; }
}
