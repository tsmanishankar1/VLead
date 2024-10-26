using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Renci.SshNet.Messages;
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
            //int counter = 0;
            var invSubmitDetails = jobInventorySubmit.InventorySubmit.ToList();
            var result = new
            {
                Success = false,
                Message = " "
            };
            try
            {
                foreach (var inventorySubmitViewModel in invSubmitDetails)
                {
                    var checkexists = _tableWork.InventoryFormSubmitRepository.GetSingle(x => x.JobId == inventorySubmitViewModel.JobId && x.IsDeleted == false);
                    if (checkexists != null)
                    {

                        result = new
                        {
                            Success = false,
                            Message = "Already Exists!"

                        };
                        continue;

                    }
                    else
                    {
                        var checkJobOrder = _tbWork.JobOrderRepository.GetSingle(x => x.JobId == inventorySubmitViewModel.JobId && x.IsDeleted == false);

                        if (checkJobOrder != null)
                        {
                            checkJobOrder.DateofDelivery = jobInventorySubmit.DateOfDelivery;
                            checkJobOrder.UpdatedBy = Convert.ToInt32(inventorySubmitViewModel.CreatedBy);
                            checkJobOrder.UpdatedUtc = DateTime.Now;
                            _tbWork.JobOrderRepository.Update(checkJobOrder);
                        }

                        var invForm = new Inventory
                        {
                            JobId = inventorySubmitViewModel.JobId,
                            DeptName = inventorySubmitViewModel.DeptName,
                            CustId = _context.Customers.Where(x => x.ShortName == inventorySubmitViewModel.ShortName && x.IsDeleted == false).Select(x => x.Id).FirstOrDefault(),
                            ShortName = inventorySubmitViewModel.ShortName,
                            ProjectCode = inventorySubmitViewModel.ProjectCode,
                            DivisionId = _context.Divisions.Where(x => x.DivisionName == inventorySubmitViewModel.DivisionName && x.IsDeleted == false).Select(x => x.Id).FirstOrDefault(),
                            DivisionName = inventorySubmitViewModel.DivisionName,
                            FileName = inventorySubmitViewModel.FileName,
                            EffectiveFrom = jobInventorySubmit.EffectiveFrom,
                            EffectiveTo = jobInventorySubmit.EffectiveTo,
                            DateOfDelivery = jobInventorySubmit.DateOfDelivery,
                            IsDeleted = false,
                            CreatedBy = inventorySubmitViewModel.CreatedBy,
                            CreatedUTC = DateTime.Now

                        };
                        _tableWork.InventoryFormSubmitRepository.Insert(invForm);
                        result = new
                        {
                            Success = true,
                            Message = "InventoryForm inserted successfully!"

                        };

                    }
                }
                _tableWork.SaveChanges();


            }

            catch (Exception)
            {

                result = new
                {
                    Success = true,
                    Message = "Error while inserting!"

                };

                throw;
            }
            return result;
        }
    }
}
