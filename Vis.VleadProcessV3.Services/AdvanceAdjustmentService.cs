using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class AdvanceAdjustmentService
    {
        private readonly UnitWork _unitWork;
        private readonly TableWork _tableWork;
        public AdvanceAdjustmentService(UnitWork unitWork,TableWork tableWork)
        {
            _unitWork = unitWork;
            _tableWork = tableWork;
        }
        public IEnumerable<ReceivableAdjustment> GetCustomerAdvance(int customerId)
        {
            //return _db.ReceivableAdjustments.Include("Receivable").Where(x => x.Receivable.CustomerId == customerId && x.IsInvoiceAdjustment == false && x.IsAvailableAdvance == true).ToList().OrderByDescending(x => x.Id);
            return _tableWork.ReceivableAdjustmentRepository.GetAllVal(x => x.Receivable).Where(x => x.Receivable.CustomerId == customerId && x.IsInvoiceAdjustment == false && x.IsAvailableAdvance == true).ToList().OrderByDescending(x => x.Id);

        }

        public bool CreateAdvanceAdjustment(AdvanceAdjustmentModel1 advanceAdjustment) 
        {
            var existingAdvance = _tableWork.ReceivableAdjustmentRepository.Get(x => x.Id == advanceAdjustment.AdvanceId && x.InvoiceNo == null).FirstOrDefault();

            bool status = false;

            foreach (var item in advanceAdjustment.ReceivableAdjustments)
            {
                var _receivableAdjustment = new ReceivableAdjustment();
                _receivableAdjustment.ReceivableId = existingAdvance.ReceivableId;
                _receivableAdjustment.InvoiceId = item.InvoiceId;
                _receivableAdjustment.InvoiceNo = item.InvoiceNo;
                _receivableAdjustment.AdjustmentAmount = item.AdjustmentAmount;
                _receivableAdjustment.ReferenceNo = item.InvoiceNo;
                _receivableAdjustment.IsInvoiceAdjustment = true;
                _receivableAdjustment.Crdr= "CR";

                _unitWork.ReceivableAdjustment.Add(_receivableAdjustment);
                var dbStatus = _tableWork.SaveChanges();


                var TotalAdjustmentAmount = _tableWork.ReceivableAdjustmentRepository.Get(x => x.InvoiceId == item.InvoiceId).Sum(x => x.AdjustmentAmount);
                var _invMaster = _tableWork.InvoiceMasterRepository.Get(x => x.Id == item.InvoiceId).First();
                if (_invMaster.TotalInvoiceValue == TotalAdjustmentAmount)
                {
                    _invMaster.IsOutstanding = false;
                    //_db.Entry(existingAdvance).State = EntityState.Modified;
                    _unitWork.InvoiceMaster.Update(_invMaster);
                    dbStatus = _unitWork.Save();
                }
            }
            //if (dbStatus == true)
            //{
            var TotalAdjustment = advanceAdjustment.ReceivableAdjustments.Sum(x => x.AdjustmentAmount);
            existingAdvance.AvailableAdvance = existingAdvance.AvailableAdvance - TotalAdjustment;
            //if (existingAdvance.AvailableAdvance - TotalAdjustment == 0)
            if (existingAdvance.AvailableAdvance == 0)
            {
                existingAdvance.IsAvailableAdvance = false;
            }
            //_db.Entry(existingAdvance).State = EntityState.Modified;
            _unitWork.ReceivableAdjustment.Update(existingAdvance);
            //var updatedStaus =
            var updatedStaus = _unitWork.Save();


            //}
            return updatedStaus > 0 ? true : false; //ms


        }
    }
}
