using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public partial class JobApprovalExt
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public short? CreditDays { get; set; }

    public decimal? CreditLimit { get; set; }

    public string? Remarks { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public string? ApprovalType { get; set; }

    public bool? IsApproved { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsPeriodElapsed { get; set; }

    public virtual Customer? Client { get; set; }
    [ForeignKey("CreatedBy")]
    public virtual Employee Employee1 { get; set; }
}
