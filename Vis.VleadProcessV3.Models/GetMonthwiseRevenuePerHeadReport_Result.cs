using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetMonthwiseRevenuePerHeadReport_Result
    {
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> headCount { get; set; }
        public Nullable<int> overallHeadCount { get; set; }
        public Nullable<decimal> artRevenue { get; set; }
        public Nullable<decimal> digiRevenue { get; set; }
    }
}
