using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetRevisionCount_Result
    {
        public string? Department { get; set; }
        public string? clientName { get; set; }
        public Nullable<int> revisionFiles { get; set; }
        public Nullable<int> freshFiles { get; set; }
    }
}
