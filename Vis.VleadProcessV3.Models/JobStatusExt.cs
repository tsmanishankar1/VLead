using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class JobStatusExt
{
    public int Id { get; set; }

    public int JobStatusId { get; set; }

    public int CustomerId { get; set; }

    public decimal? Tat { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual JobStatus JobStatus { get; set; } = null!;
}
