using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class FileAttachment
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public byte[]? Fileattachment1 { get; set; }
}
