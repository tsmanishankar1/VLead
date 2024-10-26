using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class State
{
    public int Id { get; set; }

    public string? StateName { get; set; }

    public string? StateShortName { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? CountryId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<City1> City1s { get; } = new List<City1>();

    public virtual Country? Country { get; set; }
}
