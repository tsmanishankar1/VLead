using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly VoucherService _voucherService;
        public VoucherController(VoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        [HttpGet]
        [Route("GetAllVoucher")]
        public IActionResult GetAllVoucherList()
        {
            var VoucherList = _voucherService.GetAllVoucherList();
            if(VoucherList == null)
            {
                return NotFound("Voucher Control is Not Available");
            }
            else
            {
                return Ok(VoucherList);
            }
          
        }
        [HttpPost]
        [Route("Addvoucher")]
        public IActionResult CreateVoucherList( [FromBody] VoucherControl voucherControl)
        {
            var createVoucherList = _voucherService.CreateVoucherList(voucherControl);
            if (!createVoucherList)
            {
                return StatusCode(500, "Unable to get Voucher Fields");
            }
            else
            {
                return Ok(createVoucherList);
            }
           
        }
        [HttpGet]
        [Route("GetVoucherDetails")]
        public IActionResult GetVoucherDetails(int id)
        {
            var GetVoucherDetails = _voucherService.GetVoucherDetails(id);
            if (GetVoucherDetails == null)
            {
                return StatusCode(500, "Unable to get Voucher Fields");
            }
            else
            {
                return Ok(GetVoucherDetails);
            }
           
        }
        [HttpPost]
        [Route("Update-Voucher")]
        public IActionResult UpdateVoucher([FromBody] VoucherControl voucherControl)
        {
            var EditVoucher = _voucherService.UpdateVoucher(voucherControl);
            if (!EditVoucher)
            {
                return StatusCode(500, "Unable to get Voucher Fields");
            }
            else
            {
                return Ok(EditVoucher);
            }
          
        }
        [HttpGet]
        [Route("Delete-Voucher")]
        public IActionResult RemoveVoucher( int id)
        {
            var DeleteVoucher = _voucherService.RemoveVoucher(id);
            if (!DeleteVoucher)
            {
                return NotFound("Voucher is Not Available");
            }
            else
            {
                return Ok(DeleteVoucher);
            }
          
        }
        [HttpGet]
        [Route("Transaction")]
        public IActionResult GetAllTransaction()
        {
            var AllTransaction = _voucherService.GetAllTransaction();
            if (AllTransaction == null)
            {
                return NotFound("Voucher is Not Available");
            }
            else
            {
                return Ok(AllTransaction);
            }
           
        }
    }
}
