using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetCCIntakeAndUploadedReport_Result
    {
        public string? employeeCode { get; set; }
        public string? employeeName { get; set; }
        public Nullable<int> intake { get; set; }
        public Nullable<int> uploaded { get; set; }
    }
}
