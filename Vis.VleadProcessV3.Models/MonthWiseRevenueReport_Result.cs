using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class MonthWiseRevenueReport_Result
    {
        public Nullable<int> customerId { get; set; }
        public string? shortName { get; set; }
        public Nullable<int> departmentId { get; set; }
        public Nullable<int> billedFileCount { get; set; }
        public Nullable<int> nonBilledFileCount { get; set; }
        public Nullable<decimal> revenue { get; set; }
        public Nullable<int> comparedBilledFileCount { get; set; }
        public Nullable<int> comparedNonBilledFileCount { get; set; }
        public Nullable<decimal> comparedRevenue { get; set; }
    }
}

