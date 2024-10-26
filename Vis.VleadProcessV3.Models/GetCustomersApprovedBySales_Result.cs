using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{

    public partial class GetCustomersApprovedBySales_Result
    {
        // comment this for column not present exception while executing getcustomerapprovesale stored procedure
        /*******************************************************************/
        //public int Empid { get; set; }
        /*******************************************************************/

        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Description { get; set; }
        public string? Phone1 { get; set; }
        public string? EmployeeName { get; set; }
        public int Id { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
    }
}
