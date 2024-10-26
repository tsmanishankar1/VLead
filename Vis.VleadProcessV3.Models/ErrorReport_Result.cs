using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class ErrorReport_Result
    {
        public string? JobId { get; set; }
        public string? FileName { get; set; }
        public Nullable<int> NextProcessId { get; set; }
        public Nullable<int> JId { get; set; }
        public int StatusId { get; set; }
    }
}
