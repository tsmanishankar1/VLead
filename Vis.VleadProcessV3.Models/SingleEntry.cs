using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class SingleEntry
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public Nullable<DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string TableName { get; set; }
        public string TableValue { get; set; }
        public string TableValueText { get; set; }
        public string Action { get; set; }
    }
    public class SingleEntry1
    {
        public int Id { get; set; }
        public string Description { get; set; }

    }
    }
