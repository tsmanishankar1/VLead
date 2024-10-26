using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class News
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Category { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedUtc { get; set; }

    public string? NewImagePath { get; set; }

    public string? Title { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;
}
