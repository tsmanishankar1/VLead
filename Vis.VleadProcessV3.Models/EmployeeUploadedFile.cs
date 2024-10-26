using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class EmployeeUploadedFile
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class FileViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class EmployeeFileDelete
    {
        public int UpdatedBy { get; set; }
    }
}