using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Country
{
    public int Id { get; set; }

    public string? CountryName { get; set; }

    public string? CountryShortName { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdateUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<City1> City1s { get; } = new List<City1>();

    public virtual ICollection<Statee> Statees { get; } = new List<Statee>();

    public virtual ICollection<State> States { get; } = new List<State>();
}
