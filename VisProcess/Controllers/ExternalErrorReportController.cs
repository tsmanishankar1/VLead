using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Renci.SshNet.Messages;
using System.Configuration;
using System.Data;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalErrorReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _env;
        private readonly ApplicationDbContext _context;
        public ExternalErrorReportController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _configuration = configuration;
            _env = webHostEnvironment.ContentRootPath;
            _context = context;
        }

        [HttpPost("ExternalErrorReportExcel")]
        public async Task<IActionResult> ExternalErrorReportExcel([FromForm] PassingExcelAndId data)
        {
            var _db = new ApplicationDbContext(_configuration);
            var dbTran = _db.Database.BeginTransaction();
            var message = "";
            try
            {
                List<ErrorRecord> errorRecords = new List<ErrorRecord>();
                ExcelErrorReportResponse response1 = new ExcelErrorReportResponse();

                using (_db)
                {
                    int createdBy = data.Id;
                    using (dbTran)
                    {
                        var externalErrorReports = new List<ExternalErrorReport>();
                        var uploadedBy = createdBy;

                        if (data.FormCollection.Files.Count > 0)
                        {
                            var files = new List<string>();
                            foreach (var file in data.FormCollection.Files)
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
                                                            DateTime createdUtc = DateTime.UtcNow;
                                                            string jobNumber = dr.ItemArray[0].ToString();
                                                            string department = dr.ItemArray[1].ToString();
                                                            string client = dr.ItemArray[2].ToString();
                                                            string clientStatus = dr.ItemArray[3].ToString();
                                                            string jobStatus = dr.ItemArray[4].ToString();
                                                            string fileName = dr.ItemArray[5].ToString();
                                                            string artistId = dr.ItemArray[6].ToString();
                                                            string artistStr = dr.ItemArray[7].ToString();
                                                            string qcIdStr = dr.ItemArray[8].ToString();
                                                            string qcStr = dr.ItemArray[9].ToString();
                                                            string clientRevisionCommentStr = dr.ItemArray[10].ToString();
                                                            string errorTypeStr = dr.ItemArray[11].ToString();
                                                            DateTime inputMonthYear = DateTime.Parse(dr.ItemArray[12].ToString());
                                                            string division = dr.ItemArray[13].ToString();

                                                            if (!string.IsNullOrEmpty(jobNumber) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(client) && !string.IsNullOrEmpty(clientStatus) && !string.IsNullOrEmpty(jobStatus) && !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(artistId) && !string.IsNullOrEmpty(division))
                                                            {
                                                                var division1 = _db.Divisions.FirstOrDefault(d => d.DivisionName == division);
                                                                if (division1 == null)
                                                                {
                                                                    errorRecords.Add(new ErrorRecord
                                                                    {
                                                                        JobNumber = dr["Job Number"].ToString() != "" ? dr["Job Number"].ToString() : null,
                                                                        Department = dr["Department"].ToString() != "" ? dr["Department"].ToString() : null,
                                                                        Client = dr["Client"].ToString() != "" ? dr["Client"].ToString() : null,
                                                                        ClientStatus = dr["Client Status"].ToString() != "" ? dr["Client Status"].ToString() : null,
                                                                        JobStatus = dr["Job Status"].ToString() != "" ? dr["Job Status"].ToString() : null,
                                                                        FileName = dr["File Name"].ToString() != "" ? dr["File Name"].ToString() : null,
                                                                        ArtistId = dr["Artist Id"].ToString() != "" ? dr["Artist Id"].ToString() : null,
                                                                        ArtistName = dr["Artist Name"].ToString() != "" ? dr["Artist Name"].ToString() : null,
                                                                        QcId = dr["Qc Id"].ToString() != "" ? dr["Qc Id"].ToString() : null,
                                                                        QcName = dr["Qc Name"].ToString() != "" ? dr["Qc Name"].ToString() : null,
                                                                        ClientRevisionComment = dr["Client Revision Comment"].ToString() != "" ? dr["Client Revision Comment"].ToString() : null,
                                                                        ErrorType = dr["Error Type"].ToString() != "" ? dr["Error Type"].ToString() : null,
                                                                        InputMonthYear = Convert.ToDateTime(dr["Input Month-Year"].ToString()),
                                                                        Division = dr["Division"].ToString() != "" ? dr["Division"].ToString() : null,
                                                                    });
                                                                    continue;
                                                                }

                                                                int? qcId = string.IsNullOrEmpty(qcIdStr) ? null : (int?)Convert.ToInt32(qcIdStr);
                                                                string artistName = string.IsNullOrEmpty(artistStr) ? null : artistStr;
                                                                string qcName = string.IsNullOrEmpty(qcStr) ? null : qcStr;
                                                                string clientRevisionComment = string.IsNullOrEmpty(clientRevisionCommentStr) ? null : clientRevisionCommentStr;
                                                                string errorType = string.IsNullOrEmpty(errorTypeStr) ? null : errorTypeStr;

                                                                int? employeeId = GetEmployeeIdByArtistId(artistId);
                                                                if (employeeId.HasValue)
                                                                {
                                                                    ExternalErrorReport existingRecord = _context.ExternalErrorReports
                                                                        .FirstOrDefault(e => e.JobNumber == jobNumber &&
                                                                                             e.ArtistId == artistId &&
                                                                                             e.InputMonthYear == inputMonthYear &&
                                                                                             e.CreatedBy == createdBy);

                                                                    if (existingRecord != null)
                                                                    {
                                                                        continue;
                                                                    }

                                                                    externalErrorReports.Add(new ExternalErrorReport
                                                                    {
                                                                        EmployeeId = Convert.ToInt32(employeeId),
                                                                        CreatedBy = Convert.ToInt32(createdBy),
                                                                        CreatedUtc = createdUtc,
                                                                        DivisionId = Convert.ToInt32(division1.Id),
                                                                        JobNumber = dr["Job Number"].ToString() != "" ? dr["Job Number"].ToString() : null,
                                                                        Department = dr["Department"].ToString() != "" ? dr["Department"].ToString() : null,
                                                                        Client = dr["Client"].ToString() != "" ? dr["Client"].ToString() : null,
                                                                        ClientStatus = dr["Client Status"].ToString() != "" ? dr["Client Status"].ToString() : null,
                                                                        JobStatus = dr["Job Status"].ToString() != "" ? dr["Job Status"].ToString() : null,
                                                                        FileName = dr["File Name"].ToString() != "" ? dr["File Name"].ToString() : null,
                                                                        ArtistId = dr["Artist Id"].ToString() != "" ? dr["Artist Id"].ToString() : null,
                                                                        ArtistName = dr["Artist Name"].ToString() != "" ? dr["Artist Name"].ToString() : null,
                                                                        QcId = Convert.ToInt32(dr["Qc Id"].ToString() != "" ? dr["Qc Id"].ToString() : null),
                                                                        QcName = dr["Qc Name"].ToString() != "" ? dr["Qc Name"].ToString() : null,
                                                                        ClientRevisionComment = dr["Client Revision Comment"].ToString() != "" ? dr["Client Revision Comment"].ToString() : null,
                                                                        ErrorType = dr["Error Type"].ToString() != "" ? dr["Error Type"].ToString() : null,
                                                                        InputMonthYear = Convert.ToDateTime(dr["Input Month-Year"].ToString()),
                                                                        Division = division1.DivisionName,
                                                                    });
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            _db.ExternalErrorReports.AddRange(externalErrorReports);
                            var output = _db.SaveChanges();
                            dbTran.Commit();
                            var rtemp = externalErrorReports.ToList();

                            if (output > 0)
                                message = "The Excel file has been successfully uploaded.";
                        }
                        if (errorRecords.Count > 0)
                        {
                            message = "The following records encountered errors while processing the Excel file:";
                            response1.ErrorRecords = errorRecords;
                        }
                        var response = new
                        {
                            Success = true,
                            Message = message,
                            ErrorRecords = response1.ErrorRecords = errorRecords
                        };
                        return Ok(response);
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
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
        }

        private int? GetEmployeeIdByArtistId(string artistId)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeCode == artistId && e.IsDeleted == false);

            return employee?.EmployeeId;
        }
    }
}
