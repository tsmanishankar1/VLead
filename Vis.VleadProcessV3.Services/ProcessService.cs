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
    public class ProcessService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow; 
  
        public ProcessService(TableWork tableWork)
        {
            tow=tableWork; 
           
            _tableWork = tableWork;
         
        }
        public IEnumerable<Process> GetAllProcess()
        {
         
                return tow.ProcessRepository.Get(x => x.IsActive == true).ToList();
         
        }
        public bool CreateProcess(Process Process)
        {
            bool status = false;
            if (Process != null)
            {
                try
                {
                    Process.IsActive = true;
                    Process.CreatedUtc = DateTime.UtcNow;
                    _tableWork.ProcessRepository.Insert(Process);
                    long dbstatus = _tableWork.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {
                }
            }
            return status;
        }
        public Process GetProcessDetails(int Id)
        {
            return _tableWork.ProcessRepository.GetSingle(x => x.Id == Id);
        }
        public bool UpdateProcess(Process process)
        {
            bool status;
            try
            {
           
                    var UpdateProcessDetails = tow.ProcessRepository.GetSingle(x => x.Id == process.Id);
                    UpdateProcessDetails.Name = process.Name;
                    UpdateProcessDetails.ShortName = process.ShortName;
                    UpdateProcessDetails.Description = process.Description;
                    UpdateProcessDetails.IsActive = process.IsActive;
                    UpdateProcessDetails.UpdatedUtc = DateTime.UtcNow;
                    tow.ProcessRepository.Update(UpdateProcessDetails);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
              

            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool RemoveProcess(int Id)
        {
            bool status = false;
            try
            {
               
                    var existingProcess = tow.ProcessRepository.GetSingle(x => x.Id == Id);
                    tow.ProcessRepository.Delete(existingProcess);
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
