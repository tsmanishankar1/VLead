using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string Address2 { get; set; } = null!;

    public string Address3 { get; set; } = null!;

    public int LocationId { get; set; }

    public string? Cstno { get; set; }

    public string? Tinno { get; set; }

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? WebAddress { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsInvoiceDisplay { get; set; }

    public string? Cinno { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual Location Location { get; set; } = null!;
}
