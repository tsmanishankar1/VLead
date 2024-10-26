using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class CurrentTrayFileStatusReport_Result
    {
        public int Id { get; set; }
        public string? ShortName { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public string? Description { get; set; }
        public string? ClientName { get; set; }
        public string? CustomerJobType { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public string? FileName { get; set; }
        public string? JobId { get; set; }
        public string? JobStatusDescription { get; set; }
        public int EstimatedTime { get; set; }
        public string? AllocatedBy { get; set; }
        public string? AllocatedTo { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> EST { get; set; }
    }
}
