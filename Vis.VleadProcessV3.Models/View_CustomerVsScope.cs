namespace Vis.VleadProcessV3.Models
{
    public partial class View_CustomerVsScope
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<bool> NeedTraining { get; set; }
        public Nullable<int> ScopeGroupId { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedUTC { get; set; }
        public int CustomerDepart { get; set; }
        public int CustomerId { get; set; }
    }
}
