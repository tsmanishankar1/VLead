using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/Inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _InventoryService;// = new InventoryService();
        public InventoryController(InventoryService inventoryService)
        {
            _InventoryService = inventoryService;// new InventoryService(configuration);

        }

        [HttpGet]
        [Route("getInventory")]//hited
        public Object GetInventory([FromQuery] InventoryViewModel jobInventory)
        {
            return _InventoryService.GetInventory(jobInventory);
        }

        [HttpPost]
        [Route("submitInventory")]//hited
        public Object SubmitInventory([FromBody] InventorySubmitPara jobInventorySubmit)
        {
            return _InventoryService.SubmitInventory(jobInventorySubmit);
        }
    }
}
