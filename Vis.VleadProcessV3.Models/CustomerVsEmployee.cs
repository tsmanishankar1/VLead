using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerVsEmployee1
{
    public int Id { get; set; }
}
public partial class CustomerVsEmployee
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public int CurrentProcessId { get; set; }

    public int NextProcessId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }


    public bool? IsAgmapproved { get; set; }

    public int? RejectedBy { get; set; }

    public DateTime? RejectedUtc { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedUtc { get; set; }

    public bool? IsAgmrejected { get; set; }


    public virtual Process CurrentProcess { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Process NextProcess { get; set; } = null!;
}
