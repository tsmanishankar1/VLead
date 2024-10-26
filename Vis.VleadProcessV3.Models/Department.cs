using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<BillingCycleMonthly> BillingCycleMonthlies { get; } = new List<BillingCycleMonthly>();

    public virtual ICollection<Checklist> Checklists { get; } = new List<Checklist>();

    public virtual ICollection<CustomerDepartment> CustomerDepartments { get; } = new List<CustomerDepartment>();

    public virtual ICollection<CustomerVsDivision> CustomerVsDivisions { get; } = new List<CustomerVsDivision>();

    public virtual ICollection<CustomerVsScope> CustomerVsScopes { get; } = new List<CustomerVsScope>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<ErrorCategory> ErrorCategories { get; } = new List<ErrorCategory>();
    public virtual ICollection<Kraapproval> Kraapprovals { get; } = new List<Kraapproval>();

    public virtual ICollection<PricingWithScope> PricingWithScopes { get; } = new List<PricingWithScope>();

    public virtual ICollection<Pricing> Pricings { get; } = new List<Pricing>();

    public virtual ICollection<ProcessWorkFlowMaster> ProcessWorkFlowMasters { get; } = new List<ProcessWorkFlowMaster>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlows { get; } = new List<ProcessWorkFlow>();

    public virtual ICollection<Scope> Scopes { get; } = new List<Scope>();

    public virtual ICollection<VoucherControl> VoucherControls { get; } = new List<VoucherControl>();
}
