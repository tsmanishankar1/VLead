using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceSheetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceSheetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Upload/{employeeId}")]
        public IActionResult UploadAttendanceSheet(IFormFile file, int employeeId)
        {
            try
            {

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var message = string.Empty;

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        string StaffId = worksheet.Cells[row, 2].Text;
                        string Date = worksheet.Cells[row, 7].Text;
                        var existingRecord = _context.AttendanceSheet.FirstOrDefault(x => x.StaffId == StaffId && x.Date == Date);
                        if (existingRecord != null)
                        {
                            message = message + $"Duplicate found for StaffId: {StaffId} for Date: {Date}, ";
                        }

                        decimal.TryParse(worksheet.Cells[row, 19].Text, out decimal absent);
                        decimal.TryParse(worksheet.Cells[row, 20].Text, out decimal present);
                        decimal.TryParse(worksheet.Cells[row, 21].Text, out decimal weeklyHoliday);
                        decimal.TryParse(worksheet.Cells[row, 22].Text, out decimal total);
                        bool.TryParse(worksheet.Cells[row, 12].Text, out bool isBreakExceeded);

                        var report = new AttendanceSheet
                        {
                            StaffId = worksheet.Cells[row, 2].Text,
                            Name = worksheet.Cells[row, 3].Text,
                            Department = worksheet.Cells[row, 4].Text,
                            Designation = worksheet.Cells[row, 5].Text,
                            ShiftTime = worksheet.Cells[row, 6].Text,
                            Date = worksheet.Cells[row, 7].Text,
                            InTime = worksheet.Cells[row, 8].Text,
                            OutTime = worksheet.Cells[row, 9].Text,
                            TotalHoursWorked = worksheet.Cells[row, 10].Text,
                            BreakHours = worksheet.Cells[row, 11].Text,
                            IsBreakExceeded = isBreakExceeded,
                            ProductiveHours = worksheet.Cells[row, 13].Text,
                            AttendanceStatus = worksheet.Cells[row, 14].Text,
                            EarlyEntry = worksheet.Cells[row, 15].Text,
                            LateEntry = worksheet.Cells[row, 16].Text,
                            EarlyExit = worksheet.Cells[row, 17].Text,
                            ExtraHoursWorked = worksheet.Cells[row, 18].Text,
                            Absent = absent,
                            Present = present,
                            WeeklyHoliday = weeklyHoliday,
                            Total = total,
                            CreatedBy = employeeId,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.AttendanceSheet.Add(report);
                        _context.SaveChanges();
                    }
                }

                //_context.AttendanceSheets.AddRange(newRecords);

                return Ok(new
                {
                    sucsess = true,
                    message = $"File processed successfully. {message}"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}