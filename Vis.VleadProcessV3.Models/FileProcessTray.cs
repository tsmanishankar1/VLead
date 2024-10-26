using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class FileProcessTray
{
    public int Id { get; set; }

    public int? Fsid { get; set; }

    public string? Fyfrom { get; set; }

    public string? Fyto { get; set; }

    public int? Value { get; set; }

    public string? Description { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
