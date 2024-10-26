using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class MailContact
{
    public int Id { get; set; }

    public string? ProcessName { get; set; }

    public string? Client { get; set; }

    public string? FromMail { get; set; }

    public string? ToMail { get; set; }

    public string? Ccmail { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? DepartmentId { get; set; }

    public int? ClientId { get; set; }
}
