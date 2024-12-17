using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class InventoryService
    {
        private readonly ProcedureWork _procedureWork;// = new ProcedureWork();
        private readonly ApplicationDbContext _context;
        private readonly TableWork _tableWork;
        private readonly TableWork _tbWork;

        public InventoryService(ProcedureWork procedureWork, ApplicationDbContext dbContext, TableWork tableWork, TableWork tbWork)
        {
            //_configuration= configuration;
            //_unitWork = unitWork;// new UnitWork(configuration);
            _procedureWork = procedureWork;// new ProcedureWork(configuration);
            _context = dbContext;
            _tableWork = tableWork;
            _tbWork=tbWork;
        }

        public Object GetInventory(InventoryViewModel jobInventory)
        {
            //var NoOfDays = DateTime.UtcNow.AddDays(-jobOrder.TransactionId);
            var jobMovement = new List<InventoryForm_Result>();

            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@DivisionId",jobInventory.DivisionId),
                new SqlParameter("@CustomerId",jobInventory.CustomerId),
                new SqlParameter("@FromDate",jobInventory.FromDate),
                new SqlParameter("@ToDate",jobInventory.ToDate)
            };

            jobMovement = _procedureWork.ExecStoredProcedure<InventoryForm_Result>("InventoryForm @DivisionId, @CustomerId, @FromDate, @ToDate", sqlPara).OrderBy(x => x.JobId).ToList();

            var result = new
            {
                JobMovement = jobMovement
            };
            return result;
        }


        //public Object SubmitInventory(List<InventorySubmit_Result> jobInventorySubmit)
        //{
        //    //var NoOfDays = DateTime.UtcNow.AddDays(-jobOrder.TransactionId);
        //    //var jobMovement = new List<InventoryForm_Result>();

        //    //var Employee = jobInventorySubmit.ToList();
        //    foreach (var inventorySubmitViewModel in jobInventorySubmit)
        //    {
        //        var sqlParam = new SqlParameter[]
        //    {
        //        new SqlParameter("@JobId",inventorySubmitViewModel.JobId),
        //        new SqlParameter("@DeptName",inventorySubmitViewModel.DeptName),
        //        new SqlParameter("@ShortName",inventorySubmitViewModel.ShortName),
        //        new SqlParameter("@ProjectCode",inventorySubmitViewModel.ProjectCode),
        //        new SqlParameter("@DivisionName",inventorySubmitViewModel.DivisionName),
        //        new SqlParameter("@FileName",inventorySubmitViewModel.FileName),
        //        new SqlParameter("@EffectiveFrom",inventorySubmitViewModel.EffectiveFrom),
        //        new SqlParameter("@EffectiveTo",inventorySubmitViewModel.EffectiveTo),
        //        new SqlParameter("@IsDeleted",inventorySubmitViewModel.IsDeleted),
        //        new SqlParameter("@CreatedBy",inventorySubmitViewModel.CreatedBy)
        //    };

        //        _procedureWork.ExecStoredProcedure<InventorySubmit_Result>("InventorySubmitForm @JobId, @DeptName, @ShortName , @ProjectCode, @DivisionName, @FileName, @EffectiveFrom, @EffectiveTo, @IsDeleted, @CreatedBy", sqlParam).ToList();

        //        //_procedureWork.ExecStoredProcedure<InventorySubmit_Result>("InventorySubmitForm", sqlParam).ToList();

        //    }


        //    var result = "Inserted successfuly";

        //    return result;
        //}

        public Object SubmitInventory(InventorySubmitPara jobInventorySubmit)
        {
            var invSubmitDetails = jobInventorySubmit.InventorySubmit.ToList();
            var result = new
            {
                Success = false,
                Message = " "
            };

            try
            {
                // Preload required data into dictionaries for quick lookups
                var customerDictionary = _context.Customers
                    .Where(x => invSubmitDetails.Select(v => v.ShortName).Contains(x.ShortName) && !x.IsDeleted)
                    .ToDictionary(x => x.ShortName, x => x.Id);

                var divisionDictionary = _context.Divisions
                    .Where(x => invSubmitDetails.Select(v => v.DivisionName).Contains(x.DivisionName) && x.IsDeleted == false)
                    .ToDictionary(x => x.DivisionName, x => x.Id);

                var existingInventories = _context.Inventory
                    .Where(x => invSubmitDetails.Select(v => v.JobId).Contains(x.JobId) && x.IsDeleted == false)
                    .ToList();

                var jobOrdersToUpdate = new List<JobOrder>();
                var inventoriesToUpdate = new List<Inventory>();
                var inventoriesToInsert = new List<Inventory>();

                // HashSets for faster existence checks
                var jobIds = new HashSet<string>(existingInventories.Select(x => x.JobId));

                foreach (var inventorySubmitViewModel in invSubmitDetails)
                {
                    if (!jobIds.Contains(inventorySubmitViewModel.JobId))
                    {
                        inventoriesToInsert.Add(new Inventory
                        {
                            JobId = inventorySubmitViewModel.JobId,
                            DeptName = inventorySubmitViewModel.DeptName,
                            DepartmentId = inventorySubmitViewModel.DepartmentId,
                            CustId = customerDictionary.TryGetValue(inventorySubmitViewModel.ShortName, out var custId) ? custId : (int?)null,
                            ShortName = inventorySubmitViewModel.ShortName,
                            ProjectCode = inventorySubmitViewModel.ProjectCode,
                            DivisionId = divisionDictionary.TryGetValue(inventorySubmitViewModel.DivisionName, out var divisionId) ? divisionId : (int?)null,
                            DivisionName = inventorySubmitViewModel.DivisionName,
                            FileName = inventorySubmitViewModel.FileName,
                            EffectiveFrom = jobInventorySubmit.EffectiveFrom,
                            EffectiveTo = jobInventorySubmit.EffectiveTo,
                            DateOfDelivery = jobInventorySubmit.DateOfDelivery,
                            Remarks = jobInventorySubmit.Remarks,
                            IsDeleted = false,
                            CreatedBy = inventorySubmitViewModel.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        // Update JobOrder and existing Inventory records if necessary
                        var checkJobOrder = _tbWork.JobOrderRepository
                            .GetSingle(x => x.JobId == inventorySubmitViewModel.JobId && !x.IsDeleted);

                        if (checkJobOrder != null)
                        {
                            checkJobOrder.DateofDelivery = jobInventorySubmit.DateOfDelivery;
                            checkJobOrder.UpdatedBy = Convert.ToInt32(inventorySubmitViewModel.CreatedBy);
                            checkJobOrder.UpdatedUtc = DateTime.UtcNow;
                            jobOrdersToUpdate.Add(checkJobOrder);
                        }

                        var existingInventoryRecords = existingInventories
                            .Where(x => x.JobId == inventorySubmitViewModel.JobId && x.IsDeleted == false)
                            .ToList();

                        foreach (var item in existingInventoryRecords)
                        {
                            item.IsDeleted = true;
                            item.UpdatedBy = inventorySubmitViewModel.CreatedBy;
                            item.UpdatedUtc = DateTime.UtcNow;
                            inventoriesToUpdate.Add(item);
                        }

                        inventoriesToInsert.Add(new Inventory
                        {
                            JobId = inventorySubmitViewModel.JobId,
                            DeptName = inventorySubmitViewModel.DeptName,
                            DepartmentId = inventorySubmitViewModel.DepartmentId,
                            CustId = customerDictionary.TryGetValue(inventorySubmitViewModel.ShortName, out var custId) ? custId : (int?)null,
                            ShortName = inventorySubmitViewModel.ShortName,
                            ProjectCode = inventorySubmitViewModel.ProjectCode,
                            DivisionId = divisionDictionary.TryGetValue(inventorySubmitViewModel.DivisionName, out var divisionId) ? divisionId : (int?)null,
                            DivisionName = inventorySubmitViewModel.DivisionName,
                            FileName = inventorySubmitViewModel.FileName,
                            EffectiveFrom = jobInventorySubmit.EffectiveFrom,
                            EffectiveTo = jobInventorySubmit.EffectiveTo,
                            DateOfDelivery = jobInventorySubmit.DateOfDelivery,
                            IsDeleted = false,
                            CreatedBy = inventorySubmitViewModel.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        });
                    }
                }

                // Bulk update and insert in a single transaction
                if (jobOrdersToUpdate.Any())
                    _context.JobOrders.UpdateRange(jobOrdersToUpdate);

                if (inventoriesToUpdate.Any())
                    _context.Inventory.UpdateRange(inventoriesToUpdate);

                if (inventoriesToInsert.Any())
                    _context.Inventory.AddRange(inventoriesToInsert);

                _context.SaveChanges();

                result = new
                {
                    Success = true,
                    Message = "InventoryForm inserted successfully!"
                };
            }
            catch (Exception ex)
            {
                result = new
                {
                    Success = false,
                    Message = ex.Message
                };
            }

            return result;
        }
    }
}
