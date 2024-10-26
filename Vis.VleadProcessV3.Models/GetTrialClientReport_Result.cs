using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetTrialClientReport_Result
    {
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string? shortName { get; set; }
        public string? FileName { get; set; }
        public string? JobStatus { get; set; }
        public int JobStatusId { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string? Status { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
    }
}
