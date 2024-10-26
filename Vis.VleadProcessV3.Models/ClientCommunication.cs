using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;


public partial class ClientCommunication
{
    [JsonIgnore]
    public int Id { get; set; }

    public int JobOrderId { get; set; }

    public string DelayType { get; set; } = null!;

    public string Reason { get; set; } = null!;
    [JsonIgnore]
    public string? EmailId { get; set; }
    [JsonIgnore]
    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }
    [JsonIgnore]
    public DateTime? UpdatedUtc { get; set; }
    [JsonIgnore]
    public bool? Agmapproved { get; set; }
    [JsonIgnore]
    public bool? Agmrejected { get; set; }

    public int? RejectedUpdatedBy { get; set; }
    [JsonIgnore]
    public DateTime? RejectedUpdatedUtc { get; set; }
    [JsonIgnore]
    public virtual JobOrder JobOrder { get; set; } = null!;
}
