using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class sp_HourlyACReport_Result
    {
        public Nullable<System.DateTime> Date { get; set; }
        public string? DivisionName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? ClientShortName { get; set; }
        public string? Process { get; set; }
        public string? Type { get; set; }
        public Nullable<int> P0 { get; set; }
        public Nullable<int> P1 { get; set; }
        public Nullable<int> P2 { get; set; }
        public Nullable<int> P3 { get; set; }
        public Nullable<int> P4 { get; set; }
        public Nullable<int> P5 { get; set; }
        public Nullable<int> P6 { get; set; }
        public Nullable<int> P7 { get; set; }
        public Nullable<int> P8 { get; set; }
        public Nullable<int> P9 { get; set; }
        public Nullable<int> P10 { get; set; }
        public Nullable<int> P11 { get; set; }
        public Nullable<int> P12 { get; set; }
        public Nullable<int> P13 { get; set; }
        public Nullable<int> P14 { get; set; }
        public Nullable<int> P15 { get; set; }
        public Nullable<int> P16 { get; set; }
        public Nullable<int> P17 { get; set; }
        public Nullable<int> P18 { get; set; }
        public Nullable<int> P19 { get; set; }
        public Nullable<int> P20 { get; set; }
        public Nullable<int> P21 { get; set; }
        public Nullable<int> P22 { get; set; }
        public Nullable<int> P23 { get; set; }
    }
}
