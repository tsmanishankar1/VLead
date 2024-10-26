using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int? CompanyId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsActive { get; set; }
}
