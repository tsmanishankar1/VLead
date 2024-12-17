using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class InventorySubmit
    {
        public string JobId { get; set; } = null!;

        public string DeptName { get; set; } = null!;

        public string? ShortName { get; set; }

        public string? ProjectCode { get; set; }

        public string? DivisionName { get; set; }

        public string FileName { get; set; } = null!;

        public string CreatedBy { get; set; }
        public int DepartmentId { get; set; }
    }

    public class InventorySubmitPara
    {
        public List<InventorySubmit> InventorySubmit { get; set; } = null!;

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime? DateOfDelivery { get; set; }
        public string? Remarks { get; set; }
    }
}