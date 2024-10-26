using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;


namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/ArtistInventoryReport")]
    [ApiController]
    public class ArtistInventoryReportController : ControllerBase
    {
        private readonly ArtistInventoryReportService _artistInventoryReportService;// = new ArtistInventoryReportService();
        public ArtistInventoryReportController(ArtistInventoryReportService artistInventoryReportService)
        {
            _artistInventoryReportService = artistInventoryReportService;// new ArtistInventoryReportService(configuration);
        }


        // ----------- Read Artist Inventory Report Dept wise Details ---------

        [HttpGet]
        [Route("GetArtistInventoryReportDept")]
        public Object GetArtistInventoryReportDept([FromQuery] ArtistInventoryReportDeptViewModel jobArtistInventoryReportDept)
        {
            var ArtistInventoryReportDeptDetails = _artistInventoryReportService.GetArtistInventoryDeptReport(jobArtistInventoryReportDept);
            var Details = new
            {
                ArtistInventoryReportDept = ArtistInventoryReportDeptDetails,
            };
            return Ok(Details);
        }

        // ----------- Read Artist Inventory Report Division wise Details ---------

        [HttpGet]
        [Route("GetArtistInventoryReportDiv")]
        public Object GetArtistInventoryReportDiv([FromQuery] ArtistInventoryReportDivViewModel jobArtistInventoryReportDiv)
        {
            var ArtistInventoryReportDivDetails = _artistInventoryReportService.GetArtistInventoryDivReport(jobArtistInventoryReportDiv);
            var Details = new
            {
                ArtistInventoryReportDiv = ArtistInventoryReportDivDetails,
            };
            return Ok(Details);
        }

        // ----------- Read Artwork/Digitizing/OrderEntry Inventory Details ---------

        [HttpGet]
        [Route("GetArtDigiOrderEntryInventory")]
        public Object GetArtDigiOrderEntryInventory([FromQuery] ArtistInventoryReportDeptViewModel jobArtDigiOrderEntryInventory)
        {
            var ArtDigiOrderEntryInventoryDetails = _artistInventoryReportService.GetArtDigiOrderEntryInventory(jobArtDigiOrderEntryInventory);
            var Details = new
            {
                ArtDigiOrderEntryInventory = ArtDigiOrderEntryInventoryDetails,
            };
            return Ok(Details);
        }

        // ----------- Read Artwork/Digitizing File Count ---------

        [HttpGet]
        [Route("GetArtDigiFileCount")]
        public Object GetArtDigiFileCount([FromQuery] ArtistInventoryReportDeptViewModel jobArtDigiOrderFileCount)
        {
            var ArtDigiFileCountDetails = _artistInventoryReportService.GetArtDigiFileCount(jobArtDigiOrderFileCount);
            var Details = new
            {
                ArtDigiFileCount = ArtDigiFileCountDetails,
            };
            return Ok(Details);
        }

    }
}
