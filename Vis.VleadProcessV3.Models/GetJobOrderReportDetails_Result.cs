using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetJobOrderReportDetails_Result
    {
        public int JId { get; set; }
        public string? ClientJobId { get; set; }
        public string? JobId { get; set; }
        public string? Allcolumn { get; set; }
        public string? ProjectCode { get; set; }
        public int Countcolumn { get; set; }
        public Nullable<System.DateTime> EstDateofUpload { get; set; }
        public System.DateTime EstJobDate { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public string? CustomerJobType { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string? JobStatusDescription { get; set; }
        public int DepartmentId { get; set; }
        public string? FileName { get; set; }
        public string? CustomerName { get; set; }
        public string? Username { get; set; }
        public string? SalesPersonName { get; set; }
        public string? ClientSalesPerson { get; set; }
        public string? DepartmentName { get; set; }
        public string? ShortName { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string? ScopeName { get; set; }
        public string? PoNo { get; set; }
        public decimal? SpecialPrice { get; set; }
    }
}
