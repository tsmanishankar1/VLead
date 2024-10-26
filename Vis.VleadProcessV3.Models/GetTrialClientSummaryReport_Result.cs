using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetTrialClientSummaryReport_Result
    {
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string? Client { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string? Scope { get; set; }
        public string? pricingGiven { get; set; }
        public Nullable<System.DateTime> FileReceivedDate { get; set; }
        public Nullable<int> freshFileCount { get; set; }
        public Nullable<int> revisionFileCount { get; set; }
        public Nullable<int> freshStitchCount { get; set; }
        public Nullable<int> revisionStitchCount { get; set; }
        public Nullable<int> freshFileTimeTaken { get; set; }
        public Nullable<int> revisionFileTimeTaken { get; set; }
    }
}
