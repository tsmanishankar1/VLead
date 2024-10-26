using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerRegistration
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? EmailId { get; set; }

    public string? PhoneNo { get; set; }

    public int Id { get; set; }

    public bool? Active { get; set; }

    public string? VerifyCode { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Phone1 { get; set; }

    public string? Fax { get; set; }

    public string? Website { get; set; }

    public string? Mobile { get; set; }
}
