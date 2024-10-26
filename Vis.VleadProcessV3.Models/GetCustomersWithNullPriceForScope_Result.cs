using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public class GetCustomersWithNullPriceForScope_Result
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ScopeId { get; set; }
        public string ScopeName { get; set; }
        public decimal? Price { get; set; }
    }
}