using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetEmployeeDetailsReport_Result
    {
        public Nullable<int> ClientId { get; set; }
        public string? ClientShortName { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Status { get; set; }
        public Nullable<int> FilesAllocatedInProd { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<int> ProdWorkedTime { get; set; }
        public Nullable<int> ProdBreakTime { get; set; }
        public Nullable<int> FilesCompInProd { get; set; }
        public Nullable<int> FilesAllocatedInQuality { get; set; }
        public Nullable<int> QualityWorkedTime { get; set; }
        public Nullable<int> FilesCompInQuality { get; set; }
        public Nullable<int> FilesAllocatedInBP { get; set; }
        public Nullable<int> BPWorkedTime { get; set; }
        public Nullable<int> FilesCompInBP { get; set; }
        public Nullable<int> FilesAllocatedInSO { get; set; }
        public Nullable<int> SOWorkedTime { get; set; }
        public Nullable<int> FilesCompInSO { get; set; }
        public Nullable<int> FilesAllocatedInPR { get; set; }
        public Nullable<int> PRWorkedTime { get; set; }
        public Nullable<int> FilesCompInPR { get; set; }
    }
}
