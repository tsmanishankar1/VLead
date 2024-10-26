using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class OSFileStatusReportModify_Result
    {
        public string? FileName { get; set; }
        public string? JobId { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? ShortName { get; set; }
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string? CustomerJobType { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public Nullable<System.DateTime> EST { get; set; }
    }
}
