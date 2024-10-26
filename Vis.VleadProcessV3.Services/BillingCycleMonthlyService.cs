using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class BillingCycleMonthlyService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext db;
        private readonly TableWork tb;
        private readonly TableWork tow;//= new TableWork();
        private readonly ProcedureWork _procedureWork;

        public BillingCycleMonthlyService(ApplicationDbContext dbContext, TableWork tableWork,ProcedureWork procedureWork)
        {
            db = dbContext;
            tb = tableWork;
            tow = tableWork;
            _procedureWork = procedureWork;
        }

        public BillingCycleMonthly CreateInvoiceBilling(BillingCycleMonthly bill)
        {
            try
            {
                // Check if a record with the same BillingDate, CustomerId, and DepartmentId exists.
                bool isDuplicate = db.BillingCycleMonthlies
                    .Any(b => b.BillingDate == bill.BillingDate &&
                              b.CustomerId == bill.CustomerId &&
                              b.DepartmentId == bill.DepartmentId &&
                              b.IsDelete == false);

                if (isDuplicate)
                {
                    // Handle the case where a duplicate record already exists.
                    // You can throw an exception, return an error response, or take appropriate action.
                    throw new Exception("Record already exists.");
                }

                //if (bill )
                //{
                //    throw new Exception("Please vaild datatype");
                //}

                // No duplicate record found, proceed with creating the new record.
                var createInvoiceBilling = new BillingCycleMonthly
                {
                    CustomerId = bill.CustomerId,
                    DepartmentId = bill.DepartmentId,
                    BillingDate = bill.BillingDate,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = bill.CreatedBy,
                  
                };
                db.BillingCycleMonthlies.Add(createInvoiceBilling);
                db.SaveChanges();
                return createInvoiceBilling;
            }
            catch (Exception ex)
            {
                throw new Exception("Uable to create a record." + ex.Message);
            }
        }

        public BillingCycleMonthly DeleteBilling(BillingCycleMonthly billing, int id)
        {
            try
            {
                if (id != null)
                {
                    var check = db.BillingCycleMonthlies.FirstOrDefault(x => x.Id == id && x.IsDelete == false);
                    if (check != null)
                    {
                        check.IsDelete = true;
                        check.UpdateBy = billing.UpdateBy;
                        check.UpadatedUtc = DateTime.Now;
                        db.SaveChanges();
                        return check;
                    }
                    else
                    {
                        throw new Exception("record is already deleted.");
                    }
                }
                else
                {
                    throw new Exception("id not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Upable to delete the records due to " + ex.Message);
            }
        }
        public BillingCycleMonthly UpdataionBilling(BillingCycleMonthly billing, int id)
        {
            bool flag = false;
            try
            {
                if (id != null)
                {
                    var check = db.BillingCycleMonthlies.FirstOrDefault(x => x.Id == id && x.IsDelete == false);
                    if (check != null)
                    {
                        if (check.CustomerId != billing.CustomerId)
                        {
                            check.CustomerId = billing.CustomerId;
                            check.UpadatedUtc = DateTime.UtcNow;
                            check.UpdateBy = billing.UpdateBy;
                            flag = true;
                        }
                        if (check.DepartmentId != billing.DepartmentId)
                        {
                            check.DepartmentId = billing.DepartmentId;
                            check.UpadatedUtc = DateTime.UtcNow;
                            check.UpdateBy = billing.UpdateBy;
                            flag = true;
                        }
                        if (check.BillingDate != billing.BillingDate)
                        {
                            check.BillingDate = billing.BillingDate;
                            check.UpadatedUtc = DateTime.UtcNow;
                            check.UpdateBy = billing.UpdateBy;
                            flag = true;
                        }
                        if (flag == false)
                        {
                            throw new Exception("Please enter the updated reocord");
                        }
                        db.SaveChanges();
                        return check;
                    }
                    else
                    {
                        throw new Exception("no records found");
                    }
                }
                else
                {
                    throw new Exception("No id  matched");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update recored: " + ex.Message);
            }
        }
          
     
        
        public IEnumerable<object> GetData(int? clientId, int? departmentId, DateTime? billingDate)
        {
            try
            {
                IEnumerable<Getclientidanddepartmentidforbillingcycle> result=Array.Empty<Getclientidanddepartmentidforbillingcycle>();
                if (clientId != null && departmentId != null && billingDate != null)
                {
                    List<Getclientidanddepartmentidforbillingcycle> namesList = new List<Getclientidanddepartmentidforbillingcycle>();
                    var data = tb.BillingCycleMonthlyRepository.Get(a => a.CustomerId == clientId && a.DepartmentId == departmentId && a.BillingDate == billingDate && a.IsDelete == false).ToList();
                    foreach (var record in data)
                    {
                        var clientIdProcedure = record.CustomerId;
                        var departmentProcedure = record.DepartmentId;
                        var sqlPara = new SqlParameter[]
                        {
                            new SqlParameter("@ClientId",clientIdProcedure),
                            new SqlParameter("@DepartmentId",departmentId),
                        };
                        result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getclientidanddepartmentidforbillingcycle @ClientId, @DepartmentId", sqlPara);
                        var finalresult = result.Where(item => item.BillingDate== billingDate).ToList();
                        return finalresult;
                    }
                    // return namesList;
                    return result;
                }
                else if (clientId != null && departmentId != null)
                {

                    var data = tb.BillingCycleMonthlyRepository.Get(a => a.CustomerId == clientId && a.DepartmentId == departmentId && a.IsDelete == false).ToList();
                    foreach (var record in data)
                    {
                        var clientIdProcedure = record.CustomerId;
                        var departmentProcedure = record.DepartmentId;
                        var sqlPara = new SqlParameter[]
                        {
                            new SqlParameter("@ClientId",clientIdProcedure),
                            new SqlParameter("@DepartmentId",departmentId),
                        };
                        result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getclientidanddepartmentidforbillingcycle @ClientId, @DepartmentId", sqlPara);
                        // namesList.Add((Getclientidanddepartmentidforbillingcycle)name);

                        //return result;


                    }
                    return result;
                }

                else if (departmentId != null && billingDate != null)
                {
                    var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@DepartmentId",departmentId)
                    };
                    result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getDepartmentIdForBillingcycle @DepartmentId", sqlPara);
                    var finalresult = result.Where(item => item.BillingDate == billingDate).ToList();
                    return finalresult;

                }
                else if (clientId != null && billingDate != null)
                {
                    var sqlPara = new SqlParameter[]
                   {
                        new SqlParameter("@ClientId",clientId)
                   };
                    result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getclientidforbillingcycle @ClientId", sqlPara);
                    var finalresult = result.Where(item => item.BillingDate == billingDate).ToList();
                    return finalresult;
                }
                else if (clientId != null)
                {
                    var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@ClientId",clientId)
                    };
                    result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getclientidforbillingcycle @ClientId", sqlPara);

                    return result;

                }
                else if (departmentId != null)
                {
                    var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@DepartmentId",departmentId)
                    };
                    result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getDepartmentIdForBillingcycle @DepartmentId", sqlPara);
                    
                    return result;

                }
                else if (billingDate != null)
                {
                    var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@BillingDate",billingDate)
                    };
                    result = _procedureWork.ExecStoredProcedure<Getclientidanddepartmentidforbillingcycle>("getbillingdateforbillingcycle @BillingDate", sqlPara);
                    return result;
                }
                else
                {
                    
                    var display = _procedureWork.ExecProcedureOrView<Getclientidanddepartmentidforbillingcycle>("getallforbillingcycle").ToList();
                    if (display.Count <= 0)
                    {
                        throw new Exception("");
                    }

                    return display;
                    
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application
                throw new Exception("List is empty");
            }

        }
    }
}