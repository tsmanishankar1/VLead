using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Domain { get; set; }

    public string? Roles { get; set; }

    public string UserType { get; set; } = null!;

    public string? MenuAccess { get; set; }

    public int? EmployeeId { get; set; }

    public int? CustomerId { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }
}
