//using ExcelDataReader;
//using Microsoft.AspNetCore.Http;
//using Microsoft.VisualBasic;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Vis.VleadProcessV3.Models;

//public class ExternalErrorReportService
//{
//    private readonly ApplicationDbContext _context;
//    private readonly string uploadFile = "ExternalErrorReport\\UploadedFiles";

//    public ExternalErrorReportService(ApplicationDbContext context)
//    {
//        _context = context;

//        var filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadFile);
//        if (!Directory.Exists(filePath))
//        {
//            Directory.CreateDirectory(filePath);
//        }
//    }

//    public ExcelErrorReportResponse ExternalErrorReportExcel(ExternalErrorReportExcel excel)
//    {
//        try
//        {
//            string message = "";
//            List<ErrorRecord> errorRecords = new List<ErrorRecord>();
//            ExcelErrorReportResponse response = new ExcelErrorReportResponse();

//            if (excel.file != null)
//            {
//                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

//                using (var stream = excel.file.OpenReadStream())
//                {
//                    using (var reader = ExcelReaderFactory.CreateReader(stream))
//                    {
//                        var result = reader.AsDataSet();
//                        var data = new List<ExternalErrorReport>();

//                        if (result != null && result.Tables.Count > 0)
//                        {
//                            DataTable dtStudentRecords = result.Tables[0];

//                            if (dtStudentRecords.Rows.Count > 1)
//                            {
//                                foreach (DataRow row in dtStudentRecords.Rows)
//                                {
//                                    if (row.Table.Rows.IndexOf(row) == 0)
//                                        continue;

//                                    string dateFormat = "dd-MM-yyyy HH:mm:ss";
//                                    DateTime inputMonthYear = DateTime.ParseExact(Convert.ToString(row[12]), dateFormat, System.Globalization.CultureInfo.InvariantCulture);

//                                    string jobNumber = Convert.ToString(row[0]);
//                                    string artistId = Convert.ToString(row[6]);
//                                    string department = Convert.ToString(row[1]);
//                                    string client = Convert.ToString(row[2]);
//                                    string clientStatus = Convert.ToString(row[3]);
//                                    string jobStatus = Convert.ToString(row[4]);
//                                    string fileName = Convert.ToString(row[5]);
//                                    string artistStr = Convert.ToString(row[7]);
//                                    string qcIdStr = Convert.ToString(row[8]);
//                                    string qcStr = Convert.ToString(row[9]);
//                                    string clientRevisionCommentStr = Convert.ToString(row[10]);
//                                    string errorTypeStr = Convert.ToString(row[11]);
//                                    string division = Convert.ToString(row[13]);

//                                    var division1 = _context.Divisions.Where(d => d.DivisionName == division).FirstOrDefault();
//                                    if (division1 == null)
//                                    {
//                                        ErrorRecord errorRecord = new ErrorRecord
//                                        {
//                                            JobNumber = jobNumber,
//                                            Department = department,
//                                            Client = client,
//                                            ClientStatus = clientStatus,
//                                            JobStatus = jobStatus,
//                                            FileName = fileName,
//                                            ArtistId = artistId,
//                                            ArtistName = artistStr,
//                                            QcId = qcIdStr,
//                                            QcName = qcStr,
//                                            ClientRevisionComment = clientRevisionCommentStr,
//                                            ErrorType = errorTypeStr,
//                                            InputMonthYear = inputMonthYear,
//                                            Division = division
//                                        };
//                                        errorRecords.Add(errorRecord);
//                                        continue;
//                                    }
//                                    int? qcId = string.IsNullOrEmpty(qcIdStr) ? null : (int?)Convert.ToInt32(qcIdStr);
//                                    string artistName = string.IsNullOrEmpty(artistStr) ? null : artistStr;
//                                    string qcName = string.IsNullOrEmpty(qcStr) ? null : qcStr;
//                                    string clientRevisionComment = string.IsNullOrEmpty(clientRevisionCommentStr) ? null : clientRevisionCommentStr;
//                                    string errorType = string.IsNullOrEmpty(errorTypeStr) ? null : errorTypeStr;

//                                    int? employeeId = GetEmployeeIdByArtistId(artistId);
//                                    if (employeeId.HasValue)
//                                    {
//                                        ExternalErrorReport existingRecord = _context.ExternalErrorReports
//                                            .FirstOrDefault(e => e.JobNumber == jobNumber &&
//                                                                 e.ArtistId == artistId &&
//                                                                 e.InputMonthYear == inputMonthYear &&
//                                                                 e.CreatedBy == excel.CreatedBy);

//                                        if (existingRecord != null)
//                                        {
//                                            continue;
//                                        }

//                                        ExternalErrorReport errorReport = new ExternalErrorReport
//                                        {
//                                            EmployeeId = employeeId.Value,
//                                            CreatedBy = excel.CreatedBy,
//                                            CreatedUtc = DateTime.UtcNow,
//                                            DivisionId = division1.Id,
//                                            JobNumber = jobNumber,
//                                            Department = department,
//                                            Client = client,
//                                            ClientStatus = clientStatus,
//                                            JobStatus = jobStatus,
//                                            FileName = fileName,
//                                            ArtistId = artistId,
//                                            ArtistName = artistName,
//                                            QcId = qcId,
//                                            QcName = qcName,
//                                            ClientRevisionComment = clientRevisionComment,
//                                            ErrorType = errorType,
//                                            InputMonthYear = inputMonthYear,
//                                            Division = division
//                                        };

//                                        data.Add(errorReport);
//                                    }
//                                }

//                                if (data.Count > 0)
//                                {
//                                    _context.ExternalErrorReports.AddRange(data);
//                                    int output = _context.SaveChanges();

//                                    if (output > 0)
//                                    {
//                                        message = "The Excel file has been successfully uploaded.";
//                                        response.Success = true;
//                                    }
//                                    else
//                                    {
//                                        message = "The Excel file is already uploaded.";
//                                        response.Success = true;
//                                    }
//                                }
//                                else
//                                {
//                                    message = "Excel file is already uploaded.";
//                                }
//                            }
//                            else
//                            {
//                                throw new MessageNotFoundException("Selected file is empty");
//                            }
//                        }
//                        else
//                        {
//                            throw new MessageNotFoundException("Selected file is empty");
//                        }
//                    }
//                }
//            }
//            else
//            {
//                throw new CustomException("Invalid File.");
//            }

//            if (errorRecords.Count > 0)
//            {
//                message = "The following records encountered errors while processing the Excel file:";
//                response.ErrorRecords = errorRecords;
//            }

//            response.Message = message;
//            return response;
//        }
//        catch (MessageNotFoundException ex)
//        {
//            throw new MessageNotFoundException("Selected file is empty");
//        }
//        catch (CustomException ex)
//        {
//            throw new CustomException("Invalid File.");
//        }
//        catch (Exception ex)
//        {
//            throw new Exception("Error processing Excel file.", ex);
//        }
//    }

//    private int? GetEmployeeIdByArtistId(string artistId)
//    {
//        var employee = _context.Employees.FirstOrDefault(e => e.EmployeeCode == artistId && e.IsDeleted == false);

//        return employee?.EmployeeId;
//    }
//}
