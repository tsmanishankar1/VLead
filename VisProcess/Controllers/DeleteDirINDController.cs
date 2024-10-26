
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Vis.VleadProcessV3.Repositories;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/FTP")]
    [ApiController]
    public class DeleteDirINDController : ControllerBase
    {
        private string host = null;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private readonly UnitWork _unitofwork;
        private readonly IConfiguration _configuration;
        public DeleteDirINDController(IConfiguration configuration,UnitWork unitWork)
        {
            _configuration = configuration;
             _unitofwork = unitWork;
    }
        [HttpGet]
        [Route("DeleteDirIND")]
        public IActionResult Post()
        {
            host = _configuration.GetSection("DeleteDirectory").GetValue<string>("DeleteDirINDHost");//ConfigurationManager.AppSettings["DeleteDirINDHost"];
            user = _configuration.GetSection("DeleteDirectory").GetValue<string>("DeleteDirINDUser");//ConfigurationManager.AppSettings["DeleteDirINDUser"];
            pass = _configuration.GetSection("DeleteDirectory").GetValue<string>("DeleteDirINDPass");//ConfigurationManager.AppSettings["DeleteDirINDPass"];
            //string tmppath = "";

           DateTime Datetobetaken = DateTime.UtcNow.AddDays(-21).Date;

            var JobsTobeDeleted = _unitofwork.JobOrder.Where(x => x.IsDeleted == false && x.DateofUpload != null && x.DateofDownload.Value.Date == Datetobetaken.Date).ToList();

            foreach (var jobs in JobsTobeDeleted)
            {
                if (jobs.FileUploadPath != null)
                {
                    string clientpath = jobs.FileUploadPath.Replace("\\", "/");
                    //*............Deleting Files/Directory from JobFiles/ClientOrder............*//
                    //string clientpath = "ClientOrders/20185";//sample

                    deleteDirectory(clientpath);

                    //...............................// 
                }
            }
            foreach (var item in JobsTobeDeleted)
            {
                var JobFiletobedeleted = _unitofwork.ProcessWorkFlowTran.FirstOrDefault(x => x.Jid == item.Id && x.PreviousProcessId == 1 && x.ProcessId == null && x.StatusId == 12);
                string path = JobFiletobedeleted.FileUploadPath.Replace("\\", "/");
                //*............Deleting Files/Directory from JobFiles.............*//
                //string path = "PRAS/05-28-2016/2805a 2018 direct-VLA-Fr-0528-13483/Client Coordination/Pending-1";//sample
                string[] year = path.Split(new char[] { '/' });
                string PathQuarter = String.Join("/", year, 0, 2);
                string staticpath = PathQuarter + "/";
                //string[] mFileNameNew = path.Split('/');
                //tmppath = mFileNameNew[mFileNameNew.Length - 1];
                deleteDirectory(path);
                foreach (var str in year.Reverse())
                {
                    var first = str;
                    int ind = path.IndexOf("/");
                    var list = path.Remove(path.IndexOf(first, ind));
                    if (staticpath != list)
                    {
                        deleteDirectory(list);
                    }
                    else
                    {
                        break;
                    }
                }
                //..................................//
            }

            return Ok();
        }
        [ApiExplorerSettings(IgnoreApi =true)]
        public void DeleteFile(string deleteFile)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                //throw ex;
                StreamWriter sw = new StreamWriter(_configuration.GetSection("DeleteDirectory").GetValue<string>("ServiceLogPath")/*ConfigurationManager.AppSettings["ServiceLogPath"]*/ + "DeleteDirINDLogE.txt", true);
                sw.WriteLine(ex);
                sw.WriteLine("In DeleteFile()" + DateTime.Now);
                sw.Flush();
                sw.Close();
            }
            return;
        }
        //public string[] directoryListDetailed(string directory)
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<string> directoryListDetailed(string directory)
        {
            List<string> result = new List<string>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + directory);
                request.Credentials = new NetworkCredential(user, pass);

                request.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                }

                reader.Close();
                response.Close();
                //return result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                //throw ex;
                StreamWriter sw = new StreamWriter(_configuration.GetSection("DeleteDirectory").GetValue<string>("ServiceLogPath")/*ConfigurationManager.AppSettings["ServiceLogPath"]*/ + "DeleteDirINDLogE.txt", true);
                sw.WriteLine(ex);
                sw.WriteLine("In directoryListDetailed()" + DateTime.Now);
                sw.Flush();
                sw.Close();
            }
            return result;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public void deleteDirectory(string directoryName)
        {
            try
            {
                var direcotryChildren = directoryListDetailed(directoryName);
                if (direcotryChildren.Any() && (!string.IsNullOrWhiteSpace(direcotryChildren[0])))
                {
                    foreach (var child in direcotryChildren)
                    {
                        if (child.Trim().Length > 0)
                        {
                            string mFileName = child;
                            if (mFileName.Contains("."))
                            {
                                if (mFileName.Contains(@"/"))
                                {
                                    string[] mFileNameNew = mFileName.Split('/');
                                    mFileName = mFileNameNew[mFileNameNew.Length - 1];
                                }
                                //DeleteFile(directoryName + @"/" + mFileName);
                                DeleteFile(directoryName + "/" + mFileName);
                            }
                            else
                            {
                                //folder
                                string data = directoryName + "/" + mFileName;
                                deleteDirectory(data.Replace("//", "/"));
                            }
                        }
                    }
                }
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + directoryName);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                //throw ex;
                StreamWriter sw = new StreamWriter(_configuration.GetSection("DeleteDirectory").GetValue<string>("ServiceLogPath")/*ConfigurationManager.AppSettings["ServiceLogPath"]*/ + "DeleteDirINDLogE.txt", true);
                sw.WriteLine(ex);
                sw.WriteLine("In deleteDirectory()" + DateTime.Now);
                sw.Flush();
                sw.Close();
            }
            return;
        }
    }
}
