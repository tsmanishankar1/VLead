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
    public class CustomerVsProcessController : ControllerBase
    {
        private readonly CustomerVsProcessService _customerVsProcessService;
        public CustomerVsProcessController(CustomerVsProcessService customerVsProcessService)
        {
            _customerVsProcessService = customerVsProcessService;
        }
        [HttpGet]
        [Route("GetAllddlList")]
        public Object ddlDetailList()
        {
            return _customerVsProcessService.GetAllDropDownList();
        }
        [HttpGet]
        [Route("CustomerVsProcess")]
        public IEnumerable<ViewCustomerVsProcess> GetCustomerVsProcess()
        {
            return _customerVsProcessService.GetCustomerVsProcess();
        }
        [HttpGet]
        [Route("GetEditCustomerVsProcess")]
        public IActionResult GetEditCustomerVsProcess( int Id)
        {
            var editCustVsProcess = _customerVsProcessService.GetEditCustomerVsProcess(Id);
            var processworkflow = new ProcessWorkFlow
            {
                Id = editCustVsProcess.Id,
                DepartmentId = editCustVsProcess.DepartmentId,
                CustomerId = editCustVsProcess.CustomerId,
                JobStatusId = editCustVsProcess.JobStatusId,
                CustomJobType = editCustVsProcess.CustomJobType,
                CurrentProcessId = editCustVsProcess.CurrentProcessId,
                StatusId = editCustVsProcess.StatusId,
                NextProcessId = editCustVsProcess.NextProcessId,
                ScopeId = editCustVsProcess.ScopeId
            };
            if (processworkflow != null)
            {
                return Ok(processworkflow);
            }
            else
            {
                return NotFound("Process Flow Not Available");
            }
           
        }
        [HttpPost]
        [Route("AddProcessworkflow")]
        public IActionResult AddProcessworkflow( AddProcessWorkFlow Customerandprocess)
        {
            var addCustomerVsProcess = _customerVsProcessService.AddProcessworkflow(Customerandprocess);
            var Getresult = new
            {
                Message = "Process flow has been added successfully",
            };
            var GetResult = new
            {
                Message = "Already process flow defined..!",
            }; if (addCustomerVsProcess)
            {
                return Ok(Getresult);
            }
            else
            {
                return Ok(GetResult);
            }
           
        }
        [HttpPost]
        [Route("UpdateProcessworkflow")]
        public IActionResult UpdateProcessworkflow( [FromBody] ProcessWorkFlow Customerandprocess)
        {
            var updateCustomerVsProcess = _customerVsProcessService.UpdateProcessworkflow(Customerandprocess);
            if (updateCustomerVsProcess != null)
            {
                return Ok(updateCustomerVsProcess);
            }
            else
            {
                return StatusCode(500, "Unable to Update Customer Vs Employee");
            }
           
        }
        [HttpGet]
        [Route("DeleteProcessworkflow")]
        public IActionResult DeleteProcessworkflow(int Id, int EmployeeId)
        {
            var deleteProcessworkflow = _customerVsProcessService.DeleteProcessworkflow(Id, EmployeeId);
            if (deleteProcessworkflow)
            {
                return Ok(deleteProcessworkflow);
            }
            else
            {
                return StatusCode(500, "Unable to Update Customer Vs Employee");
            }
            
        }
        [HttpGet]
        [Route("GetStatusddlList")]
        public IEnumerable<Status> GetStatusddlList()
        {
            return _customerVsProcessService.GetStatusddlList();
        }
        [HttpPost]
        [Route("GetScopeList")]
        public Object GetScopeList( CustomerVsScope1 getdeptid)
        {
            var GetScopeId = _customerVsProcessService.GetScopeList(getdeptid);
            var Getscopeid = new
            {
               GetScopeList = GetScopeId,
            };

            return Getscopeid;
        }
        [HttpPost]
        [Route("GetNextProcessddlList")]
        public Object GetNextProcessddlList(Process1 getnextprocess)
        {
            var GetNextProcess = _customerVsProcessService.GetNextProcessddlList(getnextprocess);
            var Getprocess = new
            {
                GetNextProcessList = GetNextProcess,
            };
            return Getprocess;
        }
        [HttpPost]
        [Route("GetCustomerddlList")]
        public Object GetCustomerddlList(ProcessWorkFlow1 getdeptid)
        {
            var Getcustomer = _customerVsProcessService.GetCustomerddlList(getdeptid);
            var Getprocess = new
            {
                CustomerList = Getcustomer,
            };
            return Getprocess;
        }
        [HttpPost]
        [Route("SaveCustomerVsProcess")]
        public IActionResult SaveCustomerVsProcess( [FromBody] SaveCustomerVsProcessViewModel customervsprocess)
        {
            var saveCustomervsprocess = _customerVsProcessService.SaveCustomerVsProcess(customervsprocess);
            if(saveCustomervsprocess )
            {
                return Ok(saveCustomervsprocess);
            }
            else
            {
                return StatusCode(500, "Unable to Create Customer Vs Employee");
            }
           
        }
        [HttpGet]
        [Route("GetCustomerVsProcessById")]
        public IActionResult GetCustomerVsProcessById( int Id)
        {
            var CustomerprocessDetails = _customerVsProcessService.GetCustomerVsProcessById(Id);
            if (CustomerprocessDetails != null)
            {
                return Ok(CustomerprocessDetails);
            }
            else
            {
                return NotFound("Error");
            }
          
        }
    }
}
