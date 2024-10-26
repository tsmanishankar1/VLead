using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetCustomerforSelection
{
    public int Id { get; set; }

    public string? ShortName { get; set; }

    public string Name { get; set; } = null!;
}
