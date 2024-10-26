using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class FileSummary
{
    public int Id { get; set; }

    public string? Fyfrom { get; set; }

    public string? Fyto { get; set; }

    public string? ClientShortName { get; set; }

    public string? EmployeeName { get; set; }

    public string? Department { get; set; }

    public string? Status { get; set; }

    public int? M01 { get; set; }

    public int? M02 { get; set; }

    public int? M03 { get; set; }

    public int? M04 { get; set; }

    public int? M05 { get; set; }

    public int? M06 { get; set; }

    public int? M07 { get; set; }

    public int? M08 { get; set; }

    public int? M09 { get; set; }

    public int? M10 { get; set; }

    public int? M11 { get; set; }

    public int? M12 { get; set; }

    public string? TrayName { get; set; }

    public int? Total { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
