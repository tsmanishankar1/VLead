using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewLogtext
{
    public int Id { get; set; }

    public string Module { get; set; } = null!;

    public string? Description { get; set; }

    public int? ClientId { get; set; }

    public int? OrderId { get; set; }

    public int? JobId { get; set; }
}
