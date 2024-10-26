using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vis.VleadProcessV3.Models;

public partial class ProcessWorkFlowTran
{
    public int Id { get; set; }

    public int Wfmid { get; set; }

    public int DepartmentId { get; set; }

    public int? PreviousProcessId { get; set; }

    public int? ProcessId { get; set; } = null;

    public int JobStatusId { get; set; }

    public int? AllocattedById { get; set; }

    public int? EstimatedTime { get; set; }

    public int? DeviationTime { get; set; }

    public int StatusId { get; set; }

    public int? ErrorCategoryId { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? WorkflowTypeDescription { get; set; }

    public string? FileUploadPath { get; set; }

    
    public byte[] TimeStamp { get; set; } = null!;

    public string? EmployeeName { get; set; }

    public int? EmployeeCount { get; set; }

    public int? AllocatedEstimatedTime { get; set; }

    public bool IsFileCopied { get; set; }

    public string? ArtistName { get; set; }

    public string? ErrorStatus { get; set; }

    public int? Jid { get; set; }

    public virtual ICollection<BenchScheduler> BenchSchedulers { get; } = new List<BenchScheduler>();

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<EmployeeAssign> EmployeeAssigns { get; } = new List<EmployeeAssign>();

    public virtual JobOrder? JidNavigation { get; set; }

    public virtual ICollection<JobOrderExtension> JobOrderExtensions { get; } = new List<JobOrderExtension>();

    public virtual ICollection<JobOrderFile> JobOrderFiles { get; } = new List<JobOrderFile>();

    public virtual ICollection<JobQuery> JobQueries { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuotation> JobQuotations { get; } = new List<JobQuotation>();

    public virtual JobStatus JobStatus { get; set; } = null!;

    public virtual Process? PreviousProcess { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual Employee? UpdatedByNavigation { get; set; }

    public virtual ProcessWorkFlowMaster Wfm { get; set; } = null!;
}
