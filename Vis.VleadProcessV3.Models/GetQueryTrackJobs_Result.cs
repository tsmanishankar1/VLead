using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class GetQueryTrackJobs_Result
    {
        public int Id { get; set; }
        public Nullable<int> JId { get; set; }
        public string ShortName { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
    }
}
