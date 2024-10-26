using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class LocationService
    {
        
        private readonly TableWork tow;
   
        public LocationService(TableWork tableWork)
        {
            
            tow = tableWork;
          
        }
        public IEnumerable<LocationViewModel> GetFullLocations()
        {
          
                var locationTable = new List<LocationViewModel>();
                var locality = tow.LocationRepository.Get(x => x.LocationHeaderDescription == "Location");
                if (locality != null)
                {
                    foreach (var item in locality)
                    {
                        var locationRecord = new LocationViewModel();
                        locationRecord.Locality = item.Description;
                        locationRecord.LocalityId = item.Id;
                        var city = tow.LocationRepository.GetSingle(x => x.Id == item.ContraLocationId);
                        if (city != null)
                        {
                            locationRecord.City = city.Description;
                            locationRecord.CityId = city.Id;
                            var state = tow.LocationRepository.GetSingle(x => x.Id == city.ContraLocationId);
                            if (state != null)
                            {
                                locationRecord.State = state.Description;
                                locationRecord.StateId = state.Id;
                                var country = tow.LocationRepository.GetSingle(x => x.Id == state.ContraLocationId);
                                locationRecord.Country = country.Description;
                                locationRecord.CountryId = country.Id;
                            }
                        }
                        locationTable.Add(locationRecord);
                    }
                }
                return locationTable;
        
        }
        public IEnumerable<Location> GetAllCountries()
        {
          
                return tow.LocationRepository.Get(x => x.LocationHeaderDescription == "Country").ToList();
       
        }
        public IEnumerable<Location> GetLocationsById(int locationId)
        {
            return tow.LocationRepository.Get(x => x.ContraLocationId == locationId).OrderBy(x => x.Description);
        }
        public IEnumerable<Location> GetLocationNameById(int locationId)
        {
          
            
                return tow.LocationRepository.Get(x => x.Id == locationId).AsQueryable();
            
        }
        public IEnumerable<Vis.VleadProcessV3.Models.TimeZone> GetTimeZone()
        {
         
                return tow.TimeZoneRepository.Get(x => x.IsDeleted == false).ToList();
           
        }
        public Vis.VleadProcessV3.Models.TimeZone GetTimeZomeByLocationId(int locationTimeZoneId)
        {
          
                return tow.TimeZoneRepository.GetSingle(x => x.Id == locationTimeZoneId);
           
        }
        public bool AddNewLocation(Location location)
        {
            location.IsDeleted = false;
            location.CreatedUtc = DateTime.UtcNow;
            bool status = false;
            try
            {
                if (location != null)
                {
                  
                        tow.LocationRepository.Insert(location);
                        long dbStatus = tow.SaveChanges();
                        status = dbStatus > 0;
                   
                }
            }
            catch (Exception e)
            {
            }
            return status;
        }
        public bool UpdateLocation(Location location)
        {
            bool status = false;
            try
            {
              
                    var existingLocation = tow.LocationRepository.GetSingle(x => x.Id == location.Id);
                    existingLocation.Description = location.Description;
                    existingLocation.IsDeleted = false;
                    existingLocation.UpdatedUtc = DateTime.UtcNow;
                    existingLocation.TimeZoneId = location.TimeZoneId;
                    tow.LocationRepository.Update(existingLocation);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
        public bool RemoveLocation(int locationId)
        {
            bool status = false;
            try
            {
               
                    var deleteLocation = tow.LocationRepository.GetSingle(x => x.Id == locationId);
                    tow.LocationRepository.Delete(deleteLocation);
                    long dbSaveChanges = tow.SaveChanges();
                    status = dbSaveChanges > 0;
              
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
        //Ajish Added
        public IEnumerable<Location> GetAllStates()
        {
          
                return tow.LocationRepository.Get(x => x.LocationHeaderDescription == "State").ToList();
           
        }
        public IEnumerable<Location> GetAllCity()
        {
          
                return tow.LocationRepository.Get(x => x.LocationHeaderDescription == "City").ToList();
        
        }
        public Object GetTimeZoneDetails()
        {
           
                return tow.TimeZoneRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Description).ToList();
          
        }
    }
}
