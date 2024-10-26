using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class EmployeeVsSkillsetService
    {
       
        private readonly TableWork _tablework;
        private readonly TableWork t;
        private readonly TableWork tb;
        
        private readonly ApplicationDbContext db;
        public EmployeeVsSkillsetService(TableWork tableWork, ApplicationDbContext db)
        {
            _tablework = tableWork;
            this.db = db;
            t = tableWork;
            tb = tableWork;
        }

        //[Route("GetDropDownList")]=================================get list of employee code,name and division
        public Object GetDropDownList()
        {
            var Emp = _tablework.EmployeeRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var Div = _tablework.DivisionRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var Skill = _tablework.SkillsetRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var DetailList = new
            {
                Employeelist = Emp,
                Divisionlist = Div,
                Skilllist = Skill,
            };
            return DetailList;
        }
        //[Route("CreateEmployeeSkillset")]=================================create skillset
        public Object CreateEmployeeSkillset(ESS para)
        {
            string message = "Inserted Sucessfully";
            try
            {
                
                //var AddCountpara = para.AddCountpara;
                int EmployeeId = para.CreatedBy;
                var Emp = db.Employees.Where(x => x.EmployeeId == para.EmployeeId && x.IsDeleted == false).FirstOrDefault();
                if (Emp != null)
                {
                    var Empcheck = db.EmployeeVsSkillsets.Where(x => x.EmployeeId == para.EmployeeId && x.IsDeleted == false).FirstOrDefault();
                    if (Empcheck == null)
                    {
                        foreach (var item in para.AddCountpara)
                        {
                            var skillcheck = db.EmployeeVsSkillsets.Where(x => x.EmployeeId == para.EmployeeId && x.SkillsetId == item.SkillsetId).FirstOrDefault();
                            if (skillcheck == null)
                            {
                                EmployeeVsSkillset EmpSkill = new EmployeeVsSkillset();
                                EmpSkill.EmployeeId = para.EmployeeId;
                                EmpSkill.EmployeeCode = para.EmployeeCode;
                                EmpSkill.EmployeeName = para.EmployeeName;
                                EmpSkill.DivisionId = para.DivisionId;
                                EmpSkill.WorkingStatus = para.WorkingStatus;
                                EmpSkill.SkillsetId = item.SkillsetId;
                                EmpSkill.ProficiencyLevel = item.ProficiencyLevel;
                                EmpSkill.IsDeleted = false;
                                EmpSkill.CreatedBy = para.CreatedBy;
                                EmpSkill.CreatedUtc = DateTime.UtcNow;
                                EmpSkill.UpdatedBy = para.UpdatedBy;
                                EmpSkill.UpdatedUtc = DateTime.UtcNow;
                                db.EmployeeVsSkillsets.Add(EmpSkill);
                                db.SaveChanges();
                            }
                            if (skillcheck != null)
                            {
                                message = "Already Exists";
                            }
                        }
                    }
                    else if (Empcheck != null)
                    {
                        foreach (var item in para.AddCountpara)
                        {
                            var skillcheck = db.EmployeeVsSkillsets.Where(x => x.EmployeeId == para.EmployeeId && x.SkillsetId == item.SkillsetId).FirstOrDefault();
                            if (skillcheck == null)
                            {
                                EmployeeVsSkillset EmpSkill = new EmployeeVsSkillset();
                                EmpSkill.EmployeeId = para.EmployeeId;
                                EmpSkill.EmployeeCode = para.EmployeeCode;
                                EmpSkill.EmployeeName = para.EmployeeName;
                                EmpSkill.DivisionId = para.DivisionId;
                                EmpSkill.WorkingStatus = para.WorkingStatus;
                                EmpSkill.SkillsetId = item.SkillsetId;
                                EmpSkill.ProficiencyLevel = item.ProficiencyLevel;
                                EmpSkill.IsDeleted = false;
                                EmpSkill.CreatedBy = para.CreatedBy;
                                EmpSkill.CreatedUtc = DateTime.UtcNow;
                                EmpSkill.UpdatedBy = para.UpdatedBy;
                                EmpSkill.UpdatedUtc = DateTime.UtcNow;
                                db.EmployeeVsSkillsets.Add(EmpSkill);
                                db.SaveChanges();
                            }
                            if (skillcheck != null)
                            {
                                message = "Already Exists";
                            }
                        }                                                                               //-----for each ends
                    }                                                                                   //----else if ends
                }                                                                                       //------ Emp if ends
            }                                                                                           //-----try bolck ends
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }

        //[Route("ShowEmployeeVsSkillset")]=================================================get all EmployeeVsSkillset
        public Object ShowEmployeeVsSkillset()
        {
         
            var result = (from evs in db.EmployeeVsSkillsets
                          join emp in db.Employees on evs.EmployeeId equals emp.EmployeeId
                          join div in db.Divisions on evs.DivisionId equals div.Id
                          join skill in db.Skillsets on evs.SkillsetId equals skill.Id
                          where (evs.IsDeleted == false)
                          select new
                          {
                              Id = evs.Id,
                              EmployeeId = evs.EmployeeId,
                              EmployeeCode = evs.EmployeeCode,
                              EmployeeName = evs.EmployeeName,
                              Division = div.DivisionName,
                              Skill = skill.Description,
                              ProficiencyLevel = evs.ProficiencyLevel,
                          }).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        //[Route("Delete-Skill")]===========================================delete Skill
        public bool RemoveSkill(int Id)
        {
            bool status = false;
            try
            {
            
                    var SkillExist = t.EmployeeskillsetRepository.GetSingle(x => x.Id == Id);
                    SkillExist.IsDeleted = true;
                    t.EmployeeskillsetRepository.Update(SkillExist);
                    long dbstatus = t.SaveChanges();
                    status = dbstatus > 0;
              
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        //[Route("GetEmployeeVsSkillsetbyId")]============================get particular id details
        public object GetEmployeeVsSkillsetbyId(int Id)
        {
           
               return t.EmployeeskillsetRepository.GetAllVal(s=>s.Skillset,t=>t.Division).Where(x => x.Id == Id).Select(x =>new{ 
                   id=x.Id,
                   employeeCode=x.EmployeeCode,
                   employeeId = x.EmployeeId,
                   employeeName=x.EmployeeName,
                   divisionId=x.Division.Id,
                   divisionName=x.Division.DivisionName,
                   skillSetId=x.Skillset.Id,
                   skillSetName=x.Skillset.Description,
                   x.WorkingStatus,
                   x.ProficiencyLevel
               }).FirstOrDefault();
                // return t.EmployeeskillsetRepository.GetAllVal(x => x.EmployeeId, x => x.Skillset, x => x.DivisionId).Where(x => x.Id == Id).FirstOrDefault();
           
        }
        //[Route("UpdateEmployeeSkill")]==================================edit 
        public bool UpdateEmployeeSkill(EmployeeVsSkillset empskillset)
        {
            bool status = false;
            try
            {
              
                    var skilledit = tb.EmployeeskillsetRepository.GetSingle(x => x.EmployeeId == empskillset.EmployeeId && x.DivisionId == empskillset.DivisionId && x.SkillsetId == empskillset.SkillsetId && x.IsDeleted == false && x.ProficiencyLevel == empskillset.ProficiencyLevel);
                    if (skilledit != null)
                    {
                        status = false;
                    }
                    else
                    {
                        var UpdatesKill = tb.EmployeeskillsetRepository.GetSingle(x => x.Id == empskillset.Id);
                        UpdatesKill.EmployeeId = empskillset.EmployeeId;
                        UpdatesKill.EmployeeCode = empskillset.EmployeeCode;
                        UpdatesKill.EmployeeName = empskillset.EmployeeName;
                        UpdatesKill.DivisionId = empskillset.DivisionId;
                        UpdatesKill.WorkingStatus = empskillset.WorkingStatus;
                        UpdatesKill.SkillsetId = empskillset.SkillsetId;
                        UpdatesKill.ProficiencyLevel = empskillset.ProficiencyLevel;
                        UpdatesKill.IsDeleted = false;
                        UpdatesKill.UpdatedBy = empskillset.UpdatedBy;
                        UpdatesKill.UpdatedUtc = DateTime.UtcNow;
                        tb.EmployeeskillsetRepository.Update(UpdatesKill);
                        long dbstatus = tb.SaveChanges();
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
        //[Route("GetEmployeeCodeByEmployeeId")]======================Get employeecode using Employee id
        public Employee GetEmployeeCodeByEmployeeId(int employeeid)
        {
            return _tablework.EmployeeRepository.GetSingle(x => x.EmployeeId == employeeid);
        }
    }
}
