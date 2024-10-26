using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
       
        private readonly FileUpload fileUpload;
        public FileController( FileUpload fileUpload)
        {
            this.fileUpload = fileUpload;   
        }
        [HttpPost]
        [Route("uploadFiles/{orderId}/{isClientOrder}/{processId}/{statusId}/{isProcessWorkFlowTranInserted}/{fakeProcessId}/{fakeStatusId}")]
        public bool UploadFiles(int orderId, int isClientOrder, int? processId, int statusId, int isProcessWorkFlowTranInserted, int? fakeProcessId, int fakeStatusId)
        {
            var httpRequest = HttpContext.Request;
            if (processId == 0)
            {
                processId = null;
            }
            if (fakeProcessId == 0)
            {
                fakeProcessId = null;
            }
            var fileMovement = new FileMovement();
            fileMovement.OrderId = orderId;
            fileMovement.IsClientOrder = isClientOrder;
            fileMovement.ProcessId = processId;
            fileMovement.StatusId = statusId;
            fileMovement.FakeProcessId = fakeProcessId;
            fileMovement.FakeStatusId = fakeStatusId;
            fileMovement.IsProcessWorkFlowTranInserted = isProcessWorkFlowTranInserted;
         
            var result = fileUpload.UploadFiles(httpRequest.Form.Files, fileMovement);
            return true;
        }
        [HttpPost]
        [Route("copyFiles")]
        public Object CopyFiles(FileMovement fileMovement)
        {
            var fileMovementList = new List<FileMovement>();
            fileMovementList.Add(fileMovement);
         
            var result = fileUpload.CopyFiles(fileMovementList);
            return result;
        }
        [HttpPost]
        [Route("copyFilesWithFileNames")]
        public Object copyFilesWithFileNames(FileMovement fileMovement)
        {
            var fileMovementList = new List<FileMovement>();
            fileMovementList.Add(fileMovement);
          
            var result = fileUpload.copyFilesWithFileNames(fileMovementList);
            return result;
        }
        [HttpPost]
        [Route("deleteFiles")]
        public Object DeleteFiles(FileMovement fileMovement)
        {
         
            var result = fileUpload.DeleteFiles(fileMovement);
            return result;
        }
        [HttpPost]
        [Route("uploadQueryFiles/{orderId}")]
        public bool UploadFiles(int orderId)
        {
            var httpRequest = HttpContext.Request;
         
            var result = fileUpload.UploadQueryFiles(httpRequest.Form.Files, orderId);
            return true;
        }
    }
}
