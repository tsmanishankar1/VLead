using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerExt
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ScopeId { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public bool IsBillable { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? LiveDate { get; set; }

    public int? TrialDays { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
