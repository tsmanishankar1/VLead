using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public partial class Process1
{
    public int Id { get; set; }
}
public partial class Process
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<CustomerVsEmployee> CustomerVsEmployeeCurrentProcesses { get; } = new List<CustomerVsEmployee>();

    public virtual ICollection<CustomerVsEmployee> CustomerVsEmployeeNextProcesses { get; } = new List<CustomerVsEmployee>();

    public virtual ICollection<EmployeeAssign> EmployeeAssigns { get; } = new List<EmployeeAssign>();

    public virtual ICollection<EmployeeProcess> EmployeeProcesses { get; } = new List<EmployeeProcess>();

    public virtual ICollection<JobOrderFile> JobOrderFiles { get; } = new List<JobOrderFile>();

    public virtual ICollection<JobQuery> JobQueries { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuotation> JobQuotations { get; } = new List<JobQuotation>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlowCurrentProcesses { get; } = new List<ProcessWorkFlow>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlowNextProcesses { get; } = new List<ProcessWorkFlow>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTranPreviousProcesses { get; } = new List<ProcessWorkFlowTran>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTranProcesses { get; } = new List<ProcessWorkFlowTran>();

    [ForeignKey("UpdatedBy")]
    public virtual Employee? UpdatedByNavigation { get; set; }
}
