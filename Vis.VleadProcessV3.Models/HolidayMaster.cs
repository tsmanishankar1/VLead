using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class HolidayMaster
    {
        public int Id { get; set; }

        public string HolidayDescription { get; set; } = null!;

        public DateTime HolidayDate { get; set; }

        public string HolidayType { get; set; } = null!;

        public bool IsDelete { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedByUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedByUtc { get; set; }
    }

    public partial class HolidayMasters
    {
        public int Id { get; set; }

        public string HolidayDescription { get; set; } = null!;

        public DateTime HolidayDate { get; set; }

        public string HolidayType { get; set; } = null!;
    }

    public class HolidayYear
    {
        public List<int> year { get; set; }
    }

}
