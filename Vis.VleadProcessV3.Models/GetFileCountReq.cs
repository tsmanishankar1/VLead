using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public class GetFileCountReq
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int? managerId { get; set; }
        public int[]? CustomerIds { get; set; }
        public string? period { get; set; }
    }
}
