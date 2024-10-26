// Code for Execl to ssms.
using ExcelDataReader;
//using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

using Vis.VleadProcessV3.Models;
using VisProcess.Models;

namespace ExcelUploadApi.Controllers
{
    [Route("Api/Excel")]
    [ApiController]
    public class BenchUploadController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BenchUploadController(ApplicationDbContext dbContext)
        {
            _applicationDbContext = dbContext;
        }



        [HttpPost("upload")]
        public IActionResult UploadExcelFile([FromForm] PassExcel excel)
        {
            try
            {
                string message = "";

                if (excel.file != null)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    using (var stream = excel.file.OpenReadStream())
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var data = new List<BeanchUpload>();
                            if (result != null && result.Tables.Count > 0)
                            {
                                DataTable dtStudentRecords = result.Tables[0];
                                foreach (DataRow row in dtStudentRecords.Rows)
                                {
                                    if (row.Table.Rows.IndexOf(row) == 0)
                                        continue;

                                    string employeeName = Convert.ToString(row[2]);
                                    string type = Convert.ToString(row[3]);
                                    string divisionId = Convert.ToString(row[5]);

                                    BeanchUpload existingRecord = _applicationDbContext.BeanchUploads.FirstOrDefault(b => b.EmployeeName == employeeName && b.Type == type);
                                    if (existingRecord != null)
                                    {
                                        existingRecord.UpdateBy = excel.createdBy;
                                        existingRecord.UpdateUtc = DateTime.UtcNow;
                                        _applicationDbContext.BeanchUploads.Update(existingRecord);
                                        _applicationDbContext.SaveChanges();
                                        BeanchUpload beanchUpload = new BeanchUpload()
                                        {
                                            UpdateBy = excel.createdBy,
                                            UpdateUtc = DateTime.UtcNow
                                        };
                                        continue;
                                    }

                                    BeanchUpload objStudent = new BeanchUpload
                                    {
                                        Id = Convert.ToInt32(row[0]),
                                        EmployeeId = Convert.ToInt32(row[1]),
                                        EmployeeName = Convert.ToString(row[2]),
                                        Type = Convert.ToString(row[3]),
                                        DivisionId = Convert.ToInt32(row[4]),
                                        ShiftId = Convert.ToInt32(row[5]),
                                        Fresh = Convert.ToInt32(row[6]),
                                        Revision = Convert.ToInt32(row[7]),
                                        Qc = Convert.ToInt32(row[8]),
                                        AchievedCount = Convert.ToInt32(row[9]),
                                        ProductionHeadCount = Convert.ToInt32(row[10]),
                                        OtherTeamBenchDeployed = Convert.ToInt32(row[11]),
                                        DeployedDivisionName = Convert.ToString(row[12]),
                                        InternalOrCrossTrainingHeadCount = Convert.ToInt32(row[13]),
                                        CreatedUtc = DateTime.UtcNow,
                                        UpdateUtc = DateTime.UtcNow,
                                        CreatedBy = excel.createdBy,
                                        UpdateBy = excel.createdBy,
                                        CommentsForInternalTraining = Convert.ToString(row[14])
                                    };

                                    data.Add(objStudent);
                                }
                                _applicationDbContext.AddRange(data);
                                int output = _applicationDbContext.SaveChanges();
                                if (output > 0)
                                    message = "The Excel file has been successfully uploaded.";
                                else
                                    message = "The Excel file is already uploaded.";
                            }
                            else
                                message = "Selected file is empty.";
                        }
                    }
                }
                else
                {
                    message = "Invalid File.";
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}