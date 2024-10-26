using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;


namespace Vis.VleadProcessV3.Services
{
    public class ClientCoOrdinationService
    {

        private readonly UnitWork _unitWork;
        private readonly ProcedureWork _procedureWork;
        private readonly IConfiguration _configuration;
     
        public ClientCoOrdinationService(IConfiguration configuration,UnitWork unitWork,ProcedureWork procedureWork)
        {
           _configuration= configuration;
            _unitWork = unitWork;
            _procedureWork = procedureWork;
        }
        public Object GetJobMovementJobsWithclientIdfileName(JobMovementViewModel jobOrder)
        {
            var NoOfDays = DateTime.UtcNow.AddDays(-jobOrder.TransactionId);
            var jobMovement = new List<JobHistoryMovement_Result>();

            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@DepartmentId",jobOrder.DepartmentId),
                new SqlParameter("@CustomerId",jobOrder.ClientId),
                new SqlParameter("@FileName",jobOrder.FileName)
            };

            // if (jobOrder.TransactionId == 0 && jobOrder.DateofUpload == null)
            if (jobOrder.TransactionId == 0 && jobOrder.DateofUpload == null)
            {
                jobMovement = _procedureWork.ExecStoredProcedure<JobHistoryMovement_Result>("JobHistoryMovement @DepartmentId, @CustomerId, @FileName", sqlPara).OrderByDescending(x => x.TranId).ToList();
            }
            //else if (jobOrder.TransactionId != 0 && jobOrder.DateofUpload == null)
            else if (jobOrder.TransactionId != 0 && jobOrder.DateofUpload == null)
            {
                //jobMovement = _procedureWork.ExecStoredProcedure<JobHistoryMovement_Result>("JobHistoryMovement @DepartmentId, @CustomerId, @FileName", sqlPara).OrderByDescending(x => x.TranId).Where(x => x.FileReceivedDate >= DateTime.UtcNow.AddDays(-jobOrder.TransactionId)).ToList();
                jobMovement = _procedureWork.ExecStoredProcedure<JobHistoryMovement_Result>("JobHistoryMovement @DepartmentId, @CustomerId, @FileName", sqlPara).OrderByDescending(x => x.TranId).Where(x => x.FileReceivedDate.Date >= NoOfDays.Date).ToList();
            }
            // else if (jobOrder.DateofUpload != null && jobOrder.TransactionId == 0)
            else if (jobOrder.DateofUpload != null && jobOrder.TransactionId == 0)
            {
                //jobMovement = _procedureWork.ExecStoredProcedure<JobHistoryMovement_Result>("JobHistoryMovement @DepartmentId, @CustomerId, @FileName", sqlPara).OrderByDescending(x => x.TranId).Where(x => (x.FileReceivedDate >= jobOrder.JobClosedUTC && x.FileReceivedDate <= jobOrder.DateofUpload)).ToList();
                jobMovement = _procedureWork.ExecStoredProcedure<JobHistoryMovement_Result>("JobHistoryMovement @DepartmentId, @CustomerId, @FileName", sqlPara).OrderByDescending(x => x.TranId).Where(x => (x.FileReceivedDate.Date >= jobOrder.JobClosedUTC.Value.Date && x.FileReceivedDate.Date <= jobOrder.DateofUpload.Value.Date)).ToList();
            }
            var result = new
            {
                JobMovement = jobMovement
            };
            return result;
        }



        public void ExportExcel()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    return;
                }
                
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet1, xlWorkSheet2, xlWorkSheet3;
                object misValue = System.Reflection.Missing.Value;
               // var grid = new GridView();

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet3 = xlWorkBook.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                xlWorkSheet3.Name = "RFQ";

                var sqlPara = new SqlParameter[]
                {
                    new SqlParameter("@ActionType",2)
                };
                var quotationJobs = _procedureWork.ExecStoredProcedure<ClientOrderCCJobs_Result>("ClientOrderCCJobs @ActionType", sqlPara).ToList();

                //var quotationJobs = _db.ClientOrderCCJobs(2).ToList();

                if (quotationJobs.Count != 0)
                {
                    //grid.DataSource= quotationJobs;

                    //grid.DataBind();
                    var headerproperties = quotationJobs[0].GetType().GetProperties();
                    var headerRow = headerproperties.Select(x => x.Name).ToList();
                    var totalCols = headerRow.Count;
                    var totalRows = quotationJobs.Count;
                  
                    for (var i = 1; i <= totalCols; i++)
                    {
                        xlWorkSheet3.Cells[1, i].Value = headerRow[i - 1];
                    }
                    for (var j = 1; j <= totalRows; j++)
                    {
                        for (var i = 1; i <= totalCols; i++)
                        {
                            var jobRow = quotationJobs.ElementAt(j - 1);
                            xlWorkSheet3.Cells[j + 1, i].Value = jobRow.GetType().GetProperty(headerRow[i - 1].ToString()).GetValue(jobRow, null);
                        }
                    }
                }
                else
                {
                    xlWorkSheet3.Cells[1, 1] = "Empty";
                }

                xlWorkSheet2 = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Microsoft.Office.Interop.Excel.Worksheet;
                xlWorkSheet2.Name = "Client Orders";
                var sqlPara1 = new SqlParameter[]
                {
                    new SqlParameter("@ActionType",1)
                };
                var clientJobs = _procedureWork.ExecStoredProcedure<ClientOrderCCJobs_Result>("ClientOrderCCJobs @ActionType", sqlPara1).ToList();
                //var clientJobs = _db.ClientOrderCCJobs(1).ToList();

                if (clientJobs.Count != 0)
                {
                    //grid.DataSource = clientJobs;
                    //grid.DataBind();
                   var clientJobsProperties = clientJobs[0].GetType().GetProperties();
                    var headerRow = clientJobsProperties.Select(x=>x.Name).ToList();
                    var totalCols = headerRow.Count;
                    var totalRows = clientJobs.Count;
                    //var totalCols = grid.Rows[0].Cells.Count;
                    //var totalRows = grid.Rows.Count;
                    //var headerRow = grid.HeaderRow;
                    for (var i = 1; i <= totalCols; i++)
                    {
                        xlWorkSheet2.Cells[1, i].Value = headerRow[i - 1];
                    }
                    for (var j = 1; j <= totalRows; j++)
                    {
                        for (var i = 1; i <= totalCols; i++)
                        {
                            var jobRow = clientJobs.ElementAt(j - 1);
                            xlWorkSheet2.Cells[j + 1, i].Value = jobRow.GetType().GetProperty(headerRow[i - 1].ToString()).GetValue(jobRow, null);
                        }
                    }
                }
                else
                {
                    xlWorkSheet2.Cells[1, 1] = "Empty";
                }

                xlWorkSheet1 = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Microsoft.Office.Interop.Excel.Worksheet;
                xlWorkSheet1.Name = "Job History";

                var sqlPara2 = new SqlParameter[]
                {
                    new SqlParameter("@ActionType",1)
                };

                var job = _procedureWork.ExecProcedureOrView<JobHistoryMovementForExcel_Result>("JobHistoryMovementForExcel").OrderByDescending(x => x.Department).ThenByDescending(x => x.FileReceivedDate).ToList();

                //var job = _db.JobHistoryMovementForExcel().OrderByDescending(x => x.Department).ThenByDescending(x => x.FileReceivedDate).ToList();

                if (job.Count != 0)
                {
                    foreach (var item in job)
                    {
                        var jo = _unitWork.JobOrder.Where(x => x.JobId == item.JobId).FirstOrDefault();
                        var pwm = _unitWork.ProcessWorkFlowMaster.Where(x => x.JobId == jo.Id).FirstOrDefault();
                        var pwt = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == pwm.Id && x.IsActive == false).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (pwt != null)
                        {
                            if (pwt.EmployeeName != null)
                            {
                                item.ArtistName = pwt.EmployeeName;
                                item.EstTime = pwt.AllocatedEstimatedTime;
                            }
                        }
                    }
                    //grid.DataSource = job;
                    //grid.DataBind();
                    var parameters = job[0].GetType().GetProperties();
                    var headerRow = parameters.Select(x => x.Name).ToList();
                    var totalCols = headerRow.Count;
                    var totalRows = job.Count;
                    //var totalCols = grid.Rows[0].Cells.Count;
                    //var totalRows = grid.Rows.Count;
                    //var headerRow = grid.HeaderRow;
                    for (var i = 1; i <= totalCols; i++)
                    {
                        xlWorkSheet1.Cells[1, i].Value = headerRow[i - 1];
                    }
                    for (var j = 1; j <= totalRows; j++)
                    {
                        for (var i = 1; i <= totalCols; i++)
                        {
                            var jobRow = job.ElementAt(j - 1);
                            xlWorkSheet1.Cells[j + 1, i].Value = jobRow.GetType().GetProperty(headerRow[i - 1].ToString()).GetValue(jobRow, null);
                        }
                    }
                }
                else
                {
                    xlWorkSheet1.Cells[1, 1] = "Empty";
                }

                var dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss");
                xlWorkBook.SaveAs(_configuration.GetSection("Excel").GetValue<string>("excelFolderPath")/*ConfigurationManager.AppSettings["excelFolderPath"] */+ dateTime + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet1);
                releaseObject(xlWorkSheet2);
                releaseObject(xlWorkSheet3);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
            catch (Exception ex)
            {
                StreamWriter sw = new StreamWriter(_configuration.GetSection("Excel").GetValue<string>("logFilePath")/*ConfigurationManager.AppSettings["logFilePath"] */+ "VIS_log.txt", true);
                sw.WriteLine(ex);
                sw.Flush();
                sw.Close();
                throw;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                StreamWriter sw = new StreamWriter(_configuration.GetSection("Excel").GetValue<string>("logFilePath")/*ConfigurationManager.AppSettings["logFilePath"] */+ "VIS_log.txt", true);
                sw.WriteLine(ex);
                sw.Flush();
                sw.Close();
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
