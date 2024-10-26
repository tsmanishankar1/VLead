using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewDailyOperationsReport
{
    public string JobId { get; set; } = null!;

    public int Wfmid { get; set; }

    public DateTime? JobDate { get; set; }

    public DateTime? FileReceivedDate { get; set; }

    public string JobStatus { get; set; } = null!;

    public int? ProcessId { get; set; }

    public int DepartmentId { get; set; }

    public string Department { get; set; } = null!;

    public string Client { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FileStatus { get; set; } = null!;

    public string FileInwardType { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string Ccintake { get; set; } = null!;

    public string Ccupload { get; set; } = null!;

    public string FinalStatus { get; set; } = null!;

    public string ProofAccepted { get; set; } = null!;

    public string? QcerrorStatus { get; set; }

    public string? PrerrorStatus { get; set; }

    public int EstimatedTime { get; set; }

    public int? WorkedTime { get; set; }

    public int? BreakTime { get; set; }

    public int? TrainingTime { get; set; }

    public int? OtherTime { get; set; }

    public string Qc { get; set; } = null!;

    public int? QcworkedTime { get; set; }

    public string ProofReader { get; set; } = null!;

    public int? PrworkedTime { get; set; }
}
