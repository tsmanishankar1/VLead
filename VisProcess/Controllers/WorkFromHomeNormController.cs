using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet.Messages;
using System.Drawing.Drawing2D;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFromHomeNormController : ControllerBase
    {
        private readonly WorkFromHomeNormService _service;
        private readonly ApplicationDbContext _context;
        public WorkFromHomeNormController(WorkFromHomeNormService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

/*        [HttpGet("GetEffectiveNorm")]
        public IActionResult GetEffectiveNorm(int employeeId)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId && e.IsDeleted == false);
                if (employee == null) return NotFound("Employee not found.");

                var norm = _service.GetEffectiveNorm(employee);
                if (norm == null) return NotFound("Norm not found. Please update the norm.");

                return Ok(norm);
            }
            catch(Exception  ex)
            {
                return Ok(ex.Message);
            }
        }
*/
        [HttpPost("CreateNorm")]
        public IActionResult CreateNorm(CreateNormRequest request)
        {
            try
            {
                var createNorm = _service.AddNorm(request);
                var response = new
                {
                    Success = true,
                    Message = "Norm created successfully."
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
        [HttpGet("GetWorkFromHomeNorm")]
        public IActionResult GetWorkFromHomeNorm()
        {
            try
            {
                var norm = _service.GetWorkFromHomeNorm();

                var response = new
                {
                    Success = true,
                    Message = norm
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

        [HttpPut("UpdateEmployeeCategory")]
        public IActionResult UpdateEmployeeCategory(EmployeeCategoryChangeRequest request)
        {
            try
            {
                _service.UpdateEmployeeCategory(request);
                var response = new
                {
                    Success = true,
                    Message = "Norm updated successfully."
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
