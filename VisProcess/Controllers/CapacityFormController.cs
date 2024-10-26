using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class CapacityFormController : ControllerBase
    {
        private readonly CapacityFormService _capacityFormService;// = new CapacityFormService();
        public CapacityFormController(CapacityFormService capacityFormService)
        {
            _capacityFormService = capacityFormService;// new CapacityFormService(configuration);
        }


        // ----------- Read Capacity Form ---------

        [HttpGet]
        [Route("GetCapacityForm")]
        public IActionResult GetCapacityForm()
        {
            var CapFormDetails = _capacityFormService.GetCapacityForm();
            var Details = new
            {
                CapFormList = CapFormDetails,
            };
            return Ok(Details);
        }



        // ------------ Create Capacity Form ---------

        [HttpPost]
        [Route("SetCapacityForm")]
        public IActionResult SetCapacityForm(CapFormPara CFPara)
        {
            var SCapFormDetails = _capacityFormService.SetCapacityForm(CFPara);
            var Details = new
            {
                SCapFormList = SCapFormDetails,
            };
            return Ok(Details);
        }


        //------------- Update Capacity Form ----------

        [HttpPost]
        [Route("UpdateCapacityForm")]
        public IActionResult UpdateCapacityForm(CapacityForm CFsetPara)
        {
            var EditCapFormDetails = _capacityFormService.UpdateCapacityForm(CFsetPara);
            var Details = new
            {
                EditCapFormList = EditCapFormDetails
            };
            return Ok(Details);
        }


        // ------------- Delete Capacity Form ----------

        [HttpPost]
        [Route("DeleteCapacityForm")]
        public IActionResult DeleteCapacityForm(CapFormDelPara CFdelId)
        {
            var DelCapFormDetails = _capacityFormService.DeleteCapacityForm(CFdelId);
            var Details = new
            {
                DelCapFormList = DelCapFormDetails,
            };
            return Ok(Details);
        }
    }
}
