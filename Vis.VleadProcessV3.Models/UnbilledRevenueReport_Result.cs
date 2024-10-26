using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class UnbilledRevenueReport_Result
    {
        public Nullable<int> CustomerID { get; set; }
        public string? shortName { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> NonBilledFileCount { get; set; }
        public Nullable<decimal> Revenue { get; set; }
    }
}
