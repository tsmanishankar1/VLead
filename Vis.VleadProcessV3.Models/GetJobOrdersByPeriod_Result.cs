using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class GetJobOrdersByPeriod_Result
    {
        public int Id { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string? CustomerJobType { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public Nullable<System.DateTime> QueryJobDate { get; set; }
        public DateTime JobDate { get; set; }

    }
}
