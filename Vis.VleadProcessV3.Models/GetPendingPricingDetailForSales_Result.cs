using Microsoft.EntityFrameworkCore;
using System;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public class GetPendingPricingDetailForSales_Result
    {
        public DateTime? DateofUpload { get; set; }
        public DateTime JobDate { get; set; }
        public string JobId { get; set; }
        public string? FileName { get; set; }
        public string? PoNo { get; set; }
        public string? ProjectCode { get; set; }
        public string DepartmentName { get; set; }
        public string CustomerName { get; set; }
        public string? Username { get; set; }
        public string? SalesPersonName { get; set; }
        public string? CustomerJobType { get; set; }
        public string Scope { get; set; }
        public string JobStatusDescription { get; set; }
        public int? EstimatedTime { get; set; }
        public long? StitchCount { get; set; }
    }
}