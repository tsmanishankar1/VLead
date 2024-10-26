using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class EmployeeHierarchy
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string? EmployeeCode { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int SubEmpId { get; set; }

    public string? SubEmpCode { get; set; }

    public string SubEmpName { get; set; } = null!;

    public int? DesignationId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
