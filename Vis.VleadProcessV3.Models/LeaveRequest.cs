using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public LeaveCategory LeaveCategory { get; set; }
        public string LeaveCategoryName
        {
            get => LeaveCategory.ToString();
            set => LeaveCategory = Enum.Parse<LeaveCategory>(value);
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public bool IsDelete { get; set; }
    }
}
