using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class TrailJobsCompletedCountbyClientwise_Result
    {
        public Nullable<int> Filescount { get; set; }
        public string CustomerName { get; set; }
        public int ClientId { get; set; }
    }
}
