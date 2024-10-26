using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class TimeZoneService
    {
 
        private readonly TableWork tow;
    
        public TimeZoneService(TableWork tableWork)
        {
          
            tow = tableWork;
          
        }
        public IEnumerable<Vis.VleadProcessV3.Models.TimeZone> GetAllTimeZone()
        {
            return tow.TimeZoneRepository.Get(x => x.IsDeleted == false).ToList();
        }
        public bool CreateTimeZone(Vis.VleadProcessV3.Models.TimeZone timeZone)
        {
            bool status = false;
            if (timeZone != null)
            {
                try
                {
                    timeZone.Name = timeZone.Description;
                    timeZone.IsDeleted = false;
                    tow.TimeZoneRepository.Insert(timeZone);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {
                }
            }
            return status;
        }
        public Vis.VleadProcessV3.Models.TimeZone GetTimeZoneDetails(int Id)
        {
            return tow.TimeZoneRepository.GetSingle(x => x.Id == Id);
        }
        public bool UpdateTimeZone(Vis.VleadProcessV3.Models.TimeZone timeZone)
        {
            bool status;
            try
            {
           
                    var updateTimeZone = tow.TimeZoneRepository.GetSingle(x => x.Id == timeZone.Id);
                    updateTimeZone.Description = timeZone.Description;
                    updateTimeZone.Istdiff = timeZone.Istdiff;
                    updateTimeZone.TimezoneDiff = timeZone.TimezoneDiff;
                    tow.TimeZoneRepository.Update(updateTimeZone);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool RemoveTimeZone(int Id)
        {
            bool status = false;
            try
            {
             
                    var existingTimeZone = tow.TimeZoneRepository.GetSingle(x => x.Id == Id);
                    existingTimeZone.IsDeleted = true;
                    tow.TimeZoneRepository.Update(existingTimeZone);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
    }
}
