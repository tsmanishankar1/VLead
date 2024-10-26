using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewDashBoardStatus
{
    public string? DepartmentName { get; set; }

    public string? CustomerClassificationType { get; set; }

    public int? CustomerClassificationId { get; set; }

    public int JobStatusId { get; set; }

    public string? ClientName { get; set; }

    public string? JobStatusDescription { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public int? ClientId { get; set; }

    public DateTime? DateofUpload { get; set; }

    public int DepartmentId { get; set; }

    public int? PreviousProcessId { get; set; }

    public int? ProcessId { get; set; }

    public int StatusId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? EmployeeCount { get; set; }

    public DateTime? Est { get; set; }

    public DateTime? EstqueryDate { get; set; }

    public DateTime? EstjobDate { get; set; }

    public DateTime? EstqueryJobDate { get; set; }

    public bool? Flag { get; set; }

    public DateTime? UpdateJobDate { get; set; }
}
