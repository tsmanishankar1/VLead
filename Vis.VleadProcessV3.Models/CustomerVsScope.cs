using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;
public partial class CustomerVsScope1 {
    public int CustomerId { get; set; } = 0;
    public int DeptId { get; set; }
    public string CustomerJobType { get; set; } = null!;
}

public class CustomerVsScope2
{
    public int Id { get; set; }
    public string CustomerJobType { get; set; } = null!;
    public int? UpdatedBy { get; set; }
}
public partial class CustomerVsScope
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ScopeId { get; set; }

    public int DeptId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string ScopeName { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public string? ScopeGroupDescription { get; set; }

    public int? ScopeGroupId { get; set; }
    public int? EstimatedTime { get; set; }

    public DateTime? EffectiveFromDate { get; set; }

    public DateTime? EffectiveToDate { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    public virtual Department Dept { get; set; } = null!;

    public virtual Scope Scope { get; set; } = null!;
}
