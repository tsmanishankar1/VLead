using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetClientStatus_Result
    {
        public string? Department { get; set; }
        public string? clientName { get; set; }
        public string? CustomerJobType { get; set; }
        public System.DateTime jobDate { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> fileCount { get; set; }
    }
}
