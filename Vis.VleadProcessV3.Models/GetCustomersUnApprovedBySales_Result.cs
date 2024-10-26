using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
   
    public partial class GetCustomersUnApprovedBySales_Result
    {
        // comment this for column not present exception while executing getcustomerunapprovesale stored procedure
        /*******************************************************************/
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        /*******************************************************************/
        public string? UserName { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? EmailID { get; set; }
        public int Id { get; set; }
        public string? PhoneNo { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
    }
}
