using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerTat
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int JobStatusId { get; set; }

    public string CustomerShortName { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public decimal? Tat { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }
}
