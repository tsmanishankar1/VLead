using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class QueryTrackerSummaryDetails_Result
    {
        public string? ShortName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string? Department { get; set; }
        public Nullable<System.DateTime> FileReceivedDate { get; set; }
        public Nullable<int> queryFiles { get; set; }
        public Nullable<int> querySpFiles { get; set; }
        public Nullable<int> queryResponseFiles { get; set; }
        public Nullable<int> querySpResponseFiles { get; set; }
        public Nullable<int> TotalQueryFiles { get; set; }
        public Nullable<int> TotalFilesResolved { get; set; }
    }
}
