using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Log
{
    public int Id { get; set; }

    public string Module { get; set; } = null!;

    public string? Description { get; set; }

    public int? ClientId { get; set; }

    public int? OrderId { get; set; }

    public int? JobId { get; set; }

    public int? ProcessId { get; set; }

    public int? StatusId { get; set; }

    public string? Type { get; set; }

    public DateTime CreatedUtc { get; set; }
}
