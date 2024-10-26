using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewMailContact
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public bool? Isdeleted { get; set; }

    public int? DepartmentId { get; set; }

    public int? CustomerId { get; set; }
}
