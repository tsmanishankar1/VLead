using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public long LocationCode { get; set; }

    public int? ContraLocationId { get; set; }

    public string? LocationHeaderDescription { get; set; }

    public string? Zipcode { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? TimeZoneId { get; set; }

    public string? TimezoneDescription { get; set; }

    public string? TimezoneDifference { get; set; }

    public string? DayLightTimezoneDifference { get; set; }

    public virtual ICollection<Company> Companies { get; } = new List<Company>();

    public virtual ICollection<CustomerCommunication> CustomerCommunications { get; } = new List<CustomerCommunication>();
}
