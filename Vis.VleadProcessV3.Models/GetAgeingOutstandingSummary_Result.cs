using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetAgeingOutstandingSummary_Result
    {
        public Nullable<int> CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public decimal Firstq { get; set; }
        public decimal Second { get; set; }
        public decimal Third { get; set; }
        public decimal Fourth { get; set; }
        public decimal Fifth { get; set; }
        public decimal Total { get; set; }
    }
}
