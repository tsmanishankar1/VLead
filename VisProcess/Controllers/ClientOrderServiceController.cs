using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientOrderServiceController : ControllerBase
    {
        private readonly ClientOrderService _clientOrderService;
        public ClientOrderServiceController(ClientOrderService clientOrderService)
        {
            _clientOrderService = clientOrderService;
        }
        [HttpGet]
        [Route("GetJobOrderByJobId")]//hited
        public IActionResult GetJobOrderByJobId( string JobId)
        {
            var orderDetails = _clientOrderService.GetJobOrderOrder(JobId);
            if (orderDetails == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(orderDetails);
            }
           
        }
        [HttpPost]
        [Route("QueryDetailspost")]//hited
        public Object QueryDetailspost(Jobqueryfortesting gjobquery)
        {
            var orderDetails = _clientOrderService.QueryDetailspost(gjobquery);
            var DetailList = new
            {
                Querylist = orderDetails,
            };
            return DetailList;
        }
        [HttpGet]
        [Route("QueryDetails")]//hited
        public IActionResult QueryDetails( int WFTId, int WFMId)
        {
            var orderDetails = _clientOrderService.QueryDetails(WFTId, WFMId);
            if (orderDetails == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(orderDetails);
            }
           
        }

        [HttpGet]
        [Route("QuotationDetails")]//hited
        public IActionResult QuotationDetails( string JobId)
        {
            var orderDetails = _clientOrderService.QuotationDetails(JobId);
            if (orderDetails == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(orderDetails);
            }
           
        }
        [HttpGet]
        [Route("getDepartmentsForJO")]
        public IActionResult GetDepartmentsForJO()
        {
            var clientOrder = _clientOrderService.GetDepartmentsForJO();
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
           
        }
        [HttpGet]
        [Route("nGetDivisionForJO")]
        public IActionResult GetDivisionForJO()
        {
            var clientOrder = _clientOrderService.GetDivisionForJO();
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
          
        }

        [HttpGet]
        [Route("CCByCusId")]
        public IActionResult GetCCByCusId(int custId)
        {
            var CCList = _clientOrderService.GetCustomerContactsForJO(custId);
            if (CCList == null)
            {
                return NotFound("Scope Not Available");
            }
            else
            {
                return Ok(CCList);
            }
           
        }

        [HttpGet]
        [Route("getFileInwardTypeListForJO")]
        public IActionResult GetFileInwardTypeListForJO()
        {
            var clientOrder = _clientOrderService.GetFileInwardTypeListForJO();
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
          
        }
        [HttpGet]
        [Route("getJobStatusForJO")]
        public IActionResult GetJobStatusForJO()
        {
            var clientOrder = _clientOrderService.GetJobStatusForJO();
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
            
        }
        [HttpGet]
        [Route("getCustomersForJO")]
        public IActionResult GetCustomersForJO()
        {
            var clientOrder = _clientOrderService.GetCustomersForJO();
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
          
        }
        [HttpGet]
        [Route("ClientOrdersExts/{actionType}")]
        public IActionResult GetAllClientOrderExt( int actionType)
        {
            var result = _clientOrderService.GetAllClientOrderExt(actionType);
            var clientOrder = new
            {
                Data = result
            };
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
           
        }
        [HttpGet]
        [Route("ClientOrdersCount/{actionType}")]
        public IActionResult GetAllClientOrderCounts(int actionType)
        {
            var result = _clientOrderService.GetAllClientOrderCounts(actionType);
            var clientOrder = new
            {
                Count = result
            };
            if (clientOrder == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(clientOrder);
            }
           
        }
        [HttpGet]
        [Route("GetClientByOrderId/{needFileCopy}")]//hited
        public IActionResult GetClientOrder( int orderId, int needFileCopy)
        {
            var orderDetails = _clientOrderService.GetClientOrder(orderId, needFileCopy);
            if (orderDetails == null)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(orderDetails);
            }
           
        }
        [HttpPost]
        [Route("GetAllApproval")]
        public Object AllApprovalListforSalesTrack(ApprovalViewModel Approval)
        {
            var ApprovalInfo = _clientOrderService.AllApprovalListforSalesTrack(Approval);
            var result = new
            {
                ApprovalDetails = ApprovalInfo
            };
            return result;
        }
        //
        [HttpPost]
        [Route("GetFeedbackReport")]//hited
        public Object GetClientFeedbackReport(CustomerFeedbackViewModel GetFeedback)
        {
            var FeedbackDetails = _clientOrderService.GetCustomerFeedbackDetails(GetFeedback);
            var result = new
            {
                FeedbackDetailsList = FeedbackDetails,
            };
            return result;
        }
        //

        [HttpGet]
        [Route("CreditApprovalBySales")]//hited
        public Object GetCreditApprovalBySales( int JobApprovalExtnId, int IsApprove, int EmployeeId)
        {
            var CreditApprovalDetails = _clientOrderService.GetCreditApprovalBySalesProcess(JobApprovalExtnId, IsApprove, EmployeeId);
            var result = new
            {
                CreditApprovalDetailsList = CreditApprovalDetails,
            };
            return result;
        }
    }
}
