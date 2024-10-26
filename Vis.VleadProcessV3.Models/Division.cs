using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Division
{
    public int Id { get; set; }

    public string DivisionName { get; set; } = null!;

    public int TeamLeadEmpId { get; set; }

    public int Capacity { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreateUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdateUtc { get; set; }
    public virtual ICollection<ExternalErrorReport> ExternalErrorReports { get; set; } = new List<ExternalErrorReport>();
    public virtual ICollection<BeanchUpload> BeanchUploads { get; } = new List<BeanchUpload>();
    public virtual ICollection<CustomerVsDivision> CustomerVsDivisions { get; } = new List<CustomerVsDivision>();
    public virtual ICollection<Norm> Norms { get; set; } = new List<Norm>();

    public virtual ICollection<OutsourceCount> OutsourceCounts { get; set; } = new List<OutsourceCount>();

    public virtual ICollection<DailyBench> DailyBenches { get; set; } = new List<DailyBench>();
}
