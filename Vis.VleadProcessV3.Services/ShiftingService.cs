using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class ShiftingService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
      

        public ShiftingService(TableWork tableWork)
        {
             tow = tableWork;
            _tableWork = tableWork;
            
        }
        public IEnumerable<Shift> GetAllShift()
        {
          
                return tow.ShiftRepository.GetAll().ToList();
        
        }
        public bool CreateShifting(Shift shiftDetails)
        {
            bool status = false;
            if (shiftDetails != null)
            {
                try
                {
                    _tableWork.ShiftRepository.Insert(shiftDetails);
                    long dbstatus = _tableWork.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {
                }
            }
            return status;
        }
        public Shift GetShiftDetails(int Id)
        {
         
                return tow.ShiftRepository.GetSingle(x => x.Id == Id);
         
        }
        public bool UpdateShifting(Shift shift)
        {
            bool status;
            try
            {
              
                    var updateshiftDetails = tow.ShiftRepository.GetSingle(x => x.Id == shift.Id);
                    updateshiftDetails.Description = shift.Description;
                    updateshiftDetails.ShiftType = shift.ShiftType;
                    updateshiftDetails.FromTime = shift.FromTime;
                    updateshiftDetails.ToTime = shift.ToTime;
                    tow.ShiftRepository.Update(updateshiftDetails);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool RemoveShifting(int Id)
        {
            bool status = false;
            try
            {
               
                    var existingShifts = tow.ShiftRepository.GetSingle(x => x.Id == Id);
                    tow.ShiftRepository.Delete(existingShifts);
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
