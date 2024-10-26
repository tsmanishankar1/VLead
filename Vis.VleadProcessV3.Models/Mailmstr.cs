using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Mailmstr
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? MailType { get; set; }

    public string? Tray { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
