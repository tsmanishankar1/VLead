using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class SingleEntryController : ControllerBase
    
    {
        private readonly SingleEntryService _singleEntryService;
        public SingleEntryController(SingleEntryService singleEntryService)
        {
            _singleEntryService = singleEntryService;
        }
        [HttpGet]
        [Route("getTableValue")]
        public IEnumerable<Object> GetTableValue(String tableName)
        {
            return _singleEntryService.GetTableValuesByTableName(tableName);
        }
        [HttpPost]
        [Route("postSingleEntryData")]
        public Object CustomerDetails1([FromBody] SingleEntry data)
        {
            bool success = false;
            if (data.Action == "Add")
            {
                data.IsDeleted = false;
                data.CreatedUTC = DateTime.UtcNow;
                success = _singleEntryService.AddSingleEntry(data);
            }
            else if (data.Action == "Update")
            {
                data.IsDeleted = false;
                data.UpdatedUTC = DateTime.Now;
                success = _singleEntryService.UpdateSingleEntry(data);
            }
            else if (data.Action == "Remove")
            {
                data.IsDeleted = true;
                data.UpdatedUTC = DateTime.Now;
                success = _singleEntryService.UpdateSingleEntry(data);
            }
            var result = new
            {
                Success = success
            };
            return result;
        }
    }
}
