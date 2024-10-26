using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO.Compression;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
       
        public HomeController(
            IWebHostEnvironment webHostEnvironment)
        {
            
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("DownloadZipFile")]
        public IActionResult DownloadZipFile(string path)
        {
            path = path.Replace('_', '\\');

            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", path + '\\');//Server.MapPath("~/JobFiles/" + path + "/");
            var selectedfiles = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
            var timeStamp = DateTime.Now.Ticks;
            var tempPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles\\Zip", timeStamp + ".zip");
            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }
            ZipArchive zip = ZipFile.Open(tempPath, ZipArchiveMode.Create);

            foreach (string file in selectedfiles)
            {
                zip.CreateEntryFromFile(folderPath + file, file);
            }
            zip.Dispose();
            IFileProvider provider = new PhysicalFileProvider(Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles\\Zip"));
            IFileInfo fileInfo = provider.GetFileInfo(timeStamp + ".zip");
            var readStream = fileInfo.CreateReadStream();
            return File(readStream, "application/zip", timeStamp + ".zip");
        }
    }
}
