using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using ReadExcel;

using System.Data;
using System.Data.OleDb;
using System.Security.Policy;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [Route("api/Excel")]
    [ApiController]
    public class HolidayMasterController : ControllerBase
    {
        private readonly HolidayMasterService _holidayMasterService;
        private readonly IConfiguration _configuration;
        private readonly string _env;

        public HolidayMasterController(HolidayMasterService holidayMasterService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _holidayMasterService = holidayMasterService;
            _configuration = configuration;
            _env = webHostEnvironment.ContentRootPath;
        }

        [HttpPost("UploadHolidayExcel")]
        public async Task<IActionResult> UploadHolidayExcelFile([FromForm] PassingExcelAndId data)
        {
            var _db = new ApplicationDbContext(_configuration);
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                using (_db)
                {
                    int createdBy = data.Id;
                    using (dbTran)
                    {
                        var holidays = new List<HolidayMaster>();
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
                                                            bool isDeleted = false;
                                                            DateTime createdUtc = DateTime.UtcNow;
                                                            string holidayDescription = dr.ItemArray[0].ToString();
                                                            string holidayDate = dr.ItemArray[1].ToString();
                                                            string holidayType = dr.ItemArray[2].ToString();
                                                            if (!string.IsNullOrEmpty(holidayDescription) && !string.IsNullOrEmpty(holidayType))
                                                            {
                                                                holidays.Add(new HolidayMaster
                                                                {
                                                                    HolidayDescription = dr["HolidayDescription"].ToString() != "" ? dr["HolidayDescription"].ToString() : null,
                                                                    HolidayType = dr["HolidayType"].ToString() != "" ? dr["HolidayType"].ToString() : null,
                                                                    HolidayDate = Convert.ToDateTime(dr["HolidayDate"].ToString()),
                                                                    IsDelete = isDeleted,
                                                                    CreatedBy = Convert.ToInt32(createdBy),
                                                                    CreatedByUtc = createdUtc,
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
                            _db.HolidayMasters.AddRange(holidays);
                            _db.SaveChanges();
                            dbTran.Commit();
                            var rtemp = holidays.ToList();
                            var response = new
                            {
                                Success = true,
                                Message = "Holiday Master Uploaded Successfully"
                            };
                            return Ok(response);
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
                            var response = new
                            {
                                Success = false,
                                Message = "Not Started"
                            };
                            return Ok(response);
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
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
        }

        [HttpGet("GetHolidayYears")]
        public IActionResult GetHolidayYears()
        {
            try
            {
                var years = _holidayMasterService.GetHolidayYears();

                var response = new
                {
                    Success = true,
                    Years = years
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
        }

        [HttpGet("GetHolidayListByYear")]
        public IActionResult GetHolidayListByYear(int year)
        {
            try
            {
                List<HolidayMaster> holidays = _holidayMasterService.GetHolidayListByYear(year);

                if (holidays.Count > 0)
                {
                    var response = holidays.Select(h => new HolidayMasters
                    {
                        Id = h.Id,
                        HolidayDescription = h.HolidayDescription,
                        HolidayDate = h.HolidayDate,
                        HolidayType = h.HolidayType
                    }).ToList();

                    var response1 = new
                    {
                        Success = true,
                        Message = response
                    };
                    return Ok(response1);
                }
                else
                    throw new MessageNotFoundException($"No holidays found for the year {year}");
            }
            catch (MessageNotFoundException ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
            catch (CustomException ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
        }

    }
}