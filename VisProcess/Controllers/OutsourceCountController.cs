using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutsourceCountController : ControllerBase
    {
        private readonly OutsourceCountService _service;

        public OutsourceCountController(OutsourceCountService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("CreateOutsourceCount")]
        public ActionResult CreateOutsourceCount(OutsourceCount outsourceCount)
        {
            try
            {
                var createdOutsourceCountDetails = _service.CreateOutsourceCount(outsourceCount);

                return Ok(createdOutsourceCountDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetAllOutsourceCount")]
        public ActionResult GetAll()
        {
            try
            {
                var results = _service.GetAllOutsourceCounts();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("UpdateOutsourceCount")]
        public ActionResult UpdateOutsourceCount(OutsourceCount outsourceCount)
        {
            try
            {
                var result = _service.UpdateOutsourceCount(outsourceCount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the outsource count.");
            }
        }
        [HttpDelete]
        [Route("DeleteOutsourceCount/{id}")]
        public ActionResult DeleteOutsourceCount(int id)
        {
            try
            {
                var result = _service.DeleteOutsourceCount(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the outsource count.");
            }
        }
    }
}