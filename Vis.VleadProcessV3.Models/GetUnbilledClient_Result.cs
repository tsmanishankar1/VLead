using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetUnbilledClient_Result
    {
        public Nullable<int> clientId { get; set; }
        public string? clientName { get; set; }
        public string? department { get; set; }
        public Nullable<int> departmentId { get; set; }
        public string? fileName { get; set; }
        public string? scope { get; set; }
        public string? category { get; set; }
        public Nullable<int> statusId { get; set; }
    }
}
