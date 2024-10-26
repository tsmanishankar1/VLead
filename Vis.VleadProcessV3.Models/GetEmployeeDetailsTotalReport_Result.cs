using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetEmployeeDetailsTotalReport_Result
    {
        public Nullable<int> ClientId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ClientShortName { get; set; }
        public string? Status { get; set; }
        public Nullable<int> TotalFilesAllocatedInProd { get; set; }
        public Nullable<int> TotalEstimatedTime { get; set; }
        public Nullable<int> TotalProdWorkedTime { get; set; }
        public Nullable<int> TotalProdBreakTime { get; set; }
        public Nullable<int> TotalFilesCompInProd { get; set; }
        public Nullable<int> TotalFilesAllocatedInQuality { get; set; }
        public Nullable<int> TotalQualityWorkedTime { get; set; }
        public Nullable<int> TotalFilesCompInQuality { get; set; }
        public Nullable<int> TotalFilesAllocatedInBP { get; set; }
        public Nullable<int> TotalBPWorkedTime { get; set; }
        public Nullable<int> TotalFilesCompInBP { get; set; }
        public Nullable<int> TotalFilesAllocatedInSO { get; set; }
        public Nullable<int> TotalSOWorkedTime { get; set; }
        public Nullable<int> TotalFilesCompInSO { get; set; }
        public Nullable<int> TotalFilesAllocatedInPR { get; set; }
        public Nullable<int> TotalPRWorkedTime { get; set; }
        public Nullable<int> TotalFilesCompInPR { get; set; }
    }
}
