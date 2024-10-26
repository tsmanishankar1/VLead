using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Location = Vis.VleadProcessV3.Models.Location;
//using  Vis.VleadProcessV3.Models;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;
        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Route("GetFullLocations")]
        public IActionResult GetAllLocations()
        {
            var Locations = _locationService.GetFullLocations();
            if(Locations == null)
            {
                return NotFound("Locations Not Available");
            }
            else
            {
                return Ok(Locations);
            }
           
        }
        [HttpGet]
        [Route("GetCountries")]
        public IActionResult GetAllCountries()
        {
            var countries = _locationService.GetAllCountries();
            if (countries == null)
            {
                return NotFound("countries Not Available");
            }
            else
            {
                return Ok(countries);
            }
           
          
        }
        [HttpGet]
        [Route("GetLocationById")]
        public IActionResult GetLocationsById(int locationId)
        {
            var item = _locationService.GetLocationsById(locationId);
            if (item == null)
            {
                return NotFound("item Not Available");
            }
            else
            {
                return Ok(item);
            }
            
           
        }
        [HttpPost]
        [Route("newLocation")]
        public IActionResult AddNewLocation(Location location)
        {
            var newLocation = _locationService.AddNewLocation(location);
            if (!newLocation)
            {
                return NotFound("newLocation is not Add");
            }
            else
            {
                return Ok(newLocation);
            }
            
        }
        [HttpGet]
        [Route("GetItemById")]
        public IActionResult GetNameByLocationId(int locationId)
        {
            var item = _locationService.GetLocationNameById(locationId);
            if (item == null)
            {
                return NotFound("tem Not Available");
            }
            else
            {
                return Ok(item);
            }
          
        }
        [HttpGet]
        [Route("getTimezones")]
        public IActionResult GetTimeZones()
        {
            var timeZones = _locationService.GetTimeZone();
            if (timeZones == null)
            {
                return NotFound("TimeZones Not Available");
            }
            else
            {
                return Ok(timeZones);
            }
           
        }
        [HttpPost]
        [Route("updateLocation")]
        public IActionResult UpdateLocation(Location location)
        {
            var updateLocation = _locationService.UpdateLocation(location);
            if (!updateLocation)
            {
                return NotFound("Location is not Add");
            }
            else
            {
                return Ok(updateLocation);
            }
      
        }
        [HttpGet]
        [Route("deleteLocation")]
        public IActionResult RemoveLocation(int locationid)
        {
            var deleteLocation = _locationService.RemoveLocation(locationid);
            if (!deleteLocation)
            {
                return NotFound("Location is not Removed");
            }
            else
            {
                return Ok(deleteLocation);
            }
            
        }
        [HttpGet]
        [Route("GetTimeZoneById")]
        public IActionResult GetTimeZoneByTimeZoneId(int timeZoneId)
        {
            var item = _locationService.GetTimeZomeByLocationId(timeZoneId);
            if (item == null)
            {
                return NotFound("item Not Available");
            }
            else
            {
                return Ok(item);
            }
            
        }
        //Ajish Added
        [HttpGet]
        [Route("GetStates")]
        public IActionResult GetAllStates()
        {
            var countries = _locationService.GetAllStates();
            if (countries == null)
            {
                return NotFound("countries Not Available");
            }
            else
            {
                return Ok(countries);
            }
            
        }

        [HttpGet]
        [Route("GetCity")]
        public IActionResult GetAllCity()
        {
            var countries = _locationService.GetAllCity();
            if (countries == null)
            {
                return NotFound("countries Not Available");
            }
            else
            {
                return Ok(countries);
            }
         
        }
        [HttpGet]
        [Route("getTimeZone")]
        public IActionResult GetTimeZone()
        {
            var countries = _locationService.GetTimeZoneDetails();
            if (countries == null)
            {
                return NotFound("countries Not Available");
            }
            else
            {
                return Ok(countries);
            }
           
        }

    }
}
