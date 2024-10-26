using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class WellnessStatus
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? Employeecode { get; set; }

    public string? EmployeeName { get; set; }

    public string? Emobile { get; set; }

    public string? Email { get; set; }

    public string? Stay { get; set; }

    public string? Transport { get; set; }

    public string? Aarogya { get; set; }

    public string? Covid { get; set; }

    public string? Foreigns { get; set; }

    public string? Uforeign { get; set; }

    public string? Contact { get; set; }

    public string? Area { get; set; }

    public string? ContactF { get; set; }

    public string? Symptoms { get; set; }

    public string? Family { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? Monthlystatus { get; set; }
}
