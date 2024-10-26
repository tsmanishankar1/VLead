using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class HolidayService
    {
      
        public HolidayService(TableWork tableWork)
        {
            _tableWork = tableWork;
            tow = tableWork;
            
        }
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
        public string AddHoliday(Holiday AddHoliday)
        {
            string Message = "";
            if (AddHoliday != null)
            {
                try
                {
                    var CheckAlreadyExist = _tableWork.HolidayRepository.Get(x => x.HolidayDate == AddHoliday.HolidayDate && x.IsDeleted == false).ToList();
                    if (CheckAlreadyExist.Count == 0)
                    {
                        Holiday saveholiday = new Holiday();
                        saveholiday.HolidayDate = AddHoliday.HolidayDate;
                        saveholiday.HolidayDescription = AddHoliday.HolidayDescription;
                        saveholiday.IsDeleted = false;
                        saveholiday.CreatedBy = AddHoliday.CreatedBy;
                        saveholiday.CreatedUtc = DateTime.UtcNow;
                        _tableWork.HolidayRepository.Insert(saveholiday);
                        long dbstatus = _tableWork.SaveChanges();
                        Message = "Holiday Saved Successfully....!";
                    }
                    else
                    {
                        Message = "Holiday Record is there already....!";
                    }
                }
                catch (Exception e)
                {
                    Log savelog = new Log();
                    savelog.Module = "Holiday Master error in catch - HolidayMaster";
                    savelog.Description = e.Message + " " + e.InnerException + " " + e.StackTrace + " " + e.Source;
                    savelog.Type = "Error";
                    savelog.CreatedUtc = DateTime.UtcNow;
                    //_logRepository = ServiceLocator.LogRepository();
                    //_logRepository.AddLog(savelog);

                }
            }
            return Message;
        }
        public IEnumerable<Holiday> GetHoliday()
        {
         
                return tow.HolidayRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();
          
        }

        public string UpdateHoliday(Holiday EditList)
        {
            var Message = "";

            try
            {
                if (EditList != null)
                {
                   
                        var existholiday = tow.HolidayRepository.GetSingle(X => X.Id == EditList.Id);
                        existholiday.HolidayDate = EditList.HolidayDate;
                        existholiday.HolidayDescription = EditList.HolidayDescription;
                        existholiday.IsDeleted = false;
                        existholiday.UpdatedBy = EditList.UpdatedBy;
                        existholiday.UpdatedUtc = DateTime.UtcNow;
                        tow.HolidayRepository.Update(existholiday);
                        long dbstatus = tow.SaveChanges();
                        Message = "Updated Saved Successfully....!";
                    }
              
            }
            catch (Exception e)
            {
                Log savelog = new Log();
                savelog.Module = "Holiday Master error in catch - HolidayMaster";
                savelog.Description = e.Message + " " + e.InnerException + " " + e.StackTrace + " " + e.Source;
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                //_logRepository = ServiceLocator.LogRepository();
                //_logRepository.AddLog(savelog);
            }

            return Message;
        }
        public string DeleteHoliday(int Id)
        {
            var Message = "";
            if (Id != null)
            {
                try
                {
                  
                        var existingholiday = tow.HolidayRepository.GetSingle(x => x.Id == Id);
                        existingholiday.IsDeleted = true;
                        tow.HolidayRepository.Update(existingholiday);
                        long dbstatus = tow.SaveChanges();
                        Message = "Deleted Successfully";
                  
                }

                catch (Exception e)
                {
                    Log savelog = new Log();
                    savelog.Module = "Holiday Master error in catch - HolidayMaster";
                    savelog.Description = e.Message + " " + e.InnerException + " " + e.StackTrace + " " + e.Source;
                    savelog.Type = "Error";
                    savelog.CreatedUtc = DateTime.UtcNow;
                    //_logRepository = ServiceLocator.LogRepository();
                    //_logRepository.AddLog(savelog);
                }
            }
            return Message;
        }
        public Holiday GetEditHoliday(int Id)
        {
            return _tableWork.HolidayRepository.GetSingle(x => x.Id == Id);
        }
    }
}
