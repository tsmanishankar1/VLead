using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class CancelledJobReport_Result
    {
        public int JId { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
        public string? FileName { get; set; }
        public string? ShortName { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? JobScopeStatus { get; set; }
        public Nullable<System.DateTime> FileReceivedDate { get; set; }
        public string? ActionStatus { get; set; }
        public Nullable<System.DateTime> ActionTakenDate { get; set; }
        public string? ActionDoneBy { get; set; }
        public string? Remarks { get; set; }
        public string? ActionInitiatedTray { get; set; }
        public string? ActionInitiatedBy { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public int WorkedTimeProd { get; set; }
        public int WorkedTimeQC { get; set; }
        public int WorkedTimeProof { get; set; }
        public int WorkedTimeSewout { get; set; }
        public int WorkedTimeBuddy { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
    }
}
