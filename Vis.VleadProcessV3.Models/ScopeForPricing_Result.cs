namespace Vis.VleadProcessV3.Models
{
    public partial class ScopeforPricing_Result
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<bool> NeedTraining { get; set; }
        public Nullable<int> ScopeGroupId { get; set; }
    }
}
