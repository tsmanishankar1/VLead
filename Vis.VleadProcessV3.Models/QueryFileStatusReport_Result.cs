using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class QueryFileStatusReport_Result
    {
        public string? ClassificationName { get; set; }
        public int ClassificationId { get; set; }
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
        public int JobStatusId { get; set; }
        public string? JobStatusDescription { get; set; }
        public Nullable<int> PreviousProcessId { get; set; }
        public Nullable<int> EmployeeCount { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime JobDate { get; set; }
        public Nullable<System.DateTime> EST { get; set; }
        public string? Name { get; set; }
        public Nullable<System.DateTime> ESTQueryDate { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public Nullable<System.DateTime> ESTQueryJobDate { get; set; }
        public Nullable<bool> Flag { get; set; }
    }

}
