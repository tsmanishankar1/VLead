using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeFileUploadController : ControllerBase
    {
        private readonly EmployeeFileUploadService _service;

        public EmployeeFileUploadController(EmployeeFileUploadService service)
        {
            _service = service;
        }

        [HttpPost("EmployeeFileUpload")]
        public IActionResult EmployeeFileUpload(string employeeCode, string employeeName, [FromForm] EmployeeFileUpload upload)
        {
            try
            {
                string resultMessage = _service.EmployeeFileUpload(employeeCode, employeeName, upload);
                var response = new
                {
                    Success = true,
                    Message = resultMessage
                };
                return Ok(response);
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

        [HttpGet("DownloadFile")]
        public IActionResult DownloadFile(string employeeCode, string fileName)
        {
            try
            {
                var fileResult = _service.DownloadFile(employeeCode, fileName);
                return fileResult;
            }
            catch (FileNotFoundException ex)
            {
                return Ok(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while downloading the file.");
            }
        }

        [HttpGet("ViewUploadedFiles")]
        public IActionResult ViewUploadedFiles()
        {
            try
            {
                var files = _service.ViewUploadedFiles();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching files.");
            }
        }
        [HttpDelete("EmployeeFileDelete")]
        public IActionResult EmployeeFileDelete(EmployeeFileDelete fileDelete, string employeeCode, string fileName)
        {
            try
            {
                string resultMessage = _service.EmployeeFileDelete(fileDelete, employeeCode, fileName);
                var response = new
                {
                    Success = true,
                    Message = resultMessage
                };
                return Ok(response);
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
