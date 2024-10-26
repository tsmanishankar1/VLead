using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Please upload a valid Excel file." });

                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                if (employee == null)
                    return BadRequest(new { message = $"Employee with id {employeeId} does not exist." });

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var newRecords = new List<AttendanceSheet>();

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[4];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int.TryParse(worksheet.Cells[row, 1].Text, out int sNo);
                        if (sNo == 0)
                        {
                            return BadRequest(new { message = "Sno cannot be null or zero." });
                        }

                        var existingRecord = _context.AttendanceSheets.FirstOrDefault(x => x.Sno == sNo);
                        if (existingRecord != null)
                        {
                            return BadRequest(new { message = $"Duplicate found for S.No: {sNo}. Record already exists." });
                        }

                        int.TryParse(worksheet.Cells[row, 15].Text, out int absent);
                        int.TryParse(worksheet.Cells[row, 14].Text, out int present);
                        int.TryParse(worksheet.Cells[row, 16].Text, out int weeklyHoliday);
                        int.TryParse(worksheet.Cells[row, 17].Text, out int total);
                        bool.TryParse(worksheet.Cells[row, 13].Text, out bool isBreakExceeded);

                        var report = new AttendanceSheet
                        {
                            Sno = sNo,
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
                            ProductiveHours = worksheet.Cells[row, 12].Text,
                            AttendanceStatus = worksheet.Cells[row, 18].Text,
                            EarlyEntry = worksheet.Cells[row, 19].Text,
                            LateEntry = worksheet.Cells[row, 20].Text,
                            EarlyExit = worksheet.Cells[row, 21].Text,
                            ExtraHoursWorked = worksheet.Cells[row, 22].Text,
                            Absent = absent,
                            Present = present,
                            WeeklyHoliday = weeklyHoliday,
                            Total = total,
                            CreatedBy = employeeId,
                            CreatedAt = DateTime.UtcNow
                        };

                        newRecords.Add(report);
                    }
                }

                if (newRecords.Any())
                {
                    _context.AttendanceSheets.AddRange(newRecords);
                    _context.SaveChanges();
                }

                return Ok(new { message = "File processed successfully", newRecords });
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
