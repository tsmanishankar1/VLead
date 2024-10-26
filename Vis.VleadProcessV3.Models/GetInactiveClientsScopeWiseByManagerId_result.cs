using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetInactiveClientsScopeWiseByManagerId_result
    {
        
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ScopeName { get; set; }
        public string SPOCName { get; set; }
        public string SPOCEmail { get; set;}
        public string SPOCPhNo { get; set; }
        public DateTime LastReceivedFileDate { get; set; }

    }
}
