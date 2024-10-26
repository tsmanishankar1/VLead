using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Kraapproval
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? DepartmentId { get; set; }

    public bool? IsApproved { get; set; }

    public int? Krayear { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Employee? Employee { get; set; }
}
