using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class DailyBench
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public int DivisionId { get; set; }

        public string? DivisionName { get; set; }

        public string? Client { get; set; }

        public string? Shift { get; set; }

        public string? Fresh { get; set; }

        public int? Revision { get; set; }

        public int? QC { get; set; }

        public string? BenchInMins { get; set; }

        public string? Remarks { get; set; }

        public decimal? Bench { get; set; }

        public decimal? Leave { get; set; }

        public decimal? Prod { get; set; }

        public DateTime Date { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUTC { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUTC { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Division Division { get; set; } = null!;
    }
}