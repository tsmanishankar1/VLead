using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCustomerDetail
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; } = null!;

    public int CustomerClassificationId { get; set; }

    public short? CreditDays { get; set; }

    public bool IsBlacklisted { get; set; }

    public string? BlacklistedReasons { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int CompanyUniqId { get; set; }

    public string Address1 { get; set; } = null!;

    public string Address2 { get; set; } = null!;

    public string Address3 { get; set; } = null!;

    public int LocationId { get; set; }

    public string? Cstno { get; set; }

    public string? Tinno { get; set; }

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? WebAddress { get; set; }

    public int LocationUniqId { get; set; }

    public string Description { get; set; } = null!;

    public int ClassId { get; set; }

    public string Classification { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? ApprovedBy { get; set; }

    public bool? IsApproved { get; set; }
}
