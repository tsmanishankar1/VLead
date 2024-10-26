using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class PricingWithScope
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int DepartmentId { get; set; }

    public int? ScopeId { get; set; }

    public int? JobStatusId { get; set; }

    public int PricingTypeId { get; set; }

    public int? From { get; set; }

    public int? To { get; set; }

    public decimal? Price { get; set; }

    public decimal? RatePerHour { get; set; }

    public string? EstimationTime { get; set; }

    public string? ScopeTempDesc { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Designation { get; set; }

    public int? NumberofArtist { get; set; }

    public DateTime? WefromDate { get; set; }

    public DateTime? WetoDate { get; set; }

    public bool? IsUpdated { get; set; }

    public bool? GenerateInvoice { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public bool? IsRejected { get; set; }

    public int? RejectedBy { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual PricingType PricingType { get; set; } = null!;

    public virtual Scope? Scope { get; set; }
}
