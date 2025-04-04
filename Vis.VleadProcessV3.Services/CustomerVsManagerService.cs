using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using VisProcess.Models;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerVsManagerService
    {
        private readonly ApplicationDbContext db;

        private readonly ProcedureWork _procedureWork;
        public CustomerVsManagerService(ApplicationDbContext db, ProcedureWork procedureWork)
        {
            this.db = db;
            _procedureWork = procedureWork;
        }

        public HttpResponseMessage AssignManager(ManagerAssignRequest request, int createdById)
        {
            try
            {
                if (request != null)
                {
                    foreach (var customerId in request.CustomerIds)
                    {
                        var customer = db.CustomerVsManagers.OrderByDescending(x => x.Id)
                                           .FirstOrDefault(x => x.CustomerId == customerId);

                        if (customer == null)
                        {
                            CustomerVsManager customerVsManager = new CustomerVsManager
                            {
                                CustomerId = customerId,
                                ManagerId = request.managerId,
                                CreatedBy = createdById,
                                CreatedUtc = DateTime.UtcNow,
                                EffectiveFrom = DateTime.UtcNow
                            };

                            db.CustomerVsManagers.Add(customerVsManager);
                        }
                        else
                        {
                            customer.EffectiveTo = DateTime.UtcNow.AddDays(-1);
                            customer.IsDeleted = true;
                            customer.UpdatedBy = createdById;
                            customer.UpdatedUtc = DateTime.UtcNow;

                            db.SaveChanges();

                            CustomerVsManager customerVsManager = new CustomerVsManager
                            {
                                CustomerId = customerId,
                                ManagerId = request.managerId,
                                CreatedBy = createdById,
                                CreatedUtc = DateTime.UtcNow,
                                EffectiveFrom = DateTime.UtcNow
                            };

                            db.CustomerVsManagers.Add(customerVsManager);
                        }

                        db.SaveChanges();
                    }

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("Manager assigned successfully")
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Invalid request")
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to assign manager")
                };
            }
        }

        public HttpResponseMessage CustomerCallFollowUp(CustomerFollowUpReq form, int CreatedBy,int CustomerId)
        {
            try
            {
                if (form != null)
                {
                    CustomerCallFollowUp details = new CustomerCallFollowUp();
                    details.PurposeOfCall = form.PurposeOfCall;
                    details.ModeOfCall = form.ModeOfCall;
                    details.IsDeleted = false;
                    details.DateTimeOfCall = form.DateTimeOfCall;
                    details.Comments = form.Comments;
                    details.CreatedBy = CreatedBy;
                    details.CreatedUtc = DateTime.UtcNow;
                    details.CustomerId = CustomerId;

                    if (form.File != null && form.File.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + form.File.FileName;
                        var filePath = Path.Combine("C:\\Users\\kishorekumar.g\\source\\Vlead_Enhancement_Internal_API\\VisProcess\\CustomerFollowUp", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            form.File.CopyTo(stream);
                        }
                        details.FilePath = filePath;
                    }

                    db.CustomerCallFollowUps.Add(details);
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("Follow Up details saved succesfully")
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Enter the mandatory details correctly")
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
            }
        }

        public List<CustomerCallFollowUp> GetFollowUpReport(int managerid)
        {
            try
            {
                var result = db.CustomerCallFollowUps
                     .Where(x => x.CreatedBy == managerid && x.IsDeleted == false)
                     .ToList();
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public object FileCountDivisionWise(int managerId)
        {
            var sqlPara = new SqlParameter[]
            {
        new SqlParameter("@managerId", managerId),
            };

            var fileCounts = _procedureWork.ExecStoredProcedure<GetFileCountDivisionWiseByManagerId_Result>("GetFileCountDivisionWiseByManagerId @managerId", sqlPara);

            if (fileCounts != null)
            {
                return fileCounts.ToList();
            }
            else
            {
                return new List<GetFileCountDivisionWiseByManagerId_Result>();
            }
        }

        public object GetClientByManagerId(int managerId)
        {
            try
            {
                var sqlPara = new SqlParameter[]
                    {
                new SqlParameter("@managerId", managerId),
                    };
                        
                var clients = _procedureWork.ExecStoredProcedure<GetCustomersByManager_Result>("GetCustomersByManager @managerId", sqlPara);

                return
                clients ?? new List<GetCustomersByManager_Result>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //public IActionResult GetCustomerCallFollowUpDetailsByManagerId(int managerId)
        // {
        //    try
        //    {
        //        // Fetch the file path from the database based on the provided ID
        //        var filePath = db.CustomerCallFollowUps
        //            .Where(x => x.CreatedBy == managerId && x.IsDeleted == false)
        //            .Select(x => x.FilePath)
        //            .FirstOrDefault();

        //        if (filePath != null && System.IO.File.Exists(filePath))
        //        {
        //            // Read the file content into a byte array
        //            var fileBytes = System.IO.File.ReadAllBytes(filePath);

        //            // Get the file name from the path
        //            var fileName = Path.GetFileName(filePath);

        //            // Return the file as a response
        //            return File(fileBytes, "application/octet-stream", fileName);
        //        }
        //        else
        //        {
        //            return NotFound("File not found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions appropriately
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        public object GetInactiveClientsWithLastReceivedFileDateByManagerId(int managerId)
        {
            var sqlPara = new SqlParameter[]
              {
        new SqlParameter("@managerId", managerId),
              };

            var clients = _procedureWork.ExecStoredProcedure<GetInactiveClientsScopeWiseByManagerId_result>("GetInactiveClientsScopeWiseByManagerId @managerId", sqlPara);

            if (clients != null)
            {
                return clients.ToList();
            }
            else
            {
                return new List<GetInactiveClientsScopeWiseByManagerId_result>();
            }

        }

        public object GetLastReceivedFileDateOfCustomerByManagerId(int managerId)
        {
            try
            {
                var clients = db.CustomerVsManagers
                             .Where(x => x.ManagerId == managerId && x.IsDeleted == false)
                             .Select(x => x.Customer)
                             .ToList();

                //if (clients.Count <= 0)
                //{
                //    return ("Client list is empty");
                //}

                var clientDetails = clients
                    .Select(client => new
                    {
                        Client = client,
                        LatestFileReceivedDate = db.JobOrders
                            .Where(jo => jo.ClientId == client.Id)
                            .OrderByDescending(jo => jo.FileReceivedDate)
                            .Select(jo => jo.FileReceivedDate)
                            .FirstOrDefault()
                    })
                    .OrderByDescending(clientDetail => clientDetail.LatestFileReceivedDate)
                    .ToList();

                return clientDetails;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("There is a problem in getting the details")
                };
            }

        }

        public List<StatusMaster> GetStatusMasterDropdownList()
        {
            try
            {
                var dropdownList = db.StatusMasters
                    .Where(x => x.IsActive == true)
                    .ToList();

                if (dropdownList.Count > 0)
                {
                    return dropdownList;
                }
                else
                {
                    return new List<StatusMaster>();
                }
            }
            catch (Exception ex)
            {
                return new List<StatusMaster>();
            }
        }


        public List<GetInactiveClients_Result> InactiveClientsByManagerId(int managerId)
        {
            var sqlPara = new SqlParameter[]
                    {
                new SqlParameter("@managerId", managerId),
                    };
            var result = _procedureWork.ExecStoredProcedure<GetInactiveClients_Result>("GetInactiveClients @managerId", sqlPara);
            if (result != null)
            {
                return result.ToList();
            }
            else
            {
                return new List<GetInactiveClients_Result>();
            }

        }

        public List<Employee> GetAccountManagerList()
        {
            try
            {
                var employees = db.Employees
                    .Where(x => x.IsDeleted == false && (x.Designation.Description == "Account Manager" || x.DepartmentId == 9))
                    .Include(x => x.Designation)
                    .ToList();
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<object> GetMappedCustomerList()
        {
            try
            {
                var customers = db.CustomerVsManagers
                    .Where(x => x.IsDeleted == false)
                    .Join(db.Employees,
                    manager=>manager.ManagerId,
                    employee=>employee.EmployeeId,
                    (manager, employee) =>new
                    {
                        Manager = manager,
                        Employee = employee
                    })
                    .Join(db.Customers,
                    combined=>combined.Manager.CustomerId,
                    customer=>customer.Id,
                    (combined, customer) => new
                    {
                        Manager = combined.Manager,
                        Employee = combined.Employee,
                        Customer = customer
                    })
                    .Select(result=> (object) new
                    {
                        CustomerId = result.Manager.CustomerId,
                        CustomerName= result.Customer.Name,
                        CustomerShortName = result.Customer.ShortName,
                        ManagerId = result.Manager.ManagerId,
                        ManagerName = result.Employee.EmployeeName,
                        CreatedBy = result.Manager.CreatedBy,
                        IsDeleted = result.Manager.IsDeleted,
                        EffectiveFrom = result.Manager.EffectiveFrom,
                        EffectiveTo = result.Manager.EffectiveTo,
                    })
                    .ToList();
                return customers;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<object> GetUnMappedCustomerList()
        {
            try
            {
                var customers = db.Customers
                    .Where(customer => !db.CustomerVsManagers
                    .Any(manager => manager.CustomerId == customer.Id && manager.IsDeleted == false && customer.IsApproved == true) &&
                    customer.IsDeleted == false)
                    .Select(result => (object)new
                    {
                        CustomerId = result.Id,
                        CustomerName = result.Name,
                        CShortName = result.ShortName,
                        isDeleted = result.IsDeleted,
                    })
                    .ToList<object>();
                return customers;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<object> GetAllCustomersList()
        {
            try
            {
                var customers = db.Customers
                    .Where(x => x.IsDeleted == false && x.IsApproved == true)
                    .Select(result =>(object) new
                    {
                        CustomerId = result.Id,
                        CustomerName = result.Name,
                        CustomerShortName = result.ShortName,
                        isDeleted = result.IsDeleted,

                    })
                    .ToList();
                return customers;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public object GetFileCountForGivenRangeByManagerId(GetFileCountReq request)
        {

            try
            {

                if (!string.IsNullOrEmpty(request.period) && request.CustomerIds != null && request.CustomerIds.Any() && request.managerId !=null)
                {
                    var sqlPara = new SqlParameter[]
                    {
                new SqlParameter("@managerId", request.managerId),
                new SqlParameter("@Period", request.period)
                    };
                    var counts = _procedureWork.ExecStoredProcedure<GetFileCountGivenRange_Result>("GetFileCountByTimeRange @ManagerId, @Period", sqlPara);
                    var result = counts.Where(x => request.CustomerIds.Contains(x.CustomerId)).ToList();
                    return result;
                }
                else if(request.fromDate != null && request.toDate != null && request.managerId != null && request.CustomerIds != null && request.CustomerIds.Any()) 
                {
                    var sqlPara = new SqlParameter[]
                    {
                new SqlParameter("@FromDate", request.fromDate),
                new SqlParameter("@ToDate", request.toDate),
                new SqlParameter("@managerId", request.managerId)
                    };
                    var counts = _procedureWork.ExecStoredProcedure<GetFileCountGivenRange_Result>("GetFileCountGivenRange @FromDate, @ToDate, @ManagerId", sqlPara);
                    var result = counts.Where(x => request.CustomerIds.Contains(x.CustomerId)).ToList();
                    return result;
                }
                else if(request.fromDate != null && request.toDate != null && request.managerId != null)
                {
                    var sqlPara = new SqlParameter[]
                   {
                new SqlParameter("@FromDate", request.fromDate),
                new SqlParameter("@ToDate", request.toDate),
                new SqlParameter("@managerId", request.managerId)
                   };
                    return _procedureWork.ExecStoredProcedure<GetFileCountGivenRange_Result>("GetFileCountGivenRange @FromDate, @ToDate, @ManagerId", sqlPara);
                }
                else if(!string.IsNullOrEmpty(request.period) && request.managerId != null)
                {
                    var sqlPara = new SqlParameter[]
                    {
                new SqlParameter("@managerId", request.managerId),
                new SqlParameter("@Period", request.period)
                    };
                    return _procedureWork.ExecStoredProcedure<GetFileCountGivenRange_Result>("GetFileCountByTimeRange @ManagerId, @Period", sqlPara);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

