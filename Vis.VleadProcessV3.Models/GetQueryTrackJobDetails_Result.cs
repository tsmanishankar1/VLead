using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetQueryTrackJobDetails_Result
    {
        public Nullable<System.DateTime> ESTQueryRaisedOn { get; set; }
        public string? Comments { get; set; }
        public string? Name { get; set; }
        public string? FileName { get; set; }
        public string? CustomerJobType { get; set; }
        public string? Description { get; set; }
        public string? JobStatusDescription { get; set; }
    }
}
