using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTrackController : ControllerBase
    {
        private readonly LeaveTrackService _service;
        public LeaveTrackController(LeaveTrackService service)
        {
            _service = service;
        }

        [HttpPost("AddLeave")]
        public IActionResult AddLeave(LeaveMasterRequest addLeave)
        {
            try
            {
                var leaveRequest = _service.AddLeave(addLeave);
                var response = new
                {
                    Success = true,
                    Message = "Leave added successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Success = false,
                    Message = "Leave added failed."
                };
                return Ok(response);
            }
        }

        [HttpGet("GetAllLeaves")]
        public IActionResult GetAllLeaves()
        {
            try
            {
                var leaves = _service.GetAllLeaves();
                return Ok(leaves);
            }
            catch (MessageNotFoundException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


            [HttpPost("LeaveRequest")]
        public IActionResult LeaveRequest(SubmitLeave submitLeave)
        {
            try
            {
                var leaveRequest = _service.LeaveRequest(submitLeave);
                var response = new
                {
                    Success = true,
                    Message = "Leave request submitted successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Success = false,
                    Message = "Leave request submit failed."
                };
                return Ok(response);
            }
        }

        [HttpPost("GetEmployeeLeaves")]
        public IActionResult GetEmployeeLeaves(GetLeave getLeave)
        {
            try
            {
                var leaves = _service.GetEmployeeLeaves(getLeave);
                return Ok(leaves);
            }
            catch (MessageNotFoundException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPut("UpdateLeave")]
        public IActionResult UpdateLeave(UpdateLeave updateLeave)
        {
            try
            {
                var updatedLeave = _service.UpdateLeave(updateLeave);
                var response = new
                {
                    Success = true,
                    Message = "Leave updated successfully."
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

        [HttpDelete("DeleteLeave")]
        public IActionResult DeleteLeave(int id)
        {
            try
            {
                var report = _service.DeleteLeave(id);
                var response = new
                {
                    Success = true,
                    Message = "Leave deleted successfully."
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
    }
}
