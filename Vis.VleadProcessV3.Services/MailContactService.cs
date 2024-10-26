using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class MailContactService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
        private readonly ViewWork _viewWork;
        private readonly ApplicationDbContext context;
        private readonly ApplicationDbContext _db;
      
        public MailContactService(TableWork tableWork,ViewWork viewWork,ApplicationDbContext dbContext)
        {
             tow = tableWork;
            _viewWork = viewWork;
            _tableWork = tableWork;
            _db = dbContext;
            context = dbContext;
        }
        public Object GetAllDropDown()
        {
            Object Mail;
            Object process;
            Object selection;
         
                process = tow.MenuRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).Select(x => new { x.Id, x.Name }).ToList();
                selection = tow.CustomerRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).Select(x => new { x.Id, x.Name, x.ShortName }).ToList();
         
                Mail = (from u in context.Users
                        join e in context.Employees on u.EmployeeId equals e.EmployeeId
                        select new
                        {
                            u.Id,
                            u.Username,
                            Email = (e.Email == null ? u.Username : e.Email)
                        }).ToList();
         
            var department = _tableWork.DepartmentRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).Select(x => new { x.Id, x.Description }).ToList();
            var dropdownn = new
            {
                processname = process,
                clientselection = selection,
                toandccmail = Mail,
                dept = department,
            };
            return dropdownn;
        }
        private MailContact BuildAddMailContact(string processname, string clientid, string frommail, string tomail, string ccmail, int createdby)
        {
            MailContact mailcontact = new MailContact();
            mailcontact.ProcessName = processname;
            mailcontact.Client = clientid;
            mailcontact.FromMail = frommail;
            mailcontact.ToMail = tomail;
            mailcontact.Ccmail = ccmail;
            mailcontact.IsDeleted = false;
            mailcontact.CreatedBy = createdby;
            mailcontact.CreatedUtc = DateTime.UtcNow;
            return mailcontact;
        }
        public bool AddMailContact(AddMailContact AddMailcontact)
        {

            bool status = false;
            var result = new
            {
                Message = ""
            };
            try
            {
                var check1 = new MailContact();
                var check2 = new MailContact();
                var mailcontact = new MailContact();
                int clientId = Convert.ToInt32(AddMailcontact.Client);
                //var clientName = _tableWork.CustomerRepository.Get(x => x.Id == clientId).FirstOrDefault().ShortName;
                var clientName = _tableWork.CustomerRepository.Get(x => x.Id == clientId).FirstOrDefault().Name + " - " + _tableWork.CustomerRepository.Get(x => x.Id == clientId).FirstOrDefault().ShortName;
                //have to plan like, first inserting to list to the table and following to that CC have to update for to matching records, and if to records are ended, CC have to insert the records in that table

                foreach (var item in AddMailcontact.ToMail)
                {
                    check1 = _tableWork.MailContactRepository.GetSingle(x => x.ClientId == clientId && x.ProcessName == AddMailcontact.ProcessName && (x.ToMail == item || x.Ccmail == item) && x.IsDeleted == false);
                    if (check1 == null)
                    {
                        MailContact mailContactAdd = new MailContact();
                        mailContactAdd.ProcessName = AddMailcontact.ProcessName;
                        mailContactAdd.ClientId = clientId;
                        mailContactAdd.Client = clientName.ToString();
                        mailContactAdd.FromMail = AddMailcontact.FromMail;
                        mailContactAdd.DepartmentId = AddMailcontact.DepartmentId;
                        mailContactAdd.CreatedBy = AddMailcontact.CreatedBy;
                        mailContactAdd.CreatedUtc = DateTime.UtcNow;
                        mailContactAdd.UpdatedUtc = DateTime.UtcNow;
                        mailContactAdd.UpdatedBy = AddMailcontact.CreatedBy;
                        mailContactAdd.ToMail = item;
                        _tableWork.MailContactRepository.Insert(mailContactAdd);
                    }
                }
                var Id = 0;
                if (AddMailcontact.CCMail != null)
                {
                    foreach (var itemCC in AddMailcontact.CCMail)
                    {
                        var id1 = Id++;
                        var myCheck = _tableWork.MailContactRepository.Local().OrderBy(x => x.Id).Skip(id1).FirstOrDefault();

                        if (myCheck != null)
                        {
                            myCheck.Ccmail = itemCC;
                        }
                        else
                        {
                            check1 = _tableWork.MailContactRepository.GetSingle(x => x.ClientId == clientId && x.ProcessName == AddMailcontact.ProcessName && (x.ToMail == itemCC || x.Ccmail == itemCC) && x.IsDeleted == false);
                            if (check1 == null)
                            {
                                MailContact mailContactAdd = new MailContact();
                                mailContactAdd.ProcessName = AddMailcontact.ProcessName;
                                mailContactAdd.Client = clientName.ToString();
                                mailContactAdd.ClientId = clientId;
                                mailContactAdd.FromMail = AddMailcontact.FromMail;
                                mailContactAdd.DepartmentId = AddMailcontact.DepartmentId;
                                mailContactAdd.CreatedBy = AddMailcontact.CreatedBy;
                                mailContactAdd.CreatedUtc = DateTime.UtcNow;
                                mailContactAdd.UpdatedUtc = DateTime.UtcNow;
                                mailContactAdd.UpdatedBy = AddMailcontact.CreatedBy;
                                mailContactAdd.Ccmail = itemCC;
                                _tableWork.MailContactRepository.Insert(mailContactAdd);
                            }
                        }
                    }
                }

                _tableWork.SaveChanges();
                status = true;
                result = new
                {
                    Message = "Saved Successfully....!"
                };
                //}
            }
            catch
            {

            }
            return status;
        }
        public string UpdateMailContact(AddMailContact AddMailcontact)
        {

            bool status = false;
            string Message = string.Empty;
            var result = new
            {
                Message = ""
            };
            try
            {
             
                    var UpdateExistingRec = tow.MailContactRepository.GetSingle(x => x.Id == AddMailcontact.IdVal);

                    var check1 = new MailContact();
                    var check2 = new MailContact();
                    bool IsCheck = false;

                    int clientId = Convert.ToInt32(AddMailcontact.Client);
                    //var clientName = tow.CustomerRepository.GetSingle(x => x.Id == clientId).ShortName;
                    var clientName = tow.CustomerRepository.GetSingle(x => x.Id == clientId).Name + " - " + tow.CustomerRepository.GetSingle(x => x.Id == clientId).ShortName;
                    if (AddMailcontact.ToMail != null)
                    {
                        foreach (var item in AddMailcontact.ToMail)
                        {
                            check1 = tow.MailContactRepository.GetSingle(x => x.ClientId == clientId && x.ProcessName == AddMailcontact.ProcessName && (x.ToMail == item || x.Ccmail == item) && x.IsDeleted == false);
                            if (check1 == null)
                            {
                                MailContact mailContactAdd = new MailContact();
                                mailContactAdd.ProcessName = AddMailcontact.ProcessName;
                                mailContactAdd.ClientId = clientId;
                                mailContactAdd.Client = clientName.ToString();
                                mailContactAdd.FromMail = AddMailcontact.FromMail;
                                mailContactAdd.DepartmentId = AddMailcontact.DepartmentId;
                                mailContactAdd.CreatedBy = AddMailcontact.CreatedBy;
                                mailContactAdd.CreatedUtc = DateTime.UtcNow;
                                mailContactAdd.UpdatedUtc = DateTime.UtcNow;
                                mailContactAdd.UpdatedBy = AddMailcontact.CreatedBy;
                                mailContactAdd.ToMail = item;
                                tow.MailContactRepository.Insert(mailContactAdd);
                                IsCheck = true;
                            }

                        }
                        var Id = 0;
                        foreach (var itemCC in AddMailcontact.CCMail)
                        {
                            check2 = tow.MailContactRepository.GetSingle(x => x.ClientId == clientId && x.ProcessName == AddMailcontact.ProcessName && (x.Ccmail == itemCC || x.ToMail == itemCC) && x.IsDeleted == false);
                            if (check2 == null)
                            {
                                var id1 = Id++;
                             
                                    var myCheck = _db.MailContacts.Local.OrderBy(x => x.Id).Skip(id1).FirstOrDefault();
                                    if (myCheck != null)
                                    {
                                        myCheck.Ccmail = itemCC;
                                    }
                                    else
                                    {
                                        MailContact mailContactAdd = new MailContact();
                                        mailContactAdd.ProcessName = AddMailcontact.ProcessName;
                                        mailContactAdd.Client = clientName.ToString();
                                        mailContactAdd.ClientId = clientId;
                                        mailContactAdd.FromMail = AddMailcontact.FromMail;
                                        mailContactAdd.DepartmentId = AddMailcontact.DepartmentId;
                                        mailContactAdd.CreatedBy = AddMailcontact.CreatedBy;
                                        mailContactAdd.CreatedUtc = DateTime.UtcNow;
                                        mailContactAdd.UpdatedUtc = DateTime.UtcNow;
                                        mailContactAdd.UpdatedBy = AddMailcontact.CreatedBy;
                                        mailContactAdd.Ccmail = itemCC;
                                        tow.MailContactRepository.Insert(mailContactAdd);
                                        IsCheck = true;
                                    }
                                
                            }
                        }
                        if (IsCheck == true)
                        {
                            UpdateExistingRec.IsDeleted = true;
                            UpdateExistingRec.UpdatedBy = AddMailcontact.CreatedBy;
                            UpdateExistingRec.UpdatedUtc = DateTime.UtcNow;
                            tow.MailContactRepository.Update(UpdateExistingRec);
                        }
                        tow.SaveChanges();
                        if (IsCheck == true)
                        {
                            status = true;
                            //result = new
                            //{
                            Message = "Saved Successfully....!";
                            //};    
                        }
                        else
                        {
                            status = false;
                            //result = new
                            //{
                            Message = "Mail already exist";
                            //};    
                        }

                    }
                    else
                    {
                        status = false;
                        //result = new
                        //{
                        Message = "Please select To Mail/CC Mail";
                        //};

                    }
               
            }
            catch
            {

            }
            return Message;
        }

        public Object GetCustomerorEmployee(int DeptId, int clientId)
        {
            List<ViewMailContact> customercheck = new List<ViewMailContact>();

            if (DeptId != 0 && clientId != 0)
            {
                var customercheck1 = _viewWork.view_MailContactRepository.Get(x => x.Isdeleted == false && x.CustomerId == clientId).ToList();
                var customercheck2 = _viewWork.view_MailContactRepository.Get(x => x.Isdeleted == false && x.DepartmentId == DeptId).ToList();
                customercheck = customercheck1.Concat(customercheck2).ToList();
            }
            if (DeptId == 0 && clientId != 0)
            {
                customercheck = _viewWork.view_MailContactRepository.Get(x => x.Isdeleted == false && x.CustomerId == clientId).ToList();
            }
            //if (DeptId != 0 && clientId == 0)
            // {
            //     customercheck = _db.view_MailContact.Where(x => x.Isdeleted == false && x.DepartmentId == DeptId).ToList();
            // }
            //checkexits = checkexits;
            var customeroremployee = new
            {
                CustorEmp = customercheck,
            };
            return customeroremployee;
        }
        public Object GetAllMailContact()
        {
        
                var AllMailContact = tow.MailContactRepository.Get(x => x.IsDeleted == false).Select(x => new { x.Id, x.ClientId, x.ProcessName, x.ToMail, x.Ccmail, x.FromMail, x.DepartmentId, x.Client }).ToList().OrderByDescending(x => x.Id);
                var AllMailContactLists = new
                {
                    MailContact = AllMailContact,
                };
                return AllMailContactLists;
           
        }
        public Object UpdateMailContactInfo(int Id)
        {
            var res = _tableWork.MailContactRepository.GetSingle(x => x.Id == Id);
            var GetList = new
            {
                Maillist = res
            };
            return GetList;
        }
        public bool DeleteMailContactInfo(int Id, int EmpId)
        {
            bool status = false;
            try
            {
              
                    var UpdateExistingRec = tow.MailContactRepository.GetSingle(x => x.Id == Id);
                    UpdateExistingRec.IsDeleted = true;
                    UpdateExistingRec.UpdatedBy = EmpId;
                    UpdateExistingRec.UpdatedUtc = DateTime.UtcNow;
                    tow.MailContactRepository.Update(UpdateExistingRec);
                    tow.SaveChanges();
                    status = true;
            
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }
    }
}
