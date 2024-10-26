using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class TotalNoSewOutfiles_Result
    {
        public int StatusId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string? JobId { get; set; }
    }
}
