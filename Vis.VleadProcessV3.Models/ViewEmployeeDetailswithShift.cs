using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewEmployeeDetailswithShift
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string DepartmentDesc { get; set; } = null!;

    public string DesignationDesc { get; set; } = null!;

    public string ProfiencyDesc { get; set; } = null!;

    public int? ReportingManager1 { get; set; }

    public string ManagerName1 { get; set; } = null!;

    public int? ReportLeader1 { get; set; }

    public string LeaderName1 { get; set; } = null!;

    public int? ReportingManager2 { get; set; }

    public string? ManagerName2 { get; set; }

    public int? ReportingLeader2 { get; set; }

    public string? LeaderName2 { get; set; }

    public bool IsDeleted { get; set; }

    public int? ResignReasons { get; set; }

    public int? Id { get; set; }

    public string? Description { get; set; }

    public string? ShiftType { get; set; }

    public string? FromTime { get; set; }

    public string? ToTime { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public DateTime? EffectiveFrom { get; set; }
}
