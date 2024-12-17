using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public partial class AttendanceSheet
{
    public int Id { get; set; }
    public string StaffId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string ShiftTime { get; set; } = null!;

    public string Date { get; set; } = null!;

    public string? InTime { get; set; }

    public string? OutTime { get; set; }

    public string? TotalHoursWorked { get; set; }

    public string BreakHours { get; set; } = null!;

    public bool IsBreakExceeded { get; set; }

    public string ProductiveHours { get; set; } = null!;

    public string AttendanceStatus { get; set; } = null!;

    public string EarlyEntry { get; set; } = null!;

    public string LateEntry { get; set; } = null!;

    public string EarlyExit { get; set; } = null!;

    public string ExtraHoursWorked { get; set; } = null!;

    public decimal? Absent { get; set; }

    public decimal? Present { get; set; }

    public decimal? WeeklyHoliday { get; set; }

    public decimal Total { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }
}