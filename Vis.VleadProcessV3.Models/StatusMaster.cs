using System;
using System.Collections.Generic;

namespace VisProcess.Models;

public partial class StatusMaster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
