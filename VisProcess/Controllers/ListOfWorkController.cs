using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/ ListofWork")]
    [ApiController]
    public class ListOfWorkController : ControllerBase
    {
        private readonly ListofWorkService _reportService ;
        public ListOfWorkController(ListofWorkService listofWorkService)
        {
            _reportService = listofWorkService;
        }

        // GET: ListofWork
        [HttpGet]
        [Route("GetExpiredOrders/{listofworkinfo}")]
        public Object GetExpiredOrders(string listofworkinfo)
        {
            var expiredorders = _reportService.GetExpiredOrders(listofworkinfo);
            var Expiredorders = new
            {
                Expiredorders = expiredorders,
            };
            return Expiredorders;
        }
    }
}
