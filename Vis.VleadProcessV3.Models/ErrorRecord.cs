using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class ErrorRecord
    {
        public string JobNumber { get; set; }
        public string Department { get; set; }
        public string Client { get; set; }
        public string ClientStatus { get; set; }
        public string JobStatus { get; set; }
        public string FileName { get; set; }
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string QcId { get; set; }
        public string QcName { get; set; }
        public string ClientRevisionComment { get; set; }
        public string ErrorType { get; set; }
        public DateTime InputMonthYear { get; set; }
        public string Division { get; set; }
    }

    public class ExcelErrorReportResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErrorRecord> ErrorRecords { get; set; }
    }
}
