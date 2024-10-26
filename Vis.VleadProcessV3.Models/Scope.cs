using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public class Scope1
{
    public int DepartmentId { get; set; }

    public string Description { get; set; } = null!;

    public int CreatedBy { get; set; }

}
public partial class Scope
{
    public int Id { get; set; }
    
    public int DepartmentId { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? NeedTraining { get; set; }

    public int? ScopeGroupId { get; set; }

    public virtual ICollection<BenchScheduler> BenchSchedulerBenchTypes { get; } = new List<BenchScheduler>();

    public virtual ICollection<BenchScheduler> BenchSchedulerTrainnings { get; } = new List<BenchScheduler>();

    public virtual ICollection<CustomerVsScope> CustomerVsScopes { get; } = new List<CustomerVsScope>();
   
    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<InvoiceTran> InvoiceTrans { get; } = new List<InvoiceTran>();

    public virtual ICollection<JobQuery> JobQueries { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuotation> JobQuotations { get; } = new List<JobQuotation>();

    public virtual ICollection<Norm> Norms { get; set; } = new List<Norm>();

    public virtual ICollection<PricingScope> PricingScopes { get; } = new List<PricingScope>();

    public virtual ICollection<PricingWithScope> PricingWithScopes { get; } = new List<PricingWithScope>();

    public virtual ICollection<ProcessWorkFlowMaster> ProcessWorkFlowMasters { get; } = new List<ProcessWorkFlowMaster>();

    public virtual ICollection<TrayDetailMovementTran> TrayDetailMovementTrans { get; } = new List<TrayDetailMovementTran>();
}
