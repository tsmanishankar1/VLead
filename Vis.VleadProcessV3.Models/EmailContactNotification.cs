using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public partial class EmailContactNotification
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public int CustomerId { get; set; }

    public string ToEmailId { get; set; } = null!;

    public string? CcEmailId { get; set; }

    public bool IsActive { get; set; }

    public bool? IsApproved { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    [JsonIgnore]
    public virtual Customer? Customer { get; set; }
}
public class EmailResult
{
    public bool Success { get; set; }
    public string? Type { get; set; }
    public List<string>? ToEmail { get; set; }
    public List<string>? CcEmail { get; set; }
}
