using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public class GetInactiveClients_Result
    {
        public string Name { get; set; }
        public DateTime? LastReceivedFileDate { get; set; }
    }
}