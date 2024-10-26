using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class StaffingPrice
{
    public int Id { get; set; }

    public DateTime BillingFromDate { get; set; }

    public DateTime BillingToDate { get; set; }

    public int NumberofArtist { get; set; }

    public int ClientId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public DateTime? WefromDate { get; set; }

    public DateTime? WetoDate { get; set; }
}
