using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class TrialClientMisSummaryReportViewModel
    {
        public int DepartmentId { get; set; }
        public string Client { get; set; }
        public int[] CustomerId { get; set; }
        public string Scope { get; set; }
        public DateTime? FirstDate { get; set; }
        public DateTime? LatestDate { get; set; }
        public int NumberOfDaysInTransition { get; set; }
        public string PricingGiven { get; set; }
        public int TotalFileCountReceivedTillDate { get; set; }
        public int FreshFileCount { get; set; }
        public decimal FreshAvgStitchCount { get; set; }
        public decimal FreshAvgTimeStamp { get; set; }
        public int RevisionFileCount { get; set; }
        public decimal RevisionAvgStitchCount { get; set; }
        public decimal RevisionAvgTimeStamp { get; set; }


    }
}
