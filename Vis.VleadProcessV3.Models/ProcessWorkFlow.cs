using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ProcessWorkFlow1
{
    public int DepartmentId { get; set; }
}
    public partial class ProcessWorkFlow
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int? CustomerId { get; set; }

    public int? CurrentProcessId { get; set; }

    public int StatusId { get; set; }

    public int? NextProcessId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? CustomJobType { get; set; }

    public int? ScopeId { get; set; }

    public int? JobStatusId { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Process? CurrentProcess { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Process? NextProcess { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual Employee? UpdatedByNavigation { get; set; }
}
