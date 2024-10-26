using ClosedXML.Excel;
using ReadExcel.Model;
using System.Data;

namespace ReadExcel
{
    public static class ReadExcel
    {

        public static List<AttendenceModel> ReadExcelFile(System.IO.Stream memoryStream, DateTime dt, ref string error)
        {
            var attendanceList = new List<AttendenceModel>();
            DataTable tbl = new DataTable();
            //Checking file content length and Extension must be .xlsx  
            //if (file != null && file.ContentLength > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx")
            //{




            //Started reading the Excel file.  
            using (XLWorkbook workbook = new XLWorkbook(memoryStream))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                bool FirstRow = true;
                //Range for reading the cells based on the last cell used.  
                string readRange = "1:5";
                foreach (IXLRow row in worksheet.RowsUsed())
                {
                    //If Reading the First Row (used) then add them as column name  
                    if (FirstRow)
                    {
                        //var LastCellNumber = row.LastCellUsed().Address.ColumnNumber;
                        ////Checking the Last cellused for column generation in datatable  

                        //if (LastCellNumber == 5)
                        //{

                        //    var attendance=new AttendenceModel();
                        //    tbl.Columns.Add("EmpCde");
                        //    tbl.Columns.Add("Name");
                        //    tbl.Columns.Add("Devision");
                        //    tbl.Columns.Add("Shift");
                        //    tbl.Columns.Add("AttendanceStatus");

                        //}
                        //readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        //foreach (IXLCell cell in row.Cells(readRange))
                        //{
                        //    tbl.Columns.Add(cell.Value.ToString());
                        //}
                        FirstRow = false;
                    }
                    else
                    {
                        //Adding a Row in datatable  
                        tbl.Rows.Add();
                        //int cellIndex = 0;
                        //Updating the values of datatable  


                        var columnId = 1;
                        var attendance = new AttendenceModel();

                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            var value = cell.Value.ToString();

                            if (columnId == 1)
                            {
                                attendance.EmpCode = value;
                            }
                            else if (columnId == 2)
                            {
                                attendance.Name = value;

                            }
                            else if (columnId == 3)
                            {
                                attendance.Devision = value;

                            }
                            else if (columnId == 4)
                            {
                                attendance.Shift = value;

                            }
                            else if (columnId == 5)
                            {
                                attendance.AttendanceStatus = value;

                            }
                            columnId++;
                        }

                        attendanceList.Add(attendance);
                    }
                }
                //If no data in Excel file  
                if (FirstRow)
                {
                    error = "Empty Excel File!";
                }
            }
            //}
            //else
            //{
            //    //If file extension of the uploaded file is different then .xlsx  
            //    error = "Please select file with .xlsx extension!";
            //}
            //SaveAttendace(attendanceList, dt);

            return attendanceList;
        }
    }
}