using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingFeedbackFormController : ControllerBase
    {
        private readonly TrainingFeedbackFormService _trainingFeedbackFormService;
        public TrainingFeedbackFormController(TrainingFeedbackFormService trainingFeedbackFormService)
        {
            _trainingFeedbackFormService = trainingFeedbackFormService;
        }
        [HttpGet]
        [Route("GetEmployeeDetails")]
        public object GetEmployeeDetails(int employeeId)
        {
            try
            {
                var result = _trainingFeedbackFormService.GetEmployeeDetails(employeeId);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("GetTrainingEmployee")]
        public ActionResult GetTrainingEmployee()
        {
            try
            {
                var employees = _trainingFeedbackFormService.GetTrainingEmployee();

                if (employees == null || !((IEnumerable<object>)employees).Any())
                {
                    return NotFound(new { message = "No employees found." });
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetTrainingDetails")]
        public ActionResult<List<TrainingFeedbackForm>> GetTrainingDetails(int employeeId)
        {
            try
            {
                var employeeDetails = _trainingFeedbackFormService.GetTrainingDetails(employeeId);
                return Ok(employeeDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("CreateTrainingDetails")]
        public ActionResult<TrainingFeedbackForm> CreateTrainingDetails(TrainingFeedbackForm trainingDetails)
        {
            try
            {
                var createdEmployeeDetails = _trainingFeedbackFormService.CreateTrainingDetails(trainingDetails);
                return CreatedAtAction(nameof(GetTrainingDetails), new { employeeId = trainingDetails.EmployeeId }, createdEmployeeDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("ApproveFeedbackForm")]
        public ActionResult ApproveFeedback([FromBody] ApproveFeedbackForm approveFeedbackForm)
        {
            try
            {
                var result = _trainingFeedbackFormService.ApproveTrainingFeedbackForm(approveFeedbackForm);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("RejectFeedbackForm")]
        public ActionResult RejectFeedback([FromBody] RejectFeedbackForm rejectFeedbackForm)
        {
            try
            {
                var result = _trainingFeedbackFormService.RejectTrainingFeedbackForm(rejectFeedbackForm);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("UpdateTrainingDetails")]
        public ActionResult<TrainingFeedbackForm> UpdateTrainingDetails(TrainingFeedbackForm employeeDetails)
        {
            try
            {
                var updatedDetails = _trainingFeedbackFormService.UpdateTrainingDetails(employeeDetails);
                return Ok(updatedDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpDelete]
        [Route("DeleteTrainingDetails")]
        public IActionResult DeleteTrainingDetails(int Id)
        {
            try
            {
                _trainingFeedbackFormService.DeleteTrainingDetails(Id);
                return Ok("Deleted Successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}