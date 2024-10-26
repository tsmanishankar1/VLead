using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class CapacityFormViewModel
    {
    }

    public class CapFormDelPara
    {
        public int CapFormDelete { get; set; }
    }

    public class CapFormPara
    {
        public GivenEmployee[] GivenEmployee { get; set; } = null!;

        public GivenDivision[] GivenDivision { get; set; } = null!;

        public GivenCustomer[] GivenCustomer { get; set; } = null!;

        public int? Fresh { get; set; }

        public int? Revision { get; set; }

        public int? QC { get; set; }

        public string? Remarks { get; set; } = null!;

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }

    public class GivenEmployee
    {
        public int EmpId { get; set; }

    }

    public class GivenDivision
    {
        public int DivisionId { get; set; }
    }

    public class GivenCustomer
    {
        public int CustomerId { get; set; }
    }
}
