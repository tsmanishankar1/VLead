using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class EmployeeCommunication
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public int LocationId { get; set; }

    public string AddressType { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
