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
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsController(NewsService newsService,IWebHostEnvironment webHostEnvironment)
        {
            _newsService = newsService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("GetAllNewsList")]//hited
        public IActionResult GetAllNewsList()
        {
            var NewsList = _newsService.GetAllNewsList();
            if (NewsList==null)
            {
                return NotFound("News Not Available");
            }
            else
            {
                return Ok(NewsList);
            }
           
        }
        [HttpPost]
        [Route("CreateNews")]
        public object CreateNews([FromBody] News news)
        {
            var CreateNewsList = _newsService.CreateNewsList(news);
            var result = new
            {
                Id = CreateNewsList
            };
            return result;
        }
        [HttpPost]
        [Route("UpdateNews")]
        public object UpdateNews([FromBody] News news)
        {
            var updateNewslist = _newsService.UpdateNews(news);
            var result = new
            {
                Id = updateNewslist
            };
            return result;
        }
        [HttpGet]
        [Route("GetNewsDetails")]
        public IActionResult GetNewsDetails(int Id)
        {
            var GetNewsDetails = _newsService.GetNewsDetails(Id);
            if (GetNewsDetails != null)
            {
                return Ok(GetNewsDetails);
            }
            else
            {
                return StatusCode(500, "Unable to get Voucher Details");
            }
           
        }
        [HttpGet]
        [Route("Approval-News")]
        public IActionResult ApprovalNews( int id)
        {
            var ApprovalNews = _newsService.NewsApproval(id);
            if (!ApprovalNews)
            {
                return NotFound("News Approval is Not Available");
            }
            else
            {
                return Ok(ApprovalNews);
            }
            
        }
        [HttpPost]
        [Route("AssociateFileUpload")]
        public object AssociateFileUpload( int Id)
        {
            string Message, fileName, folderPath;
            Message = fileName = folderPath = string.Empty;
            bool status = false;
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files != null)
            {
                for (int i = 0; i < httpRequest.Form.Files.Count; i++)
                {
                    var file = httpRequest.Form.Files[i];
                    fileName = Id + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                    try
                    {
                        folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "/NewsImages", @"\");//String.Concat(HttpContext.Current.Server.MapPath("~/NewsImages"), @"\");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        var memoryStream = new MemoryStream();
                        file.CopyToAsync(memoryStream);
                        FileStream fileStream = new FileStream(folderPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                         fileStream.WriteAsync(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);

                        status = _newsService.CreateNewsImage(Id, fileName, folderPath);
                    }
                    catch (Exception)
                    {
                        Message = "File upload failed! Please try again";
                    }
                }
            }
            return status;
        }
    }
}
