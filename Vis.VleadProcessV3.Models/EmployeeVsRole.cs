using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class EmployeeVsRole
{
    public int Erid { get; set; }

    public int EmployeeId { get; set; }

    public string? EmployeeCode { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsDeleted { get; set; }
}
