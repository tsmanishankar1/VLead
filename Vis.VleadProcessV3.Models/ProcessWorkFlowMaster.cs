using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public partial class ProcessWorkFlowMaster
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public int JobId { get; set; }

    public int CustomerId { get; set; }

    public int? ScopeId { get; set; }

    public int? JobCategoryId { get; set; }

    public int DepartmentId { get; set; }

    public long? StitchCount { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<BenchScheduler> BenchSchedulers { get; } = new List<BenchScheduler>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<EmployeeAssign> EmployeeAssigns { get; } = new List<EmployeeAssign>();
    [ForeignKey("JobId")]
    public virtual JobOrder Job { get; set; } = null!;

    public virtual JobCategory? JobCategory { get; set; }

    public virtual ICollection<JobOrderFile> JobOrderFiles { get; } = new List<JobOrderFile>();

    public virtual ICollection<JobQuery> JobQueries { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuotation> JobQuotations { get; } = new List<JobQuotation>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTrans { get; } = new List<ProcessWorkFlowTran>();

    public virtual Scope? Scope { get; set; }

    public virtual TransactionType Transaction { get; set; } = null!;
}
