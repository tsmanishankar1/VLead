using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class VoucherService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
      
        public VoucherService(TableWork tableWork)
        {
            _tableWork = tableWork;
            tow = tableWork;
            
        }
        public IEnumerable<VoucherControl> GetAllVoucherList()
        {
          
                var temp = tow.VoucherControlRepository.GetAllVal(x => x.Department, x => x.Transaction).Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                return temp;
          
        }
        public bool CreateVoucherList(VoucherControl voucherControl)
        {
            bool status = false;
            if (voucherControl != null)
            {
                try
                {
                    voucherControl.Cocode = 1;
                    voucherControl.IsActive = true;
                    voucherControl.CreatedUtc = DateTime.UtcNow;
                    voucherControl.UpdatedUtc = DateTime.UtcNow;
                    voucherControl.IsDeleted = false;
                    _tableWork.VoucherControlRepository.Insert(voucherControl);
                    long dbstatus = _tableWork.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {
                }
            }
            return status;
        }
        public VoucherControl GetVoucherDetails(int Id)
        {
          
                return tow.VoucherControlRepository.GetAllVal(x => x.Department, x => x.Transaction).Where(x => x.Id == Id).FirstOrDefault();
          
        }
        public bool UpdateVoucher(VoucherControl voucherControl)
        {
            bool status;
            try
            {
              
                    var UpdateVoucherDetails = tow.VoucherControlRepository.GetSingle(x => x.Id == voucherControl.Id);
                    UpdateVoucherDetails.TransactionId = voucherControl.TransactionId;
                    UpdateVoucherDetails.DepartmentId = voucherControl.DepartmentId;
                    UpdateVoucherDetails.Cocode = voucherControl.Cocode;
                    UpdateVoucherDetails.Prefix = voucherControl.Prefix;
                    UpdateVoucherDetails.Suffix = voucherControl.Suffix;
                    UpdateVoucherDetails.Autonumber = voucherControl.Autonumber;
                    UpdateVoucherDetails.Voucherno = voucherControl.Voucherno;
                    UpdateVoucherDetails.EffectiveFrom = voucherControl.EffectiveFrom;
                    UpdateVoucherDetails.EffectiveTo = voucherControl.EffectiveTo;
                    UpdateVoucherDetails.IsDeleted = voucherControl.IsDeleted;
                    UpdateVoucherDetails.CreatedBy = voucherControl.CreatedBy;
                    UpdateVoucherDetails.UpdatedUtc = DateTime.UtcNow;
                    UpdateVoucherDetails.CreatedBy = voucherControl.CreatedBy;
                    UpdateVoucherDetails.UpdatedBy = voucherControl.UpdatedBy;
                    UpdateVoucherDetails.IsActive = voucherControl.IsActive;
                    tow.VoucherControlRepository.Update(UpdateVoucherDetails);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
              
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool RemoveVoucher(int Id)
        {
            bool status = false;
            try
            {
                    var existingVoucher = tow.VoucherControlRepository.GetSingle(x => x.Id == Id);
                    existingVoucher.IsDeleted = true;
                    existingVoucher.IsActive = false;
                    tow.VoucherControlRepository.Update(existingVoucher);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
              
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public IEnumerable<TransactionType> GetAllTransaction()
        {
            return _tableWork.TransactionTypeRepository.Get(x => x.IsDeleted == false).ToList();
        }
    }
}
