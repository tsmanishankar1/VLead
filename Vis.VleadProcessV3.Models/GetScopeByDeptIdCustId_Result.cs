using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetScopeByDeptIdCustId_Result
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
    }
}
