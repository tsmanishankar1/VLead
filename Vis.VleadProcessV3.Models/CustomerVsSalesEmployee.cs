using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerVsSalesEmployee
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
