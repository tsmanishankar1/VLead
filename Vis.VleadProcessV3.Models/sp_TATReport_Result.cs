using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class sp_TATReport_Result
    {
        public int JId { get; set; }
        public Nullable<System.DateTime> JobSubmittedDateEST { get; set; }
        public Nullable<System.DateTime> DateofUploadEST { get; set; }
        public string? TATExceed { get; set; }
        public string? TATinHHMM { get; set; }
        public string? JobId { get; set; }
        public string? FileName { get; set; }
        public string? Scope { get; set; }
        public Nullable<long> StitchCount { get; set; }
    }
}
