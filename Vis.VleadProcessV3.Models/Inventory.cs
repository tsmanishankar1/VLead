using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class Inventory
    {
        public int Id { get; set; }

        public string JobId { get; set; } = null!;

        public string DeptName { get; set; } = null!;

        public int? CustId { get; set; }

        public string? ShortName { get; set; }

        public string? ProjectCode { get; set; }

        public int? DivisionId { get; set; }

        public string? DivisionName { get; set; }

        public string? FileName { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool? IsDeleted { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public DateTime? DateOfDelivery { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }

        public string? Remarks { get; set; }

        public int DepartmentId { get; set; }
    }
}