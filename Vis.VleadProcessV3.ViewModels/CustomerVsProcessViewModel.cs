using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class CustomerVsProcessViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int CustomerClassificationId { get; set; }
        public string? CustomerJobType { get; set; }
        public bool IsBlacklisted { get; set; }
        public bool IsDeleted { get; set; }
    }
 
    public class SaveCustomerVsProcessViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int[]? CustomerId { get; set; }
        public int CurrentProcessId { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> NextProcessId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public IEnumerable<ProcessWorkFlow>? AddedProcess { get; set; }
    }
    public class AddProcessWorkFlow
    {
        public int[]? selectedScopes { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int[]? JobStatusId { get; set; }
        public string ?CustomJobType { get; set; }
        public Nullable<int> CurrentProcessId { get; set; }
        public Nullable<int> NextProcessId { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }

    }
}
