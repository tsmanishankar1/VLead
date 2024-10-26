using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class QueryTrackerViewModel
    {
        public string Department { get; set; }
        public DateTime? FileReceivedDate { get; set; }
        public DateTime? QuerySentDate { get; set; }
        public string Client { get; set; }
        public string ClientStatus { get; set; }
        public string JobType { get; set; }
        public string FileName { get; set; }
        public string QueryProductionTeamComments { get; set; }
        public string QueryAllocationTeamComments { get; set; }
        public string QueryResolvedComments { get; set; }
        public string QueryRaisedfromProductionTeam { get; set; }
        public string queryRaisedAllocationTeam { get; set; }
        public string Status { get; set; }
        public decimal? SpecialPricinginDollar { get; set; }
        public string QuerySentbyinCCTeam { get; set; }
        public string QueryResolvedDate { get; set; }

    }
}
