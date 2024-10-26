using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ErrorCategory
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<EmployeeAssign> EmployeeAssigns { get; } = new List<EmployeeAssign>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
