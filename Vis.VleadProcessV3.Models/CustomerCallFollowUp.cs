using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerCallFollowUp
{
    public int Id { get; set; }

    public string PurposeOfCall { get; set; } = null!;

    public string ModeOfCall { get; set; } = null!;

    public DateTime DateTimeOfCall { get; set; }

    public string? Comments { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsDeleted { get; set; }

    public string? FilePath { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
