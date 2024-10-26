using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingCycleMonthlyController : ControllerBase
    {
        private readonly BillingCycleMonthlyService _billingCycleMonthlyService;

        public BillingCycleMonthlyController(BillingCycleMonthlyService billingCycleMonthlyService)
        {
            _billingCycleMonthlyService = billingCycleMonthlyService;
        }


        [HttpPost("Creation")]
        public ActionResult CreateInvoiceBilling(BillingCycleMonthly bill)
        {
            try
            {
                var billingMonthly = _billingCycleMonthlyService.CreateInvoiceBilling(bill);
                return Ok(billingMonthly);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpPut("Delete")]
        public ActionResult DeleteBilling(BillingCycleMonthly billing, int id)
        {
            try
            {
                var deleteBilling = _billingCycleMonthlyService.DeleteBilling(billing, id);
                return Ok(deleteBilling);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpPut("Updation")]
        public ActionResult UpdataionBilling(BillingCycleMonthly billing, int id)
        {
            try
            {
                var update = _billingCycleMonthlyService.UpdataionBilling(billing, id);
                return Ok(billing);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        
        
       
        [HttpGet("getdata")]
        public ActionResult<List<BillingCycleMonthly>> GetData(
        [FromQuery] int? clientId,
        [FromQuery] int? departmentId,
        [FromQuery] DateTime? billingDate)
        {
            try
            {
                var data = _billingCycleMonthlyService.GetData(clientId, departmentId, billingDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}

