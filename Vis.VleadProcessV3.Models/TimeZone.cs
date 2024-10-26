using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TimeZone
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Istdiff { get; set; } = null!;

    public string TimezoneDiff { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}
