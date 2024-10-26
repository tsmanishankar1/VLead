using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using Vis.VleadProcessV3.Models;
using System.Security.Policy;

namespace Vis.VleadProcessV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyBenchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DailyBenchController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("upload")]
        public ActionResult UploadExcel(IFormFile file, int divisionId, DateTime date, int createdBy)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var division = _context.Divisions.FirstOrDefault(d => d.Id == divisionId);
            if (division == null)
            {
                var result1 = new
                {
                    success = false,
                    message = $"Division with ID {divisionId} does not exist."
                };

                return Ok(result1);
            }
            var divisionName = division.DivisionName;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var employeeCode = worksheet.Cells[row, 1].Text;
                        var dateExists = _context.DailyBenches.Any(d =>
                            (d.Date.Date == date.Date && d.DivisionId == divisionId && d.EmployeeCode == employeeCode) && d.IsDeleted == false);

                        if (dateExists)
                        {
                            var result1 = new
                            {
                                success = false,
                                message = "Records for EmployeeCode and Division and Date already exist. Upload failed."
                            };
                            return Ok(result1);
                        }
                        var newUpload = new DailyBench
                        {
                            EmployeeCode = employeeCode,
                            EmployeeName = worksheet.Cells[row, 2].Text,
                            Client = worksheet.Cells[row, 3].Text,
                            Shift = worksheet.Cells[row, 4].Text,
                            Fresh = worksheet.Cells[row, 5].Text,
                            Revision = int.TryParse(worksheet.Cells[row, 6].Text, out int revision) ? (int?)revision : null,
                            QC = int.TryParse(worksheet.Cells[row, 7].Text, out int qc) ? (int?)qc : null,
                            BenchInMins = worksheet.Cells[row, 8].Text,
                            Remarks = worksheet.Cells[row, 9].Text,
                            Bench = decimal.TryParse(worksheet.Cells[row, 10].Text, out decimal bench) ? (decimal?)bench : null,
                            Leave = decimal.TryParse(worksheet.Cells[row, 11].Text, out decimal leave) ? (decimal?)leave : null,
                            Prod = decimal.TryParse(worksheet.Cells[row, 12].Text, out decimal prod) ? (decimal?)prod : null,
                            DivisionId = divisionId,
                            DivisionName = divisionName,
                            Date = date,
                            CreatedBy = createdBy,
                            CreatedUTC = DateTime.UtcNow,
                            IsDeleted = false
                        };

                        _context.DailyBenches.Add(newUpload);
                    }
                    _context.SaveChanges();
                }
            }
            var result = new
            {
                success = true,
                message = "File uploaded successfully."
            };
            return Ok(result);
        }
    }
}