using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net;
using System.Text.RegularExpressions;

using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;


namespace Vis.VleadProcessV3.Models
{
    public class FileUpload
    {
      
        private readonly TableWork _tableWork;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
      
        private readonly ApplicationDbContext _context1;
        private readonly ApplicationDbContext _context1234;
        private readonly ApplicationDbContext _context123;
        private readonly ApplicationDbContext _context1232;
        private readonly IWebHostEnvironment _webHostEnvironment;
    

        public FileUpload(TableWork tableWork,ApplicationDbContext dbContext,IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _context1 = dbContext;
            _context1234 = dbContext;
       
            _context123 = dbContext;
            _context = dbContext;
            _context1232 = dbContext;
            _tableWork = tableWork;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        public FileMovement GetFileDependents(FileMovement fileMovement)
        {
            var folderPathPrefix = "JobFiles";
            var sourcePath = "";
            var dynamicFolderPath = "";
            var fakeDynamicFolderPath = "";
            var folderPath = "";
            var fileName = "";
            int fileCount = 0;
            var WFMId = 0;
            var WFTId = 0;
            int? pid = 0;
            var orignalFilePath = "";
            var jobFileName = "";
            if (fileMovement.IsClientOrder == 1)
            {
                dynamicFolderPath = Convert.ToString(fileMovement.OrderId);
                folderPath = folderPathPrefix + '\\' + dynamicFolderPath;
                fileName = Convert.ToString(fileMovement.OrderId) + '_';
                fileCount = _tableWork.ClientOrderExtRepository.Count(x => x.ClientOrderId == fileMovement.OrderId);
                sourcePath = _tableWork.ClientOrderRepository.Get(x => x.OrderId == fileMovement.OrderId).Select(x => x.FileUploadPath).FirstOrDefault();
                jobFileName = _tableWork.ClientOrderRepository.Get(x => x.OrderId == fileMovement.OrderId).Select(x => x.FileName).FirstOrDefault();
            }
            else if (fileMovement.IsClientOrder == 0)
            {
                var jobOrder = _tableWork.JobOrderRepository.GetAllVal(x => x.Customer).FirstOrDefault(x => x.Id == fileMovement.OrderId);// _db.JobOrders.Include("Customer").FirstOrDefault(x => x.Id == fileMovement.OrderId);
                var jobId = jobOrder.JobId;
                jobFileName = Regex.Replace(jobOrder.FileName, "[^0-9a-zA-Z]+", " ");
                jobId = jobId.Replace('/', '-');
                string fileReceivedDate = jobOrder.FileReceivedDate.ToString("MM-dd-yyyy");
                dynamicFolderPath = jobOrder.Customer.ShortName + '\\' + fileReceivedDate + '\\' + Regex.Replace(jobOrder.FileName, "[^0-9a-zA-Z]+", " ") + '-' + jobId;
                orignalFilePath = dynamicFolderPath;
                fileName = jobId + '_';
                WFMId = _tableWork.ProcessWorkFlowMasterRepository.Get(x => x.JobId == fileMovement.OrderId).Select(x => x.Id).FirstOrDefault();
                WFTId = _tableWork.ProcessWorkFlowTranRepository.Get(x => x. Wfmid == WFMId).OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                if (fileMovement.StatusId == 0)
                {
                    dynamicFolderPath = dynamicFolderPath + '\\' + "Production Allocation\\Pending-1";
                    sourcePath = _tableWork.ClientOrderRepository.Get(x => x.OrderId == jobOrder.ClientOrderId).Select(x => x.FileUploadPath).FirstOrDefault();
                }
                else
                {
                    var process = _tableWork.ProcessRepository.Get(x => x.Id == fileMovement.ProcessId).Select(x => x.Name).FirstOrDefault();
                    var status = _tableWork.StatusRepository.Get(x => x.Id == fileMovement.StatusId).Select(x => x.Name).FirstOrDefault();
                    var processStatusCount = _tableWork.ProcessWorkFlowTranRepository.Count(x => x.ProcessId == fileMovement.ProcessId && x.StatusId == fileMovement.StatusId && x. Wfmid == WFMId);
                    if ((fileMovement.StatusId != fileMovement.FakeStatusId) && fileMovement.IsCopyFiles != 1)
                    {
                        var fakeStatus = _tableWork.StatusRepository.Get(x => x.Id == fileMovement.FakeStatusId).Select(x => x.Name).FirstOrDefault();
                        var fakeProcessStatusCount = _tableWork.ProcessWorkFlowTranRepository.Count(x => x.ProcessId == fileMovement.ProcessId && x.StatusId == fileMovement.FakeStatusId && x. Wfmid == WFMId);
                        fakeDynamicFolderPath = dynamicFolderPath + '\\' + process + '\\' + fakeStatus + '-' + fakeProcessStatusCount;
                        process = _tableWork.ProcessRepository.Get(x => x.Id == fileMovement.FakeProcessId).Select(x => x.Name).FirstOrDefault();
                    }
                    if (fileMovement.IsCopyFiles == 1)
                    {
                        if (fileMovement.IsProcessWorkFlowTranInserted == 0)
                        {
                            processStatusCount++;
                            var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x. Wfmid == WFMId).OrderByDescending(x => x.Id).Select(x => new { x.FileUploadPath, x.ProcessId }).FirstOrDefault();
                            sourcePath = processWorkFlowTran.FileUploadPath;
                            pid = processWorkFlowTran.ProcessId;
                        }
                        else
                        {
                            var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x. Wfmid == WFMId).OrderByDescending(x => x.Id).ToList().Skip(1).Select(x => new { x.FileUploadPath, x.ProcessId }).FirstOrDefault();
                            sourcePath = processWorkFlowTran.FileUploadPath;
                            pid = processWorkFlowTran.ProcessId;
                        }
                    }
                    else
                    {
                        if (fileMovement.IsProcessWorkFlowTranInserted == 0)
                        {
                            processStatusCount++;
                        }
                    }
                    dynamicFolderPath = dynamicFolderPath + '\\' + process + '\\' + status + '-' + processStatusCount;
                    fileCount = _tableWork.JobOrderFileRepository.Count(x => x.JobId == fileMovement.OrderId);
                }
            }
            folderPath = folderPathPrefix + '\\' + dynamicFolderPath;
            fileMovement.SourcePath = folderPathPrefix + '\\' + sourcePath;
            fileMovement.DynamicFolderPath = dynamicFolderPath;
            fileMovement.FolderPath = folderPath;
            fileMovement.FileName = fileName;
            fileMovement.FileCount = fileCount;
            fileMovement.WFMId = WFMId;
            fileMovement.WFTId = WFTId;
            fileMovement.OrignalPath = folderPathPrefix + '\\' + orignalFilePath + '\\' + "Orignal File";
            fileMovement.OrignalDynamicPath = orignalFilePath + '\\' + "Orignal File";
            fileMovement.Pid = pid;
            fileMovement.FakeDynamicFolderPath = fakeDynamicFolderPath;
            fileMovement.JobFileName = jobFileName;
            return fileMovement;
        }
        public bool UploadFiles(IFormFileCollection filesToUpload, FileMovement fileMovement)
        {
            var fileDependents = GetFileDependents(fileMovement);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath,fileDependents.FolderPath); //HttpContext.Current.Server.MapPath(fileDependents.FolderPath);
            var orignalPath = Path.Combine(_webHostEnvironment.ContentRootPath,fileDependents.OrignalPath); //HttpContext.Current.Server.MapPath(fileDependents.OrignalPath);
            System.IO.Directory.CreateDirectory(path);
            var check = _tableWork.JobOrderFileRepository.GetSingle(x => x. Wfmid == fileDependents.WFMId);
            var jobOrder = _tableWork.JobOrderRepository.GetSingle(x => x.Id == fileDependents.OrderId);
            var WFMId = _tableWork.ProcessWorkFlowMasterRepository.Get(x => x.JobId == fileMovement.OrderId).Select(x => x.Id).FirstOrDefault();
            var ProcessWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x. Wfmid == WFMId).OrderByDescending(x => x.Id).FirstOrDefault();

            for (var i = 0; i < (filesToUpload.Count); i++)
            {
                var fileName = "";
                fileDependents.FileCount++;
                fileName = Regex.Replace(Path.GetFileNameWithoutExtension(filesToUpload[i].FileName), "[^0-9a-zA-Z_-]+", " ");
                if (fileName == " ")
                {
                    fileName = fileMovement.JobFileName;
                }
                fileName += Path.GetExtension(filesToUpload[i].FileName);

                if (filesToUpload[i] == null || filesToUpload[i].Length== 0)
                {
                    return false;
                }
                try
                {
                    var checkExist = System.IO.File.Exists(path + "\\" + fileName);
                    var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    var fileNameExt = System.IO.Path.GetExtension(fileName);
                    for (int j = 1; checkExist == true; j++)
                    {
                        fileName = fileNameWithoutExt + " (" + j + ")" + fileNameExt;
                        checkExist = System.IO.File.Exists(path + "\\" + fileName);
                    }
                    using (Stream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write))
                    {
                        filesToUpload[i].CopyToAsync(fileStream);
                    }
                   // filesToUpload[i].CopyToAsync(Path.Combine(path, fileName));
                    if (fileDependents.IsClientOrder == 0)
                    {
                        if (check == null && jobOrder.ClientOrderId == null)
                        {
                           
                                System.IO.Directory.CreateDirectory(orignalPath);
                                using (Stream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write))
                                {
                                    filesToUpload[i].CopyToAsync(fileStream);
                                }
                                // filesToUpload[i].CopyToAsync(Path.Combine(orignalPath, fileName));
                                jobOrder.FileUploadPath = fileDependents.OrignalDynamicPath;
                                //_tableWork.JobOrderRepository.Update(jobOrder);
                                _context.Entry(jobOrder).State = EntityState.Modified;
                                _context.SaveChanges();
                        
                        }
                        if (fileDependents.IsProcessWorkFlowTranInserted == 1)
                        {
                        
                                var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.GetSingle(x => x.Id == fileDependents.WFTId);
                                if (fileMovement.StatusId != fileMovement.FakeStatusId)
                                {
                                    processWorkFlowTran.FileUploadPath = fileDependents.FakeDynamicFolderPath;
                                }
                                else
                                {
                                    processWorkFlowTran.FileUploadPath = fileDependents.DynamicFolderPath;
                                }
                                processWorkFlowTran.UpdatedUtc = DateTime.UtcNow;
                                processWorkFlowTran.UpdatedBy = processWorkFlowTran.CreatedBy;
                                //_tableWork.ProcessWorkFlowTranRepository.Update(processWorkFlowTran);
                                _context1.Entry(processWorkFlowTran).State = EntityState.Modified;
                                _context1.SaveChanges();
                           
                        }
                        var jobOrderFile = new JobOrderFile();
                        jobOrderFile.Wfmid = fileDependents.WFMId;
                        jobOrderFile.Wftid = fileDependents.WFTId;
                        jobOrderFile.JobId = fileDependents.OrderId;
                        jobOrderFile.ProcessId = fileDependents.FakeProcessId;
                        jobOrderFile.IsActive = true;
                        jobOrderFile.FileName = fileName;
                        jobOrderFile.CreatedUtc = DateTime.UtcNow;
                        _tableWork.JobOrderFileRepository.Insert(jobOrderFile);
                    }
                    else if (fileDependents.IsClientOrder == 1)
                    {
                        var clientOrderExt = new ClientOrderExt();
                        clientOrderExt.ClientOrderId = fileDependents.OrderId;
                        clientOrderExt.AssociateFileName = fileName;
                        clientOrderExt.IsDeleted = false;
                        _tableWork.ClientOrderExtRepository.Insert(clientOrderExt);
                        var clientOrder = _tableWork.ClientOrderRepository.GetSingle(x => x.OrderId == fileDependents.OrderId);
                        clientOrder.FileUploadPath = fileDependents.DynamicFolderPath;
                        _tableWork.ClientOrderRepository.Update(clientOrder);
                    }
                    _tableWork.SaveChanges();

                    if (fileMovement.IsClientOrder == 0 && fileMovement.IsProcessWorkFlowTranInserted == 1)
                    {
                        if (ProcessWorkFlowTran.ProcessId == null && ProcessWorkFlowTran.FileUploadPath != null)
                        {
                            FTPFileUpload(true, fileName, fileDependents.DynamicFolderPath);
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }
        public bool CopyFiles(IEnumerable<FileMovement> fileMovement)
        {
            foreach (var item in fileMovement)
            {
                var filesToCopy = new List<string>();
                var fileDependents = GetFileDependents(item);
                var sourcePath = Path.Combine(_webHostEnvironment.ContentRootPath,fileDependents.SourcePath);//HttpContext.Current.Server.MapPath(fileDependents.SourcePath);
                var orignalPath = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.OrignalPath); //Path.Combine(fileDependents.SourcePath)HttpContext.Current.Server.MapPath(fileDependents.OrignalPath);
                var destinationPath = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.FolderPath);// HttpContext.Current.Server.MapPath(fileDependents.FolderPath);
                System.IO.Directory.CreateDirectory(destinationPath);
                if (item.files == null)
                {
                    filesToCopy = Directory.GetFiles(sourcePath).Select(Path.GetFileName).ToList();
                }
                else
                {
                    filesToCopy = item.files;
                }
                foreach (var sourceFileName in filesToCopy)
                {
                    var fileName = "";
                    if (fileDependents.IsClientOrder == 1)
                    {
                        fileDependents.FileCount++;
                        fileName = sourceFileName;
                    }
                    else if (fileDependents.IsClientOrder == 0)
                    {
                        fileName = sourceFileName;
                        var checkExist = System.IO.File.Exists(destinationPath + "\\" + fileName);
                        var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        var fileNameExt = System.IO.Path.GetExtension(fileName);
                        for (int i = 1; checkExist == true; i++)
                        {
                            fileName = fileNameWithoutExt + "(" + i + ")" + fileNameExt;
                            checkExist = System.IO.File.Exists(destinationPath + "\\" + fileName);
                        }
                    }
                    string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
                    string destinationFile = System.IO.Path.Combine(destinationPath, fileName);
                    System.IO.File.Copy(sourceFile, destinationFile, true);
                    if (fileDependents.IsProcessWorkFlowTranInserted == 1)
                    {
                        var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.GetSingle(x => x.Id == fileDependents.WFTId);
                        if (item.StatusId != item.FakeStatusId)
                        {
                            processWorkFlowTran.FileUploadPath = fileDependents.FakeDynamicFolderPath;
                        }
                        else
                        {
                            processWorkFlowTran.FileUploadPath = fileDependents.DynamicFolderPath;
                        }
                        _tableWork.ProcessWorkFlowTranRepository.Update(processWorkFlowTran);
                    }
                    var jobOrderFile = new JobOrderFile();
                    jobOrderFile.Wfmid = item.WFMId;
                    jobOrderFile.Wftid = fileDependents.WFTId;
                    jobOrderFile.JobId = item.OrderId;
                    jobOrderFile.ProcessId = item.ProcessId;
                    jobOrderFile.IsActive = true;
                    jobOrderFile.FileName = fileName;
                    jobOrderFile.CreatedUtc= DateTime.UtcNow;
                    _tableWork.JobOrderFileRepository.Insert(jobOrderFile);
                }
            }
            _tableWork.SaveChanges();
            return true;
        }

        public bool copyFilesWithFileNames(IEnumerable<FileMovement> fileMovement)
        {
            var folderPathPrefix = "JobFiles";
            foreach (var fileName in fileMovement)
            {
                var WFMId = _tableWork.ProcessWorkFlowMasterRepository.Get(x => x.JobId == fileName.OrderId).Select(x => x.Id).FirstOrDefault();

                var pwtRecord = _tableWork.ProcessWorkFlowTranRepository.Get(x => x. Wfmid == WFMId).OrderByDescending(x => x.Id).Select(x => new { x.FileUploadPath, x.ProcessId }).ToList();

                var sourceFiles = pwtRecord.Skip(1).FirstOrDefault().FileUploadPath;

                var destinationFiles = pwtRecord.FirstOrDefault().FileUploadPath;

                var sourcePath = folderPathPrefix + "//" + sourceFiles;
                var destinationPath = folderPathPrefix + "//" + destinationFiles;
                var sourceCompletePath = Path.Combine(_webHostEnvironment.ContentRootPath,sourcePath);//HttpContext.Current.Server.MapPath(sourcePath);
                var destinationCompletePath = Path.Combine(_webHostEnvironment.ContentRootPath,destinationPath);// HttpContext.Current.Server.MapPath(destinationPath);
                foreach (var item in fileName.files)
                {
                    string sourceFile = System.IO.Path.Combine(sourceCompletePath, item);
                    string destinationFile = System.IO.Path.Combine(destinationCompletePath, item);
                    System.IO.File.Copy(sourceFile, destinationFile, true);
                }
            }
            return true;
        }

        public Object DeleteFiles(FileMovement fileMovement)
        {
            List<string> files = new List<string>();
            var fileDependents = GetFileDependents(fileMovement);
            foreach (var item in fileMovement.files)
            {
                string sourceFile = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath,fileDependents.SourcePath, item);
                System.IO.File.Delete(sourceFile);
                files.Add(item);
            }
            var result = new
            {
                FilesName = files
            };
            return result;
        }
        public bool UploadQueryFiles(IFormFileCollection filesToUpload, int WFMId)
        {
            var jobId = _tableWork.ProcessWorkFlowMasterRepository.Get(x => x.Id == WFMId).Select(x => x.JobId).FirstOrDefault();
            var tranId = _tableWork.ProcessWorkFlowTranRepository.Get(x => x. Wfmid == WFMId).OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefault();
            var fileUploadPath = _tableWork.JobOrderRepository.Get(x => x.Id == jobId).Select(x => x.FileUploadPath).FirstOrDefault();
            var count = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.ProcessId == null && (x.StatusId == 6 || x.StatusId == 8) && x. Wfmid == WFMId).Count();
            if (fileUploadPath == null)
            {
                var jobOrder = _tableWork.JobOrderRepository.GetAllVal(x => x.Customer).FirstOrDefault(x => x.Id == jobId);
                var jobOrderId = jobOrder.JobId;
                jobOrderId = jobOrderId.Replace('/', '-');
                string fileReceivedDate = jobOrder.FileReceivedDate.ToString("MM-dd-yyyy");
                fileUploadPath = jobOrder.Customer.ShortName + '\\' + fileReceivedDate + '\\' + jobOrder.FileName + '-' + jobOrderId + '\\' + "Orignal File";
                jobOrder.FileUploadPath = fileUploadPath;
                _tableWork.JobOrderRepository.Update(jobOrder);
            }
            string path = Path.Combine(_webHostEnvironment.ContentRootPath,"JobFiles" + fileUploadPath);
              
            System.IO.Directory.CreateDirectory(path);
            for (var i = 0; i < filesToUpload.Count; i++)
            {
                var fileName = "";
                fileName = Path.GetFileNameWithoutExtension(filesToUpload[i].FileName) + "-query-" + count + Path.GetExtension(filesToUpload[i].FileName); ;
                if (filesToUpload[i] == null || filesToUpload[i].Length == 0)
                {
                    return false;
                }
                try
                {
                    var checkExist = System.IO.File.Exists(path + "\\" + fileName);
                    var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    var fileNameExt = System.IO.Path.GetExtension(fileName);
                    for (int j = 1; checkExist == true; j++)
                    {
                        fileName = fileNameWithoutExt + " (" + j + ")" + fileNameExt;
                        checkExist = System.IO.File.Exists(path + "\\" + fileName);
                    }
                    using (Stream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write))
                    {
                        filesToUpload[i].CopyToAsync(fileStream);
                    }
                   // filesToUpload[i].CopyToAsync(Path.Combine(path, fileName));
                    var jobOrderFile = new JobOrderFile();
                    jobOrderFile.Wfmid = WFMId;
                    jobOrderFile.Wftid = tranId;
                    jobOrderFile.JobId = jobId;
                    jobOrderFile.ProcessId = null;
                    jobOrderFile.IsActive = true;
                    jobOrderFile.FileName = fileName;
                    jobOrderFile.CreatedUtc = DateTime.UtcNow;
                    _tableWork.JobOrderFileRepository.Insert(jobOrderFile);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            _tableWork.SaveChanges();
            return true;
        }

        public void FTPFileCopy(string FileUploadPath)
        {
            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath,"JobFiles", FileUploadPath);//HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
            var filesToCopy = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
            foreach (var file in filesToCopy)
            {
                try
                {
                    FTPFileUpload(true, file, FileUploadPath);
                }
                catch
                {
                    continue;
                }
            }
        }

        //SFTP
        public void SFTPFileCopyWithBatch(string FileUploadPath, DateTime? dou, string jobFileName, string ftpPath, string ftpUsername123, string ftpPassword123, string Host, int Port)
        {
            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath,"JobFiles", FileUploadPath);// HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
            var filesToCopy = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
            var destinationPath = dou.Value.Date + "\\" + jobFileName;
            var Dateval = Regex.Replace(destinationPath.ToString(), "/", " ");
            var dateval = Regex.Replace(Dateval, ":", "");
            foreach (var file in filesToCopy)
            {
                SFTPFileUploadWithBatch(true, file, FileUploadPath, dateval, ftpPath, ftpUsername123, ftpPassword123, Host, Port);
            }
        }
        //SFTP
        public void SFTPFileUploadWithBatch(bool IsFilesInLocalServer, string FileName, string FileUploadPath, string destinationPath, string SFTPPath, string SFTPUsername123, string SFTPPassword123, string Host, int Port)
        {
            string SFTPUrl, SFTPUsername, SFTPPassword, fileToUploadPath, SFTPHost;
            int SFTPPort;

            var connectionInfo = new KeyboardInteractiveConnectionInfo(Host, Port, SFTPUsername123);

            connectionInfo.AuthenticationPrompt += delegate (object sender, AuthenticationPromptEventArgs e)
            {
                foreach (var prompt in e.Prompts)
                {
                    if (prompt.Request.Equals("Password: ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        prompt.Response = SFTPPassword123;
                    }
                }
            };

            var client = new SftpClient(connectionInfo);

            client.Connect();
            client.ChangePermissions(SFTPPath, 777);

            if (!IsFilesInLocalServer)
            {
                SFTPUrl = SFTPPath;
                SFTPUsername = SFTPUsername123;
                SFTPPassword = SFTPPassword123;
                SFTPHost = Host;
                SFTPPort = Port;
            }
            else
            {
                SFTPUrl = SFTPPath;
                SFTPUsername = SFTPUsername123;
                SFTPPassword = SFTPPassword123;
            }
            var credentials = new NetworkCredential(SFTPUsername, SFTPPassword);

            bool isFolderAvailable = false;
            bool isFileAvailable = false;
            string fileName = FileName, dynamicFolder = null;
            fileToUploadPath = destinationPath.Replace("\\", "/");
            var folders = fileToUploadPath.Split('/');
            foreach (var folder in folders)
            {
                try
                {
                    dynamicFolder = dynamicFolder + folder + "/";
                    if (client.Exists(SFTPUrl + "/" + dynamicFolder) == false)
                    {
                        client.CreateDirectory(SFTPUrl + "/" + dynamicFolder);
                        client.ChangePermissions(SFTPUrl + "/" + dynamicFolder, 777);
                    }

                    isFolderAvailable = true;
                }
                catch (Exception ex)
                {
                  
                        Log addlog = new Log();
                        addlog.Module = "SFTP Upload Error";
                        addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                        addlog.Type = "Error Upload File";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _context1234.Logs.Add(addlog);
                        _context1234.SaveChanges();
                 
                    if (client.Exists(SFTPUrl + "/" + dynamicFolder) == true)
                    {
                        isFolderAvailable = true;
                    }
                    else
                    {
                        isFolderAvailable = false;
                    }

                }
            }

            if (isFolderAvailable)
            {
                try
                {
                    isFileAvailable = false;
                }
                catch (Exception ex)
                {
                   
                        Log addlog = new Log();
                        addlog.Module = "SFTP Upload Error middle";
                        addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                        addlog.Type = "Error Upload File";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _context123.Logs.Add(addlog);
                   
                    if (client.Exists(SFTPUrl + "/" + dynamicFolder) == true)
                    {
                        isFolderAvailable = false;
                    }
                    else
                    {
                        isFolderAvailable = true;
                    }
                }
            }
            if (!isFileAvailable)
            {
                try
                {
                    string address = SFTPUrl + fileToUploadPath + "/" + fileName;
                    address = address.Replace("\\", "/");
                    var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", FileUploadPath); //HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
                    string filename = folderPath + "\\" + fileName;
                    using (var fileStream = new FileStream(filename, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fileStream, address);
                        client.ChangePermissions(address, 777);
                    }

                }
                catch (Exception ex)
                {
                   
                        Log addlog = new Log();
                        addlog.Module = "SFTP Upload Error end";
                        addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                        addlog.Type = "Error Upload File";
                        addlog.CreatedUtc= DateTime.UtcNow;
                        _context1232.Logs.Add(addlog);
                        _context1232.SaveChanges();
                   
                }
            }
        }
        public void FTPFileCopyWithBatch(string FileUploadPath, DateTime? dou, string jobFileName, string ftpPath, string ftpUsername123, string ftpPassword123)
        {
            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", FileUploadPath);// HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
            var filesToCopy = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
            var destinationPath = dou.Value.Date + "\\" + jobFileName;
            var Dateval = Regex.Replace(destinationPath.ToString(), "/", " ");
            var dateval = Regex.Replace(Dateval, ":", "");
            foreach (var file in filesToCopy)
            {
                FTPFileUploadWithBatch(true, file, FileUploadPath, dateval, ftpPath, ftpUsername123, ftpPassword123);
            }
        }
        public void FTPFileUploadWithBatch(bool IsFilesInLocalServer, string FileName, string FileUploadPath, string destinationPath, string ftpPath, string ftpUsername123, string ftpPassword123)
        {
            string ftpUrl, ftpUsername, ftpPassword, fileToUploadPath;
            if (!IsFilesInLocalServer)
            {
                ftpUrl = ftpPath;
                ftpUsername = ftpUsername123;
                ftpPassword = ftpPassword123;
            }
            else
            {
                ftpUrl = ftpPath;
                ftpUsername = ftpUsername123;
                ftpPassword = ftpPassword123;
            }
            var credentials = new NetworkCredential(ftpUsername, ftpPassword);

            bool isFolderAvailable = false;
            bool isFileAvailable = false;
            string fileName = FileName, dynamicFolder = null;
            fileToUploadPath = destinationPath.Replace("\\", "/");
            var folders = fileToUploadPath.Split('/');
            foreach (var folder in folders)
            {
                try
                {
                    dynamicFolder = dynamicFolder + folder + "/";
                    WebRequest webRequest = WebRequest.Create(ftpUrl + "\\" + dynamicFolder);
                    webRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    webRequest.Credentials = credentials;
                    webRequest.GetResponse();
                    isFolderAvailable = true;
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                        isFolderAvailable = true;
                    }
                    else
                    {
                        response.Close();
                        isFolderAvailable = false;
                    }
                }
            }

            if (isFolderAvailable)
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(ftpUrl + fileToUploadPath + "/" + fileName);
                    webRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                    webRequest.Credentials = credentials;
                    webRequest.GetResponse();
                    isFileAvailable = true;
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                        isFileAvailable = false;
                    }
                    else
                    {
                        response.Close();
                        isFileAvailable = false;
                    }
                }
            }
            if (!isFileAvailable)
            {
                WebClient client = new WebClient();
                client.Credentials = credentials;
                string address = ftpUrl + fileToUploadPath + "/" + fileName;
                address = address.Replace("\\", "/");
                var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", FileUploadPath); //HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
                string filename = folderPath + "\\" + fileName;
                client.UploadFile(address, filename);
            }
            //return true;
        }
        public void FTPFileUpload(bool IsFilesInLocalServer, string FileName, string FileUploadPath)
        {
            string ftpUrl, ftpUsername, ftpPassword, fileToUploadPath;
            if (!IsFilesInLocalServer)
            {
                ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerUrl");// ConfigurationManager.AppSettings["ftpLocalServerUrl"];
                ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerUsername"); //ConfigurationManager.AppSettings["ftpLocalServerUsername"];
                ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerPassword");// ConfigurationManager.AppSettings["ftpLocalServerPassword"];
            }
            else
            {
                ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUrl");//ConfigurationManager.AppSettings["ftpClientServerUrl"];
                ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUsername");// ConfigurationManager.AppSettings["ftpClientServerUsername"];
                ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerPassword"); //ConfigurationManager.AppSettings["ftpClientServerPassword"];
            }
            var credentials = new NetworkCredential(ftpUsername, ftpPassword);

            bool isFolderAvailable = false;
            bool isFileAvailable = false;
            //int fileCount = 0;
            string fileName = FileName, dynamicFolder = null;
            //if (destinationPath != string.Empty)
            //    fileToUploadPath = destinationPath.Replace("\\", "/");
            //else
            fileToUploadPath = FileUploadPath.Replace("\\", "/");
            var folders = fileToUploadPath.Split('/');
            foreach (var folder in folders)
            {
                try
                {
                    dynamicFolder = dynamicFolder + folder + "/";
                    WebRequest webRequest = WebRequest.Create(ftpUrl + dynamicFolder);
                    webRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    webRequest.Credentials = credentials;
                    webRequest.GetResponse();
                    isFolderAvailable = true;
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                        isFolderAvailable = true;
                    }
                    else
                    {
                        response.Close();
                        isFolderAvailable = false;
                    }
                }
            }

            if (isFolderAvailable)
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(ftpUrl + fileToUploadPath + "/" + fileName);
                    webRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                    // webRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    webRequest.Credentials = credentials;
                    webRequest.GetResponse();
                    isFileAvailable = true;
                    //fileCount++;
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                        isFileAvailable = false;
                        //fileName = checkFileName;
                    }
                    else
                    {
                        response.Close();
                        isFileAvailable = false;
                        //break;
                    }
                }
                //}
                //while (!isFileNotAvailable);
            }
            if (!isFileAvailable)
            {
                WebClient client = new WebClient();
                client.Credentials = credentials;
                string address = ftpUrl + FileUploadPath + "\\" + fileName;
                address = address.Replace("\\", "/");
                var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", FileUploadPath);// HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
                string filename = folderPath + "\\" + fileName;
                client.UploadFile(address, filename);
            }
            //return true;
        }

        public void CopyFilesFromServer(int TranIdOrOrderId, string FileUploadPath, bool IsClientOrder)
        {
            try
            {
                var filePath = FileUploadPath.Replace("\\", "/");
                var files = GetFilesFromServer(filePath);
                var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", FileUploadPath);// HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
                //System.IO.Directory.CreateDirectory(localPath);
                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }
                foreach (var file in files)
                {
                    var checkedFileExist = File.Exists(localPath + "\\" + file);
                    if (!checkedFileExist)
                    {
                        byte[] data;
                        using (WebClient client = new WebClient())
                        {
                            var url = _configuration.GetSection("AllocationService").GetValue<string>("serverUrl");//ConfigurationManager.AppSettings["serverUrl"];
                            data = client.DownloadData(url + "/JobFiles/" + filePath + "/" + file);
                        }
                        File.WriteAllBytes(localPath + "\\" + file, data);
                    }
                }
                if (IsClientOrder)
                {
                    var clientOrder = _tableWork.ClientOrderRepository.GetSingle(x => x.OrderId == TranIdOrOrderId);
                    clientOrder.IsFileCopied = true;
                    _tableWork.ClientOrderRepository.Update(clientOrder);
                    //_db.Entry(clientOrder).State = EntityState.Modified;
                }
                else
                {
                    var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.GetSingle(x => x.Id == TranIdOrOrderId);
                    processWorkFlowTran.IsFileCopied = true;
                    _tableWork.ProcessWorkFlowTranRepository.Update(processWorkFlowTran);
                    //_db.Entry(processWorkFlowTran).State = EntityState.Modified;
                }

                _tableWork.SaveChanges();
            }
            catch (Exception ex)
            {
              
                    Log addlog = new Log();
                    addlog.Module = "Copy Files from Server - " + TranIdOrOrderId;
                    addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                    addlog.Type = "Error Occured";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _context.Logs.Add(addlog);
                    _context.SaveChanges();
              
            }
        }

        private List<string> GetFilesFromServer(string FilePath)
        {
            var ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUrl");// ConfigurationManager.AppSettings["ftpClientServerUrl"];
            var ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUsername");// ConfigurationManager.AppSettings["ftpClientServerUsername"];
            var ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerPassword"); //ConfigurationManager.AppSettings["ftpClientServerPassword"];
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + FilePath + "/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception ex)
            {
              
                    Log addlog = new Log();
                    addlog.Module = "Get FIles from server - ";
                    addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                    addlog.Type = "Error Occured";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _context.Logs.Add(addlog);
                    _context.SaveChanges();
                
                throw ex;
            }
        }
    }
}
