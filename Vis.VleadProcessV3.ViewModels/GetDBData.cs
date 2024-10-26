using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class GetDBData
    {
        public string EmployeeName { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProductionFreshCount { get; set; }
        public int? ProductionRevisionCount { get; set; }
        public int? QualityFreshCount { get; set; }
        public int? QualityRevisionCount { get; set; }
        public int? OutsourceProductionCount { get; set; }

        public int? BenchProductionFreshCount { get; set; }
        public int? BenchProductionRevisionCount { get; set; }
        public int? BenchQualityFreshCount { get; set; }
        public int? BenchQualityRevisionCount { get; set; }
        //public bool? IsBenchTaken { get; set; }
        //public bool? IsBenchGiven { get; set; }
        public IEnumerable<GetBenchData> GetBenchDetails { get; set; }
        public int? GetBenchGivenDivision { get; set; }
        public int? GetBenchTakenDivision { get; set; }
    }

    public class GetBenchData
    {
        //public string EmployeeName { get; set; }
        //public int EmployeeId { get; set; }
        //public int DivisionId { get; set; }
        //public bool? IsBenchTaken { get; set; }
        // public bool? IsBenchGiven { get; set; }
        public int? BenchTakenDivId { get; set; }
        public int? BenchGivenDivId { get; set; }
    }
}
