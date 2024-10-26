using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadExcel.Model
{
    public class AttendenceModel
    {
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Devision { get; set; }
        public string Shift { get; set; }
        public string AttendanceStatus { get; set; }
    }

    public class AttendanceResult
    {
        public string Result { get; set; }
        public List<AttendenceModel> AttendanceList { get; set; }
        public DateTime Dt { get; set; }

    }
}
