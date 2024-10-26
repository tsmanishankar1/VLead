using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ActionStatus
    {
        public string message { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class AddShiftVsEmployee
    {
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedById { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public string Test { get; set; }
    }
    public class SaveShiftVsEmployee
    {
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedById { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public IEnumerable<GetViewEmployeeVsShiftDetails> Test { get; set; }
    }
}
