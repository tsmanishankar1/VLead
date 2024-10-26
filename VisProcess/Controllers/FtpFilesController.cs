using Microsoft.AspNetCore.Mvc;
using System.IO;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FtpFilesController : ControllerBase
    {
        private readonly FtpFilesService _service;

        public FtpFilesController(FtpFilesService service)
        {
            _service = service;
        }

        [HttpPost("FtpFileAttachments")]
        public IActionResult FtpFileAttachments(FtpFileServiceModel ftpFileService)
        {
            try
            {
                var files = _service.FtpFileAttachments(ftpFileService);
                return Ok(files);
            }
            catch(MessageNotFoundException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
