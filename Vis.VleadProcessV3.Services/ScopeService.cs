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
    public class ScopeService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
       
        public ScopeService(TableWork tableWork)
        {
           
            tow = tableWork;
            _tableWork = tableWork;
          
        }
        public IEnumerable<ScopeViewModel> GetAllScopeList()
        {
            return _tableWork.ScopeRepository.GetAllVal(x => x.Department).Where(x => x.IsDeleted == false).Select(x => new ScopeViewModel { Id = x.Id, Description = x.Description, DepartmentDescription = x.Department.Description });
        }
        public Scope GetScopeDetails(int Id)
        {
       
                return tow.ScopeRepository.GetAllVal(x => x.Department).Where(x => x.Id == Id).FirstOrDefault();
           
            
        }
       
        public bool CreateScope(Scope1 scope1)
        {
            bool status = false;
            if (scope1 != null)
            {
                var checkExists = _tableWork.ScopeRepository.GetSingle(x => x.Description == scope1.Description && x.IsDeleted == false && x.DepartmentId == scope1.DepartmentId);
                if (checkExists == null)
                {
                    try
                    {
                        Scope scope = new Scope() {DepartmentId=scope1.DepartmentId,Description=scope1.Description,CreatedBy=scope1.CreatedBy};
                        scope.CreatedUtc = DateTime.UtcNow;
                        scope.IsDeleted = false;
                        _tableWork.ScopeRepository.Insert(scope);
                        long dbstatus = _tableWork.SaveChanges();
                        status = dbstatus > 0;
                    }
                    catch (Exception ex)
                    {
                        status = false;
                    }
                }
                else
                {
                    status = false;
                }
            }
            return status;
        }
        public bool UpdateScope(Scope scope)
        {
            bool status = false;
            try
            {
              
                    var checkdetails = tow.ScopeRepository.GetSingle(x => x.Description == scope.Description && x.DepartmentId == scope.DepartmentId && x.IsDeleted == false);
                    if (checkdetails != null)
                    {
                        status = false;
                    }
                    else
                    {
                        var updateScopeDetails = tow.ScopeRepository.GetSingle(x => x.Id == scope.Id);
                        updateScopeDetails.DepartmentId = scope.DepartmentId;
                        updateScopeDetails.Description = scope.Description;
                        updateScopeDetails.NeedTraining = scope.NeedTraining;
                        updateScopeDetails.UpdatedUtc = DateTime.UtcNow;
                        updateScopeDetails.UpdatedBy = scope.UpdatedBy;
                        updateScopeDetails.IsDeleted = false;
                        updateScopeDetails.Description = scope.Description;
                        tow.ScopeRepository.Update(updateScopeDetails);
                        long dbstatus = tow.SaveChanges();
                        if (dbstatus != 0)
                        {
                            status = true;
                        }
                    }
              
            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            return status;
        }
        public bool RemoveScope(int Id)
        {
            bool status = false;
            try
            {
              
                    var existingScope = tow.ScopeRepository.GetSingle(x => x.Id == Id);
                    existingScope.IsDeleted = true;
                    tow.ScopeRepository.Update(existingScope);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
              
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public string AddBenchStatus(BenchStatus AddBenchStatus)
        {
            string Message = "";
            var CheckAlreadyExist = _tableWork.BenchStatusRepository.Get(x => x.Description == AddBenchStatus.Description && x.IsDeleted == false).ToList();
            if (CheckAlreadyExist.Count == 0)
            {
                if (AddBenchStatus.Description != null)
                {
                    try
                    {
                        BenchStatus saveBenchStatus = new BenchStatus();
                        saveBenchStatus.Description = AddBenchStatus.Description;
                        saveBenchStatus.Division = AddBenchStatus.Division;
                        saveBenchStatus.CreatedBy = AddBenchStatus.CreatedBy;
                        saveBenchStatus.CreatedUtc = DateTime.UtcNow;
                        saveBenchStatus.IsDeleted = false;
                        _tableWork.BenchStatusRepository.Insert(saveBenchStatus);
                        _tableWork.SaveChanges();
                        Message = "Bench Status Saved Successfully....!";
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            else
            {
                Message = "Bench Status Is Exists...!";
            }
            return Message;
        }
        public IEnumerable<BenchStatus> GetBenchStatus()
        {
            return _tableWork.BenchStatusRepository.Get(x => x.IsDeleted == false).ToList();
        }
        public BenchStatus EditBenchStatus(int Id)
        {
            return _tableWork.BenchStatusRepository.GetSingle(x => x.Id == Id);
        }
        public string UpdateBenchStatus(BenchStatus EditList)
        {
            string Message = "";
            try
            {
              
                    var updatebenchstatus = tow.BenchStatusRepository.GetSingle(x => x.Id == EditList.Id);
                    updatebenchstatus.IsDeleted = true;
                    tow.BenchStatusRepository.Update(updatebenchstatus);
                    BenchStatus saveBenchStatus = new BenchStatus();
                    saveBenchStatus.Description = EditList.Description;
                    saveBenchStatus.Division = EditList.Division;
                    saveBenchStatus.CreatedBy = EditList.CreatedBy;
                    saveBenchStatus.CreatedUtc = DateTime.UtcNow;
                    saveBenchStatus.UpdatedBy = EditList.UpdatedBy;
                    saveBenchStatus.UpdatedUtc = DateTime.UtcNow;
                    saveBenchStatus.IsDeleted = false;
                    tow.BenchStatusRepository.Insert(saveBenchStatus);
                    tow.SaveChanges();
               
                Message = "Updated Bench Status Successfully....!";
            }
            catch (Exception)
            {
                throw;
            }
            return Message;
        }
        public string RemoveBenchStatus(int Id)
        {
            string Message = "";
            try
            {
               
                    var existingBenchStatus = tow.BenchStatusRepository.GetSingle(x => x.Id == Id);
                    existingBenchStatus.IsDeleted = true;
                    tow.BenchStatusRepository.Update(existingBenchStatus);
                    tow.SaveChanges();
                    Message = "Deleted Successfully";
            
            }
            catch (Exception)
            {
                throw;
            }
            return Message;
        }
        public Object GetAllDropDownList()
        {
            var Dept = _tableWork.DepartmentRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var Desig = _tableWork.DesignationRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var Comp = _tableWork.CompanyRepository.Get(x => x.IsActive == true).AsQueryable();
            var Profi = _tableWork.ProficiencyRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var DetailList = new
            {
                DepartmentList = Dept,
                DesignationList = Desig,
                CompanyList = Comp,
                ProficiencyList = Profi,
            };
            return DetailList;
        }
        public Object ddltrainingDetailList()
        {
            var Trainig = _tableWork.ScopeRepository.Get(x => x.IsDeleted == false && x.DepartmentId == 13 && x.ScopeGroupId == null).AsQueryable();
            var DetailList = new
            {
                TrainingList = Trainig,
            };
            return DetailList;
        }
    }
}
