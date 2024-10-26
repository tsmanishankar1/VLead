using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        private readonly WorkflowService _workflowService;
        public WorkflowController(WorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        [Route("Traymovement")]
        public Object BulkTrayMovement(ProcessTransaction SelectedRows)
        {
            var httpRequest = HttpContext.Request;
            return _workflowService.BulkClosureNextTraymovement();
        }

        [HttpGet]

        [Route("GetProcessTransaction/{WFTId}/{EmployeeId}")]
        public Object GetProcessTransactionByWFTId(int WFTId, int EmployeeId)
        {
            return _workflowService.GetProcessTransactionByWftid(WFTId, EmployeeId);
        }

        [HttpPost]
        [Route("GetProductionWorkList")]
        public Object GetProductionWorkList(ProcessTransaction processTransaction)
        {
            var employeeAssign = _workflowService.GetProductionWorkList(processTransaction);
            var summary = _workflowService.GetProductionWorkSummaryList(processTransaction);
            var result = new
            {
                JobHistory = employeeAssign,
                Summary = summary
            };

            return result;
        }

        //=====================================final CL======================================================
        [HttpGet]
        [Route("ChecklistPopup")]
        public Object ChecklistPopup(int WFMId)
        {
            var result = _workflowService.ChecklistPopup(WFMId);
            var checklist = new
            {
                check = result
            };
            return checklist;
        }

        //=====================================final CL======================================================
        [HttpPost]
        [Route("ChangeWorkflow/{WFTId}")]
        public async Task<Object> ChangeWorkflow(int WFTId)
        {
            var payload = JsonConvert.DeserializeObject<ProcessTransaction>(HttpContext.Request.Form["data"]);
            var httpRequest = HttpContext.Request;
            return await _workflowService.ChangeWorkflow(httpRequest.Form.Files, payload);
        }
        [HttpPost]
        [Route("processMovement")]
        public Object ChangeWorkflowBulk()
        {
            var SelectedRows = JsonConvert.DeserializeObject<ProcessTransaction>(HttpContext.Request.Form["data"]);
            var httpRequest = HttpContext.Request;
            return _workflowService.ChangeWorkflowBulk(httpRequest.Form.Files, SelectedRows);
        }
        [HttpGet]
        [Route("GetErrorCategories/{WFTId}/{EmployeeId}")]
        public Object GetErrorCategories(int WFTId, int EmployeeId)
        {
            var getErrorCategories = _workflowService.GetErrorCategories(WFTId, EmployeeId);
            var result = new
            {
                errorCategories = getErrorCategories
            };
            return result;
        }
    }
}
