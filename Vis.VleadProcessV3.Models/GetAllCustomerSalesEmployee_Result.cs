using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetAllCustomerSalesEmployee_Result
    {
        public int Id { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeNameCode { get; set; }
    }
}
