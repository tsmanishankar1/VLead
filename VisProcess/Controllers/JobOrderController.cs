using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using ReadExcel;

using System.Data;
using System.Data.OleDb;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;


namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobOrderController : ControllerBase
    {
        private static object _intervalSync = new object();
        private readonly JobOrderService _jobOrderService;
      
        private readonly TableWork _tablework;
        private readonly IConfiguration _configuration;
      
        private readonly string _env;
        public JobOrderController(IConfiguration configuration,
           
            IWebHostEnvironment webHostEnvironment,JobOrderService jobOrderService,TableWork tableWork)
        {
            _configuration= configuration;
            _jobOrderService = jobOrderService;
           
            _tablework = tableWork;
            _env = webHostEnvironment.ContentRootPath;
           
        }
        [HttpPost]
        [Route("PostImportExcel")]
        public async Task<IActionResult> FileUpload([FromForm] PassingExcelAndId data)
        {
            using (var _db = new ApplicationDbContext(_configuration))
            {
                int EmployeeId = data.Id;

                using (var dbTran = _db.Database.BeginTransaction())
                {
                    var totalfile = new List<FileInwardExcel>();

                    var UploadedBy = EmployeeId;
                    if (data.FormCollection.Files.Count > 0)
                    {
                        var files = new List<string>();
                        foreach (var file in data.FormCollection.Files)
                        {
                            try
                            {
                                var postedFile = data.FormCollection.Files.ToList().Where(s => s.FileName == file.FileName).FirstOrDefault();
                                if (postedFile != null)
                                {
                                    var filePath = Path.Combine(_env, postedFile.FileName);
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        await postedFile.CopyToAsync(memoryStream);
                                        memoryStream.Position = 0;

                                        // Save the file locally
                                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                        {
                                            await memoryStream.CopyToAsync(fileStream);
                                        }

                                        files.Add(filePath);

                                        // Reading Excel file using EPPlus
                                        using (var package = new ExcelPackage(memoryStream))
                                        {
                                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                                            if (worksheet != null)
                                            {
                                                var rowCount = worksheet.Dimension.Rows;
                                                var colCount = worksheet.Dimension.Columns;

                                                DataTable dataTable = new DataTable();
                                                for (int col = 1; col <= colCount; col++)
                                                {
                                                    dataTable.Columns.Add(worksheet.Cells[1, col].Text);
                                                }

                                                for (int row = 2; row <= rowCount; row++)
                                                {
                                                    DataRow dr = dataTable.NewRow();
                                                    for (int col = 1; col <= colCount; col++)
                                                    {
                                                        dr[col - 1] = worksheet.Cells[row, col].Text;
                                                    }
                                                    dataTable.Rows.Add(dr);
                                                }

                                                DataSet ds = new DataSet();
                                                ds.Tables.Add(dataTable);

                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                                    {
                                                        if (dr.ItemArray[0].ToString() != "")
                                                        {
                                                            bool AlreadyStatus = true;
                                                            string clientname = dr.ItemArray[2].ToString();
                                                            string filename = dr.ItemArray[4].ToString().Trim();
                                                            string filestatus = dr.ItemArray[5].ToString();
                                                            string department = dr.ItemArray[0].ToString();
                                                            string dateofreceived = dr.ItemArray[1].ToString();
                                                            string clientstatus = "";
                                                            string division = dr.ItemArray[22].ToString();

                                                            if (!string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(dateofreceived) && !string.IsNullOrEmpty(clientname) && !string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(filestatus) && !string.IsNullOrEmpty(division))
                                                            {
                                                                var fileimported = _tablework.FileInwardExcelRepository.Get(x => x.ClientName == clientname && x.FileName == filename).OrderByDescending(x => x.Id).ToList();
                                                                var fileimportedcount = fileimported.Count > 0;
                                                                var fileimportedclient = _tablework.CustomerRepository.Get(x => x.ShortName == clientname && x.IsDeleted == false).ToList();

                                                                if (fileimportedclient.Count != 0)
                                                                {
                                                                    clientstatus = fileimportedclient.FirstOrDefault().CustomerJobType;
                                                                }

                                                                AlreadyStatus = fileimportedclient.Count > 0;

                                                                if (fileimportedcount)
                                                                {
                                                                    foreach (var item in fileimported)
                                                                    {
                                                                        if (filestatus.Contains("Fresh"))
                                                                        {
                                                                            AlreadyStatus = false;
                                                                        }
                                                                        else
                                                                        {
                                                                            AlreadyStatus = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                AlreadyStatus = false;
                                                            }

                                                            totalfile.Add(new FileInwardExcel
                                                            {
                                                                Department = dr["Department"].ToString() != "" ? dr["Department"].ToString() : null,
                                                                ClientStatus = clientstatus,
                                                                DateofReceived = Convert.ToDateTime(dr["Date of File Received"].ToString()),
                                                                ClientName = dr["Client"].ToString() != "" ? dr["Client"].ToString() : null,
                                                                ClientJobId = dr["Client Job ID"].ToString() != "" ? dr["Client Job ID"].ToString() : null,
                                                                FileName = dr["File Name"].ToString().Trim() != "" ? dr["File Name"].ToString().Trim() : null,
                                                                JobStatusDescription = dr["File status"].ToString() != "" ? dr["File status"].ToString() : null,
                                                                Username = dr["USERNAME"].ToString() != "" ? dr["USERNAME"].ToString() : null,
                                                                SalesPersonName = dr["SALES PRO"].ToString() != "" ? dr["SALES PRO"].ToString() : null,
                                                                CustomerName = dr["CUSTOMER Name"].ToString() != "" ? dr["CUSTOMER Name"].ToString() : null,
                                                                Temp = dr["Temp"].ToString() != "" ? dr["Temp"].ToString() : null,
                                                                Style = dr["STYLE"].ToString() != "" ? dr["STYLE"].ToString() : null,
                                                                ProjectCode = dr["PROJECT CODE"].ToString() != "" ? dr["PROJECT CODE"].ToString() : null,
                                                                TeamCode = dr["TEAM CODE"].ToString() != "" ? dr["TEAM CODE"].ToString() : null,
                                                                SchoolName = dr["SCHOOL NAME"].ToString() != "" ? dr["SCHOOL NAME"].ToString() : null,
                                                                ClientSalesPerson = dr["CLIENTSALES PRO"].ToString() != "" ? dr["CLIENTSALES PRO"].ToString() : null,
                                                                Ground = dr["GROUND"].ToString() != "" ? dr["GROUND"].ToString() : null,
                                                                Gender = dr["GENDER"].ToString() != "" ? dr["GENDER"].ToString() : null,
                                                                FileInwardMode = dr["File Inward Mode"].ToString() != "" ? dr["File Inward Mode"].ToString() : null,
                                                                Priority = dr["Priority File Status"].ToString() != "" ? dr["Priority File Status"].ToString() : null,
                                                                DateofUpload = dr["Date of Upload"].ToString() != "" ? dr["Date of Upload"].ToString() : null,
                                                                PoNo = dr["PoNo"].ToString() != "" ? dr["PoNo"].ToString() : null,
                                                                DateofDelivery = dr["DateofDelivery"].ToString() != "" ? dr["DateofDelivery"].ToString() : null,
                                                                Division = dr["Division"].ToString() != "" ? dr["Division"].ToString() : null,
                                                                Status = AlreadyStatus,
                                                                UploadedBy = Convert.ToInt32(UploadedBy),
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log addlog = new Log
                                {
                                    Module = "Excel Import",
                                    Description = "Excel importing: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + ", StatusCode:" + System.Net.HttpStatusCode.NotFound,
                                    Type = "Catch block",
                                    CreatedUtc = DateTime.UtcNow
                                };
                                _db.Logs.Add(addlog);
                                _db.SaveChanges();
                                dbTran.Rollback();
                                return NotFound(ex.InnerException);
                            }

                        }
                        _db.FileInwardExcels.AddRange(totalfile);
                        _db.SaveChanges();
                        dbTran.Commit();
                        var rtemp = totalfile.ToList();
                        return Ok();

                    }
                    else
                    {
                        Log addlog = new Log();
                        addlog.Module = "Excel else Import";
                        addlog.Description = "StatusCode:" + System.Net.HttpStatusCode.NotAcceptable;
                        addlog.Type = "In excel else block";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _db.Logs.Add(addlog);
                        _db.SaveChanges();
                        dbTran.Rollback();
                        return StatusCode(409, "Not Started");

                    }

                }
            }
        }


        //Attendance
        [HttpPost]
        [Route("PostImportAttendanceExcel")]
        public IActionResult AttendanceFileUpload()
        {
            var httpRequest = HttpContext.Request.Form;
            var file = httpRequest.Files[0];


            string error = "success";
            if (file != null && file.Length > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx")
            {

                var attendanceList = ReadExcel.ReadExcel.ReadExcelFile(file.OpenReadStream(), DateTime.Now, ref error);

                //ReadExcel.ReadExcel.ReadExcelFile(file, DateTime.Now, ref error);



                var attRes = new ReadExcel.Model.AttendanceResult();
                attRes.Result = error;
                attRes.AttendanceList = attendanceList;
                return Ok(attRes);
             
            }
            else
            {
                var attRes = new ReadExcel.Model.AttendanceResult();
                attRes.Result = "invalidfile";

                return Ok(attRes);


            }

        }

        [HttpPost]
        [Route("SaveAttendanceList")]
        public Object SaveAttendanceList(ReadExcel.Model.AttendanceResult AttendanceResult)
        {
            var result = SaveAttendace(AttendanceResult.AttendanceList, AttendanceResult.Dt);
            var test = new
            {
                Result = result
            };
            //return Ok(result);
            return test;

        }
        [ApiExplorerSettings(IgnoreApi =true)]
        public string SaveAttendace(List<ReadExcel.Model.AttendenceModel> list, DateTime attendanceDate)
        {
            var result = "";
            var dt = new DataTable();
            dt.Columns.Add("EmpCode");
            dt.Columns.Add("Name");
            dt.Columns.Add("Devision");
            dt.Columns.Add("Shift");
            dt.Columns.Add("AttendanceStatus");

            foreach (var item in list)
            {
                var row = dt.NewRow();

                //row["EmpCode"] = item.EmpCode;
                //row["Name"] = item.Name;
                //row["Devision"] = item.Devision;
                //row["Shift"] = item.Shift;
                //row["AttendanceStatus"] = item.AttendanceStatus;
                dt.Rows.Add(item.EmpCode, item.Name, item.Devision, item.Shift, item.AttendanceStatus);

            }


            string strcon = _configuration.GetConnectionString("DBConnection");

            var connection = new SqlConnection(strcon);

            // Assumes connection is an open SqlConnection object.  
            try
            {
                using (connection)
                {
                    // Create a DataTable with the modified rows.  
                    //DataTable addedCategories = CategoriesDataTable.GetChanges(DataRowState.Added);
                    connection.Open();
                    // Configure the SqlCommand and SqlParameter.  
                    SqlCommand insertCommand = new SqlCommand("SaveAttendance", connection);
                    insertCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = insertCommand.Parameters.AddWithValue("@TBLTypeAttendanceList", dt);
                    param1.SqlDbType = SqlDbType.Structured;

                    SqlParameter param2 = insertCommand.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                    param2.SqlDbType = SqlDbType.Date;

                    SqlParameter param3 = insertCommand.Parameters.AddWithValue("@User", 1);
                    param3.SqlDbType = SqlDbType.NVarChar;
                    SqlDataAdapter adp = new SqlDataAdapter(insertCommand);
                    DataSet Ds = new DataSet();
                    adp.Fill(Ds);
                    result = Ds.Tables[0].Rows[0]["Result"].ToString();
                    // Execute the command.  
                    //insertCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                result = ex.ToString();
            }

            return result;
        }

        //Attendance end
        [HttpPost]
        [Route("getJobHistory")]

        public IActionResult GetJobHistory([FromBody] int JId)
        {
            try
            {
                var result = _jobOrderService.GetJobHistory(JId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
           
        }
        //
        [HttpPost]
        [Route("DirectOrder")]
        public FileMovement DirectOrder(InwardExcel oFileInwardExcel)
        {
            //lock (_intervalSync)
            //{
            return _jobOrderService.DirectOrder(oFileInwardExcel);
            //}                        
        }
        [HttpPost]
        [Route("ExcelOrder")]
        public FileMovement ExcelOrder(InwardExcel oFileInwardExcel)
        {
            //lock (_intervalSync)
            //{
            return _jobOrderService.ExcelOrder(oFileInwardExcel);
            //}                        
        }
        [HttpPost]
        [Route("InternalOrder")]
        public FileMovement InternalOrder(InwardExcel oFileInwardExcel)
        {
            //lock (_intervalSync)
            //{
            return _jobOrderService.InternalOrder(oFileInwardExcel);
            //}                        
        }
        //
       
        [HttpPost]
        [Route("GetAssignedEmployees")]
        public Object GetAssignedEmployees(ViewJobAllocation CustomerJobType)
        {
            var getAssignedEmployees = _jobOrderService.GetAssignedEmployees(CustomerJobType);
            var result = new
            {
                AssignedEmployees = getAssignedEmployees
            };
            return result;
        }
        [HttpPost]
        [Route("GetPendingJobsWithEmployeeId")]
        public Object GetPendingJobsWithEmployeeId([FromBody] ViewJobAllocation CustomerJobType)
        {
            var getPendingJobsWithEmployeeId = _jobOrderService.GetPendingJobsWithEmployeeId(CustomerJobType);
            var result = new
            {
                PendingJobsWithEmployeeId = getPendingJobsWithEmployeeId
            };
            return result;
        }
        [HttpGet]
        [Route("GetImportExcel")]
        public IEnumerable<ViewFileInwardExcel> GetImportExcel(int employeeId)
        {
            var result = _jobOrderService.GetImportExcel(employeeId);
            return result;
        }
        [HttpGet]
        [Route("GetImportExcelTrue")]
        public IActionResult GetImportExcelTrue(int employeeId)
        {
            return Ok(_jobOrderService.GetImportExcelTrue(employeeId));
        }
        [HttpPost]
        [Route("openFolder")]
        public Object openFolder(FileMovement folderPath)
        {
            var getScopeValues = _jobOrderService.openFolder(folderPath);
            var result = new
            {
                ScopeValues = getScopeValues
            };
            return result;
        }
        [HttpDelete]
        [Route("CancelImportExcel")]
        public void CancelImportExcel()
        {
            _jobOrderService.CancelImportExcel();
        }
        [HttpPost]
        [Route("nGetNonBillableData")]
        public IActionResult GetNonBillableData(NonBillablePara para)
        {
            var GetNonBillable = _jobOrderService.GetNonBillableData(para);
            var result = new
            {
                GetNonBList = GetNonBillable
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("nGetNonBillable")]
        public IActionResult GetNonBillable()
        {
            var GetDDLNB = _jobOrderService.GetNonBillable();
            var result = new
            {
                GetDDLNBList = GetDDLNB
            };
            return Ok(result);
        }
        [HttpPost]
        [Route("nUpdateNonBillable")]
        public IActionResult UpdateNonBillable(NBJIdPara GetNBPara)
        {
            var UpdateNonBillable = _jobOrderService.UpdateNonBillable(GetNBPara);
            var result = new
            {
                UpdateNonBList = UpdateNonBillable
            };
            return Ok(result);
        }

        //credit days approval service
        [HttpGet]
        [Route("nGetServiceforCreditDays")]
        public IActionResult GetServiceforCreditDays()
        {
            var GetSCDDetail = _jobOrderService.GetServiceforCreditDays();
            var result = new
            {
                GetSCDList = GetSCDDetail
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("nSendMailServiceCreditDays")]
        public IActionResult SendMailServiceCreditDays()
        {
            _jobOrderService.SendMailServiceCreditDays();
            return Ok();
        }
    }
}
