using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetUser
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string? Roles { get; set; }

    public string? EmployeeName { get; set; }

    public string? ShortName { get; set; }

    public string UserType { get; set; } = null!;

    public string? UserTypeDesc { get; set; }

    public bool IsDeleted { get; set; }

    public int? CustomerId { get; set; }

    public string? Domain { get; set; }

    public string Password { get; set; } = null!;
}
