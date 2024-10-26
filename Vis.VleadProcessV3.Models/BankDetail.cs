using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class BankDetail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string? HolderName { get; set; }

    public string Address { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string Ifsccode { get; set; } = null!;

    public string? SwiftCode { get; set; }

    public string? PhoneNo { get; set; }

    public string? PhoneNo1 { get; set; }

    public string? EmailId { get; set; }

    public string? TypeOfAccount { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsInvoiceDisplay { get; set; }

    public string Branch { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
