using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetQuoteTrackJobs_Result
    {
        public int JId { get; set; }
        public string? Department { get; set; }
        public Nullable<System.DateTime> ESTFileRecdDate { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public Nullable<System.DateTime> ESTDateOfUpload { get; set; }
        public string? ShortName { get; set; }
        public string? FileName { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string? Remarks { get; set; }
        public Nullable<System.DateTime> ESTApprovedDate { get; set; }
        public string? ApprovedStatus { get; set; }
        public string? artistName { get; set; }
    }
}
