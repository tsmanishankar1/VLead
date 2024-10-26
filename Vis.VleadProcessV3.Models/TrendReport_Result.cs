using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class TrendReport_Result
    {
        public Nullable<int> departmentId { get; set; }
        public Nullable<int> dayFileCount { get; set; }
        public Nullable<int> tillDateFileCount { get; set; }
        public Nullable<decimal> dayArtRevenue { get; set; }
        public Nullable<decimal> dayDigiRevenue { get; set; }
        public Nullable<decimal> tillDateArtRevenue { get; set; }
        public Nullable<decimal> tillDateDigiRevenue { get; set; }
    }
    [Keyless]
    public partial class TrendDetail_Result
    {
        public Nullable<int> customerID { get; set; }
        public string? shortName { get; set; }
        public Nullable<int> CustomerClassificationId { get; set; }
        public Nullable<int> departmentId { get; set; }
        public Nullable<int> dayFileCount { get; set; }
        public Nullable<int> tillDateFileCount { get; set; }
        public Nullable<decimal> dayArtRevenue { get; set; }
        public Nullable<decimal> dayDigiRevenue { get; set; }
        public Nullable<decimal> tillDateArtRevenue { get; set; }
        public Nullable<decimal> tillDateDigiRevenue { get; set; }
    }
}
