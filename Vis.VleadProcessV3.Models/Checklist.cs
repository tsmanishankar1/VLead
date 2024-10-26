using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Checklist
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? DeptId { get; set; }

    public string? Description { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Department? Dept { get; set; }
}
