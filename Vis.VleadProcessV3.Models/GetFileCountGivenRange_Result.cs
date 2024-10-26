using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public class GetFileCountGivenRange_Result
    {
        public int CustomerId { get; set; }
        public string ClientCode { get; set; }
        public string DivisionName { get; set; }
        public string? Scope { get; set; }
        public int Fresh { get; set; }
        public int Revision { get; set; }
    }
}
