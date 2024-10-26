using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class AttendanceFileInwardExcel
{
    public int Id { get; set; }

    public string? EmployeeId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? EmployeeName { get; set; }

    public string? EmployeeDepartment { get; set; }

    public string? EmployeeShift { get; set; }

    public string? AttendanceStatus { get; set; }

    public DateTime? AttendanceDate { get; set; }
}
