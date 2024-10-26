using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly UnitWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public TransferController(IConfiguration configuration, UnitWork unitWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbForMail)
        {
            _unitofwork = unitWork;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _context = dbForMail;
        }
        [HttpGet]
        [Route("File")]
        public async Task<IActionResult> Post()
        {
            //ftp://207.244.104.74:22/ClientOrders/24060/
            var clientOrder = _unitofwork.ClientOrder.Where(x => x.IsDeleted == false && x.IsCompleted == false && x.IsFileUploaded == true && x.FileUploadPath != null && x.IsConverted == false && x.IsFileTransfer != true).ToList();

            foreach (var order in clientOrder)
            {
                var filePath = order.FileUploadPath.Replace("\\", "/");
                var files = GetFilesFromServer(filePath);
                var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", filePath);//HttpContext.Current.Server.MapPath(@"~/JobFiles/" + filePath);
                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }
                foreach (var file in files)
                {
                    try
                    {
                        var checkedFileExist = System.IO.File.Exists(localPath + "\\" + file);
                        var url = _configuration.GetSection("AllocationService").GetValue<string>("USserverUrl");
                        var ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUrl"); 
                        var ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUsername");
                        var ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerPassword");
                        if (!checkedFileExist)
                        {

                            byte[] data;

                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl +
                                //"JobFiles/" +
                                filePath + "/" + file);
                            request.Method = WebRequestMethods.Ftp.DownloadFile;

                            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                            Stream responseStream = response.GetResponseStream();
                            //StreamReader reader = new StreamReader(responseStream);
                            //string temp = reader.ReadToEnd();
                            //data = Encoding.UTF8.GetBytes(temp);

                            using (var bufferedStream = new BufferedStream(responseStream))
                            {
                                using var memoryStream = new MemoryStream();
                                bufferedStream.CopyTo(memoryStream);
                                data = memoryStream.ToArray();
                            }

                            System.IO.File.WriteAllBytes(localPath + "\\" + file, data);
                           // reader.Close();
                            response.Close();



                            //var _httpClient = new HttpClient();
                            //var url = _configuration.GetSection("AllocationService").GetValue<string>("USserverUrl");//ConfigurationManager.AppSettings["USserverUrl"];
                            //var response = await _httpClient.GetAsync(url + "/JobFiles/" + filePath + "/" + file);

                            //    response.EnsureSuccessStatusCode(); 
                            // data = await response.Content.ReadAsByteArrayAsync(); // Do something with the downloaded data (e.g., save to a file or process it) }

                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
                var updateclientorder = _unitofwork.ClientOrder.FirstOrDefault(x => x.OrderId == order.OrderId);
                if (updateclientorder != null)
                {
                    updateclientorder.IsFileTransfer = true;
                    _unitofwork.ClientOrder.Update(updateclientorder);
                    _unitofwork.SaveChanges();
                }
            }
            return Ok();

        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private List<string> GetFilesFromServer(string FilePath)
        {
            var ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUrl"); //ConfigurationManager.AppSettings["ftpClientServerUrl"];
            var ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUsername");//ConfigurationManager.AppSettings["ftpClientServerUsername"];
            var ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerPassword");//ConfigurationManager.AppSettings["ftpClientServerPassword"];
            string names = "";
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + FilePath + "/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                names = reader.ReadToEnd();

                reader.Close();
                response.Close();

               
            }
            catch (Exception ex)
            {
               
                Log addlog = new Log();
                addlog.Module = "Transfer File from US to Indian - ";
                addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                addlog.Type = "Error Occured";
                addlog.CreatedUtc = DateTime.UtcNow;
                _context.Logs.Add(addlog);
                _context.SaveChanges();
               
            }
            return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
