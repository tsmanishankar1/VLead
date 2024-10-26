using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class OutsourceCount
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }

        public int CustomerId { get; set; }

        public DateTime Date { get; set; }

        public int OutSourceFilecount { get; set; }

        public int OutsourceAmount { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUTC { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUTC { get; set; }

        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public virtual Customer? Customer { get; set; }
        [JsonIgnore]
        public virtual Division? Division { get; set; }
    }
}