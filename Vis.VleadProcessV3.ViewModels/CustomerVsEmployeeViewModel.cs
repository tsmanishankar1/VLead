namespace Vis.VleadProcessV3.ViewModels
{
    public class CustomerVsEmployeeViewModel
    {
        public int Id { get; set; }
        public int[] CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int ClassId { get; set; }
    }
}
