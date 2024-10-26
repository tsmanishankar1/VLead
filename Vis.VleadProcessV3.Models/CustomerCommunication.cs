using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerCommunication
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string EstablishmentType { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public int LocationId { get; set; }

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? WebAddress { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
