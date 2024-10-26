using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ClientwiseFileCountThresholdReportViewModel
    {
        public string Department { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FileType { get; set; }
        public string ClientType { get; set; }
        public string Scope { get; set; }
        public IList<int> Months { get; set; }
        public DateTime? LastReceivedDate { get; set; }
    }
}
