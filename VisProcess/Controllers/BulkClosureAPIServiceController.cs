using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class BulkClosureAPIServiceController : ControllerBase
    {
    
      
        private readonly IConfiguration _configuration;
        private readonly WorkflowService _workflowService;
        public BulkClosureAPIServiceController(IConfiguration configuration,WorkflowService workflowService)
        {
            _configuration = configuration;
            _workflowService = workflowService;



        }
        [HttpGet]
        [Route("Movement")]
        public IActionResult GetBC()
        {
           
            _workflowService.BulkClosureNextTraymovement();
            return Ok();
        }
        //
        [HttpGet]
        [Route("Zip")]
        public IActionResult DeleteZip()
        {
            DirectoryInfo info = new DirectoryInfo(_configuration.GetSection("Workflow").GetValue<string>("zipFolderPath")/*ConfigurationManager.AppSettings["zipFolderPath"]*/);
            var delayTime = Convert.ToInt32(_configuration.GetSection("Workflow").GetValue<string>("deleteZipInterval")/*ConfigurationManager.AppSettings["deleteZipInterval"]*/);
            FileInfo[] files = info.GetFiles().Where(x => x.CreationTime < DateTime.Now.Subtract(new TimeSpan(0, delayTime, 0))).OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                if ((System.IO.File.Exists(file.FullName)))
                {
                    System.IO.File.Delete(file.FullName);
                }
            }
            return Ok();
        }
    }
}
