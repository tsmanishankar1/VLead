using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class ManagerAssignRequest
    {
        public int managerId { get; set; }
        public List<int> CustomerIds { get; set; }

    }
}
