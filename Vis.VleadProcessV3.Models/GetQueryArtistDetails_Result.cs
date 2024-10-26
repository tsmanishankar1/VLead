using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetQueryArtistDetails_Result
    {
        public Nullable<System.DateTime> ESTQueryRaisedOn { get; set; }
        public string? Comments { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public int StatusId { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public string? EmployeeName { get; set; }
    }
}
