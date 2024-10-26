using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class Getclientidanddepartmentidforbillingcycle
    {
       
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int DepartmentId { get; set; }
        public DateTime BillingDate { get; set; }
      
        public bool IsDelete { get; set; }

        public int CreatedBy { get; set; }
        
        public DateTime CreatedUtc { get; set; }

        public int? UpdateBy { get; set; }
        
        public DateTime? UpadatedUtc { get; set; }


        public string CustomerName { get; set; } 

        public string DepartmentName { get; set; } 
    }
}
