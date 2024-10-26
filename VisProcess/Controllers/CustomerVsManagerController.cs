using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet.Messages.Authentication;
using System.IO.Compression;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using VisProcess.Models;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVsManagerController : ControllerBase
    {
        private readonly CustomerVsManagerService _customerVsManagerService;
        public CustomerVsManagerController(CustomerVsManagerService customerVsManagerService)
        {
            _customerVsManagerService = customerVsManagerService;
        }
        [HttpGet]
        [Route("GetAllCustomersList")]
        public IActionResult GetAllCustomersList()
        {
            try
            {
                var result = _customerVsManagerService.GetAllCustomersList();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetAccountManagerList")]
        public IActionResult GetAccountManagerList()
        {
            try
            {
                var result = _customerVsManagerService.GetAccountManagerList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetMappedCustomerList")]
        public IActionResult GetMappedCustomerList()
        {
            try
            {
                var result = _customerVsManagerService.GetMappedCustomerList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetUnMappedCustomerList")]
        public IActionResult GetUnMappedCustomerList()
        {
            try
            {
                var result = _customerVsManagerService.GetUnMappedCustomerList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetClientsByManagerId")]
        public object GetClientByManagerId(int managerId)
        {
            var customerDetails = _customerVsManagerService.GetClientByManagerId(managerId);
            if(customerDetails != null)
            {
                return Ok(customerDetails);

            }
            else
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("AssignManagerForMultipleCustomer")]
        public IActionResult AssignManager(ManagerAssignRequest request, int createdById)
        {
            var result = _customerVsManagerService.AssignManager(request, createdById);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Manager assigned successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false
                });
            }
        }

        [HttpGet]
        [Route("GetInactiveClientsByManagerId")]
        public object InactiveClientsByManagerId(int managerId)
        {
            return _customerVsManagerService.InactiveClientsByManagerId(managerId);
        }
        [HttpGet]
        //it will return the previous month file count of customer under the manager Id given
        [Route("GetFileCountDivisionwise")]
        public object FileCountDivisionWise(int managerId)
        {
            return _customerVsManagerService.FileCountDivisionWise(managerId);
        }
        [HttpPost]
        [Route("GetFileCountForGivenRangeByManagerId")]
        public IActionResult GetFileCountForGivenRangeByManagerId(GetFileCountReq request)
        {
            try
            {
                var result = _customerVsManagerService.GetFileCountForGivenRangeByManagerId(request);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("List is Empty");
                }
            }
            catch
            {
                return BadRequest("Enter the mandatory fields");
            }
        }

        [HttpGet]
        [Route("GetLastReceivedFileDateOfCustomerByManagerId")]
        public object GetLastReceivedFileDateOfCustomerByManagerId(int managerId)
        {
            return _customerVsManagerService.GetLastReceivedFileDateOfCustomerByManagerId(managerId);
        }
        [HttpGet]
        [Route("GetStatusMasterDropdownList")]
        public ActionResult<List<StatusMaster>> GetStatusMasterDropdownList()
        {
            try
            {
                var dropdownList = _customerVsManagerService.GetStatusMasterDropdownList();

                if (dropdownList.Count > 0)
                {
                    return Ok(dropdownList);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("GetInactiveClientsWithLastReceivedFileDateByManagerId")]
        public object GetInactiveClientsWithLastReceivedFileDateByManagerId(int managerId)
        {
            return _customerVsManagerService.GetInactiveClientsWithLastReceivedFileDateByManagerId(managerId);
        }
        [HttpPost]
        [Route("CustomerCallFollowUp")]
        public IActionResult CustomerCallFollowUp([FromForm] CustomerFollowUpReq form,int CreatedBy,int CustomerId)
        {
            var result = _customerVsManagerService.CustomerCallFollowUp(form, CreatedBy, CustomerId);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Follow Up details saved succesfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false
                });
            }
        }
        /*    Download all files as ZIP
         *    [HttpGet]
               [Route("DownloadFiles/{managerId}")]
               public IActionResult DownloadFiles(int managerId)
               {
                   try
                   {
                       // Fetch file paths from the database based on the manager ID
                       var filepaths = _customerVsManagerService.GetFilePathsByManagerId(managerId);

                       if (filepaths.Any())
                       {
                           var fileBytesList = new List<byte[]>();
                           var fileNamesList = new List<string>();

                           foreach (var filePath in filepaths)
                           {
                               if (System.IO.File.Exists(filePath))
                               {
                                   var fileName = Path.GetFileName(filePath);
                                   var fileBytes = System.IO.File.ReadAllBytes(filePath);

                                   fileNamesList.Add(fileName);
                                   fileBytesList.Add(fileBytes);
                               }
                           }

                           // Combine multiple files into a single response
                           var archiveStream = new MemoryStream();
                           using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                           {
                               for (int i = 0; i < fileBytesList.Count; i++)
                               {
                                   var entry = archive.CreateEntry(fileNamesList[i], CompressionLevel.Fastest);
                                   using (var entryStream = entry.Open())
                                   {
                                       entryStream.Write(fileBytesList[i], 0, fileBytesList[i].Length);
                                   }
                               }
                           }

                           archiveStream.Seek(0, SeekOrigin.Begin);
                           return File(archiveStream, "application/zip", "DownloadedFiles.zip");
                       }
                       else
                       {
                           return NotFound("No files found for download");
                       }
                   }
                   catch (Exception ex)
                   {
                       return StatusCode(500, "Internal Server Error");
                   }
               }*/

        [HttpGet]
        [Route("GetFollowUpReport")]
        public IActionResult GetFollowUpReport(int? managerId,string? filepath)
        {
            try
            {

                if (filepath != null && System.IO.File.Exists(filepath))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(filepath);

                    var fileName = Path.GetFileName(filepath);

                    return File(fileBytes, "application/octet-stream", fileName);
                }
                else if(managerId != null)
                {
                    var result = _customerVsManagerService.GetFollowUpReport((int)managerId);
                    return Ok(result);
                }
                else if(managerId == null && filepath == null)
                {
                    return BadRequest("Enter any one of the input fields");
                }
                else
                {
                    return NotFound("No files found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
