using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class City
{
    public int Id { get; set; }

    public string? CityName { get; set; }

    public string? CityShortName { get; set; }

    public string? TimeZone { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? CountryId { get; set; }

    public int? StateId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Statee? State { get; set; }
}
