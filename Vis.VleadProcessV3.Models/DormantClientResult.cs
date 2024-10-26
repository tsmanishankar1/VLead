using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class DormantClientResult
    {
        public string Result { get; set; }
    }

    public class DormantClient
    {
        public int CustomerId { get; set; } 
        public string CustomerShortName { get; set; }
    }

}