using Microsoft.EntityFrameworkCore;
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
    public class ITAssetService
    {
        public ITAssetService(TableWork tableWork,ApplicationDbContext dbContext)
        {
            _tablework = tableWork;
            _db = dbContext;
        }
        private readonly TableWork _tablework;
        private  ApplicationDbContext _db;

        //---------------------------------------Existing code-slowness-------------------------
        //public Object GetHardwareSoftware()
        //{
        //    var h = _db.tbl_ITAssetHardware.Where(x=>x.IsDeleted == false).ToList();
        //    var s = _db.tbl_ITAssetSoftware.Where(x => x.IsDeleted == false).ToList();
        //    var result = new
        //    {
        //        HardwareDate = h,
        //        SoftwareData = s
        //    };
        //    return result;
        //}
        //---------------------------------------Existing code-slowness-------------------------

        //------------------------------------------------------------------------------------slowness bugs fixed-------------------------------------------------------------------------
        public Object GetHardwareSoftware()
        {
            //var h = _db.tbl_ITAssetHardware.Where(x => x.IsDeleted == false).ToList();
            //var s = _db.tbl_ITAssetSoftware.Where(x => x.IsDeleted == false).ToList();
            var h = _tablework.tbl_ITAssetHardwareRepository.Get(x => x.IsDeleted == false).ToList();
            var s = _tablework.tbl_ITAssetSoftwareRepository.Get(x => x.IsDeleted == false).ToList();

            var result = new
            {
                HardwareData = h,
                SoftwareData = s
            };
            return result;
        }
        //------------------------------------------------------------------------------------slowness bugs fixed-------------------------------------------------------------------------

        public Object SetITHData(tbl_ITAssetPara Para)
        {
            try
            {
                TblItasset ITAssetH = new TblItasset();
                ITAssetH.BayNumber = Para.BayNumber;
                ITAssetH.Location = Para.Location;
                ITAssetH.PcName = Para.PcName;
                ITAssetH.HardwareId = Para.HardwareId;
                ITAssetH.Monitor = Para.Monitor;
                ITAssetH.MonitorSerialNumber = Para.MonitorSerialNumber;
                ITAssetH.Keyboard = Para.Keyboard;
                ITAssetH.KeyboardSerialNumber = Para.KeyboardSerialNumber;
                ITAssetH.Roll = Para.Roll;
                ITAssetH.Division = Para.Division;
                ITAssetH.Brand = Para.Brand;
                ITAssetH.Model = Para.Model;
                ITAssetH.WarantyDetails = Para.WarantyDetails;
                ITAssetH.Ram = Para.Ram;
                ITAssetH.Processor = Para.Processor;
                ITAssetH.Graphics = Para.Graphics;
                ITAssetH.Hdd = Para.Hdd;
                ITAssetH.TagNumber = Para.TagNumber;
                ITAssetH.MacAddress = Para.MacAddress;
                ITAssetH.Os = Para.Os;
                ITAssetH.IpAddress = Para.IpAddress;
                ITAssetH.ServerTypeId = Para.ServerTypeId;
                if (Para.ServerTypeId == 1)
                {
                    ITAssetH.ServerType = "Server";
                }
                else if (Para.ServerTypeId == 2)
                {
                    ITAssetH.ServerType = "Virtual Server";
                }
                ITAssetH.InvoiceDate = Para.InvoiceDate;
                ITAssetH.InvoiceNumber = Para.InvoiceNumber;
                ITAssetH.Mouse = Para.Mouse;
                ITAssetH.MouseSerialNumber = Para.MouseSerialNumber;
                ITAssetH.WorkingStatusId = Para.WorkingStatusId;
                if (Para.WorkingStatusId == 1)
                {
                    ITAssetH.WorkingStatus = "Active";
                }
                else if (Para.WorkingStatusId == 2)
                {
                    ITAssetH.WorkingStatus = "Repair";
                }
                else if (Para.WorkingStatusId == 3)
                {
                    ITAssetH.WorkingStatus = "Stand By";
                }
                ITAssetH.IsDeleted = false;
                ITAssetH.CreatedBy = Para.EmployeeId;
                ITAssetH.CreatedDate = DateTime.UtcNow;
                _db.TblItassets.Add(ITAssetH);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var data = _db.TblItassets.Local.FirstOrDefault();
            var result = new
            {
                Id = data.Id,
                BayNumber = data.BayNumber,
            };
            return result;
        }
        public bool UpdateITHData(tbl_ITAssetPara Para)
        {
            bool result = false;
            try
            {
                var data = _db.TblItassets.FirstOrDefault(x => x.Id == Para.ITAssetId);
                data.Location = Para.Location;
                data.PcName = Para.PcName;
                data.HardwareId = Para.HardwareId;
                data.Monitor = Para.Monitor;
                data.MonitorSerialNumber = Para.MonitorSerialNumber;
                data.Keyboard = Para.Keyboard;
                data.KeyboardSerialNumber = Para.KeyboardSerialNumber;
                data.Roll = Para.Roll;
                data.Division = Para.Division;
                data.Brand = Para.Brand;
                data.Model = Para.Model;
                data.WarantyDetails = Para.WarantyDetails;
                data.Ram = Para.Ram;
                data.Processor = Para.Processor;
                data.Graphics = Para.Graphics;
                data.Hdd = Para.Hdd;
                data.TagNumber = Para.TagNumber;
                data.MacAddress = Para.MacAddress;
                data.Os = Para.Os;
                data.IpAddress = Para.IpAddress;
                data.ServerTypeId = Para.ServerTypeId;
                if (Para.ServerTypeId == 1)
                {
                    data.ServerType = "Server";
                }
                else if (Para.ServerTypeId == 2)
                {
                    data.ServerType = "Virtual Server";
                }
                data.InvoiceDate = Para.InvoiceDate;
                data.InvoiceNumber = Para.InvoiceNumber;
                data.Mouse = Para.Mouse;
                data.MouseSerialNumber = Para.MouseSerialNumber;
                data.WorkingStatusId = Para.WorkingStatusId;
                if (Para.WorkingStatusId == 1)
                {
                    data.WorkingStatus = "Active";
                }
                else if (Para.WorkingStatusId == 2)
                {
                    data.WorkingStatus = "Repair";
                }
                else if (Para.WorkingStatusId == 3)
                {
                    data.WorkingStatus = "Stand By";
                }
                data.UpdatedBy = Para.EmployeeId;
                data.UpdatedDate = DateTime.UtcNow;
                _db.Entry(data).State = EntityState.Modified;
                _db.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public String SetITSData(tbl_ITAssetPara Para)
        {
            string result = "Record Added Successfully!";
            try
            {
                if (Para.ITAssetId != 0)
                {
                    TblItassetSoftwareCompliance ITAssetS = new TblItassetSoftwareCompliance();
                    ITAssetS.ItassetId = Para.ITAssetId;
                    if (Para.SoftwareStatusId == 1)
                    {
                        ITAssetS.SoftwareStatus = "Compliance";
                    }
                    else if (Para.SoftwareStatusId == 2)
                    {
                        ITAssetS.SoftwareStatus = "Non Compliance";
                    }
                    ITAssetS.SoftwareStatusId = Para.SoftwareStatusId;
                    ITAssetS.IsDeleted = false;
                    ITAssetS.CreatedBy = Para.EmployeeId;
                    ITAssetS.CreatedDate = DateTime.UtcNow;
                    foreach (var item in Para.SoftwareId)
                    {
                        ITAssetS.SoftwareId = Convert.ToInt16(item);
                        _db.TblItassetSoftwareCompliances.Add(ITAssetS);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    result = "Invalid Data";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        //-----------------------------------------------------------------------------update bugs fixed-----------------------------------------------------------------
        public String UpdateITSData(tbl_ITAssetPara Para)
        {
            string result = "Record Updated Successfully!";
            try
            {
                if (Para.ITAssetId != 0)
                {
                    foreach (var item in Para.SoftwareId)
                    {
                        var checkdata1 = _db.TblItassetSoftwareCompliances.FirstOrDefault(x => x.ItassetId == Para.ITAssetId &&
                            x.SoftwareId == item && x.IsDeleted == false);
                        //var checkdata1 = _db.tbl_ITAssetSoftwareCompliance.FirstOrDefault(x => x.ITAssetId == Para.ITAssetId &&
                        //    x.SoftwareId == item && x.SoftwareStatusId == 1 && x.IsDeleted == false);
                        //var checkdata2 = _db.tbl_ITAssetSoftwareCompliance.FirstOrDefault(x => x.ITAssetId == Para.ITAssetId &&
                        // x.SoftwareId == item && x.SoftwareStatusId == 2 && x.IsDeleted == false);
                        if (checkdata1 == null)
                        {
                            TblItassetSoftwareCompliance data = new TblItassetSoftwareCompliance();
                            data.ItassetId = Para.ITAssetId;
                            data.SoftwareId = Convert.ToInt16(item);
                            if (Para.SoftwareStatusId == 1)
                            {
                                data.SoftwareStatus = "Compliance";
                            }
                            else if (Para.SoftwareStatusId == 2)
                            {
                                data.SoftwareStatus = "Non Compliance";
                            }
                            data.SoftwareStatusId = Para.SoftwareStatusId;
                            data.CreatedBy = Para.EmployeeId;
                            data.CreatedDate = DateTime.UtcNow;
                            _db.TblItassetSoftwareCompliances.Add(data);
                            _db.SaveChanges();
                        }
                        else
                        {
                            result = "Existing Entry!!! Please Make A New Entry...";
                        }
                    }
                }
                else
                {
                    result = "Invalid Data";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        //-----------------------------------------------------------------------------update bugs fixed-----------------------------------------------------------------

        //-----------------------------------------------------Existing code-update bug fixed----------------------------------
        //public String UpdateITSData(tbl_ITAssetPara Para)
        //{
        //    string result = "Record Updated Successfully!";
        //    try
        //    {
        //        if (Para.ITAssetId != 0)
        //        {
        //            foreach (var item in Para.SoftwareId)
        //            {
        //                var checkdata1 = _db.tbl_ITAssetSoftwareCompliance.FirstOrDefault(x=>x.ITAssetId == Para.ITAssetId && 
        //                    x.SoftwareId == item && x.SoftwareStatusId == 1 && x.IsDeleted == false);
        //                var checkdata2 = _db.tbl_ITAssetSoftwareCompliance.FirstOrDefault(x => x.ITAssetId == Para.ITAssetId &&
        //                    x.SoftwareId == item && x.SoftwareStatusId == 2 && x.IsDeleted == false);
        //                if (checkdata1 != null || checkdata2 != null)
        //                {
        //                    int PId = 0;
        //                    if (checkdata1 != null)
        //                    {
        //                        PId = checkdata1.Id;
        //                        var data = _db.tbl_ITAssetSoftwareCompliance.FirstOrDefault(x => x.Id == PId);
        //                        data.ITAssetId = Para.ITAssetId;
        //                        data.SoftwareId = Convert.ToInt16(item);
        //                        if (Para.SoftwareStatusId == 1)
        //                        {
        //                            data.SoftwareStatus = "Compliance";
        //                        }
        //                        else if (Para.SoftwareStatusId == 2)
        //                        {
        //                            data.SoftwareStatus = "Non Compliance";
        //                        }
        //                        data.SoftwareStatusId = Para.SoftwareStatusId;
        //                        data.UpdatedBy = Para.EmployeeId;
        //                        data.UpdatedDate = DateTime.UtcNow;
        //                        _db.Entry(data).State = EntityState.Modified;
        //                    }
        //                    else if (checkdata2 != null)
        //                    {
        //                        PId = checkdata2.Id;
        //                        var data = _db.tbl_ITAssetSoftwareCompliance.FirstOrDefault(x => x.Id == PId);
        //                        data.ITAssetId = Para.ITAssetId;
        //                        data.SoftwareId = Convert.ToInt16(item);
        //                        if (Para.SoftwareStatusId == 1)
        //                        {
        //                            data.SoftwareStatus = "Compliance";
        //                        }
        //                        else if (Para.SoftwareStatusId == 2)
        //                        {
        //                            data.SoftwareStatus = "Non Compliance";
        //                        }
        //                        data.SoftwareStatusId = Para.SoftwareStatusId;
        //                        data.UpdatedBy = Para.EmployeeId;
        //                        data.UpdatedDate = DateTime.UtcNow;
        //                        _db.Entry(data).State = EntityState.Modified;
        //                    }                            
        //                    _db.SaveChanges();
        //                }                        
        //            }
        //        }
        //        else
        //        {
        //            result = "Invalid Data";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return result;
        //}
        //-----------------------------------------------------Existing code-update bug fixed----------------------------------
        public Object GetTableITAsset()
        {
            //var result = _db.tbl_ITAsset.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            var result = (from itasset in _db.TblItassets
                          join ith in _db.TblItassetHardwares
                              on itasset.HardwareId equals ith.Id
                          where (itasset.IsDeleted == false)
                          select new { itasset.Id, itasset.BayNumber, itasset.Location, itasset.PcName, ith.Description, itasset.Roll, itasset.WorkingStatus }).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        public Object GetTableITSEAsset(SinglePara Para)
        {
            var result = (from itsasset in _db.TblItassetSoftwareCompliances
                          join its in _db.TblItassetSoftwares
                              on itsasset.SoftwareId equals its.Id
                          join ita in _db.TblItassets
                          on itsasset.ItassetId equals ita.Id
                          where (itsasset.IsDeleted == false && itsasset.ItassetId == (Para.Id == 0 ? itsasset.Id : Para.Id))
                          select new { itsasset.Id, ita.BayNumber, its.Description, itsasset.SoftwareStatus }).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        public Object GetTableITSAsset()
        {
            var result = (from itsasset in _db.TblItassetSoftwareCompliances
                          join its in _db.TblItassetSoftwares
                              on itsasset.SoftwareId equals its.Id
                          join ita in _db.TblItassets
                          on itsasset.ItassetId equals ita.Id
                          where (itsasset.IsDeleted == false)
                          select new { itsasset.Id, ita.BayNumber, its.Description, itsasset.SoftwareStatus }).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        public class SinglePara
        {
            public int Id { get; set; }
        }
        public bool DeleteITSAsset(SinglePara Para)
        {
            bool end = false;
            try
            {
                var result = _db.TblItassetSoftwareCompliances.FirstOrDefault(x => x.Id == Para.Id);
                result.IsDeleted = true;
                _db.Entry(result).State = EntityState.Modified;
                _db.SaveChanges();
                end = true;
            }
            catch
            {

            }
            return end;
        }
        public bool DeleteITHAsset(SinglePara Para)
        {
            bool end = false;
            try
            {
                var result = _db.TblItassets.FirstOrDefault(x => x.Id == Para.Id);
                result.IsDeleted = true;
                _db.Entry(result).State = EntityState.Modified;
                var result1 = _db.TblItassetSoftwareCompliances.FirstOrDefault(x => x.ItassetId == Para.Id);
                if (result1 != null)
                {
                    result1.IsDeleted = true;
                    _db.Entry(result1).State = EntityState.Modified;
                }
                _db.SaveChanges();
                end = true;
            }
            catch
            {

            }
            return end;
        }
        public Object GetEditedITAsset(SinglePara Para)
        {
            var data = _db.TblItassets.FirstOrDefault(x => x.Id == Para.Id);
            var result = new
            {
                Id = data.Id,
                BayNumber = data.BayNumber,
                Location = data.Location,
                PcName = data.PcName,
                PcTypeId = data.HardwareId,
                PcType = _db.TblItassetHardwares.FirstOrDefault(x => x.Id == data.HardwareId).Description,
                Monitor = data.Monitor,
                MonitorSerialNumber = data.MonitorSerialNumber,
                Keyboard = data.Keyboard,
                KeyboardSerialNumber = data.KeyboardSerialNumber,
                Roll = data.Roll,
                Division = data.Division,
                Brand = data.Brand,
                Model = data.Model,
                WarantyDetails = data.WarantyDetails,
                Ram = data.Ram,
                Processor = data.Processor,
                Graphics = data.Graphics,
                Hdd = data.Hdd,
                TagNumber = data.TagNumber,
                MacAddress = data.MacAddress,
                Os = data.Os,
                IpAddress = data.IpAddress,
                ServerTypeId = data.ServerTypeId,
                ServerType = data.ServerType,
                InvoiceDate = data.InvoiceDate,
                InvoiceNumber = data.InvoiceNumber,
                Mouse = data.Mouse,
                MouseSerialNumber = data.MouseSerialNumber,
                WorkingStatusId = data.WorkingStatusId,
                WorkingStatus = data.WorkingStatus,
            };
            return result;
        }

        public IEnumerable<TblVendorDetail> GetVendorData()
        {
            var result = _db.TblVendorDetails.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        public String SetVendorDetails(tbl_VendorDetailsPara Para)
        {
            string message = "Record Added Successfully!";
            try
            {
                TblVendorDetail vd = new TblVendorDetail();
                vd.VendorName = Para.VendorName;
                vd.InvoiceNumber = Para.InvoiceNumber;
                vd.InvoiceDate = Para.InvoiceDate;
                vd.InvoiceValue = Para.InvoiceValue;
                vd.PendingAmount = Para.PendingAmount;
                vd.AmountPaid = Para.AmountbePaid;
                if (Para.PendingAmount == 0)
                {
                    vd.IsPaid = true;
                }
                else
                {
                    vd.IsPaid = false;
                }
                vd.VendorId = 0;
                vd.IsDeleted = false;
                vd.CreatedBy = Para.EmployeeId;
                vd.CreatedDate = DateTime.UtcNow;
                _db.TblVendorDetails.Add(vd);
                _db.SaveChanges();
            }
            catch
            {

            }
            return message;
        }
        public Object GetVendorEditDetail(SinglePara Para)
        {
            var data = _db.TblVendorDetails.FirstOrDefault(x => x.IsDeleted == false && x.Id == Para.Id);
            var result = new
            {
                Id = data.Id,
                VendorName = data.VendorName,
                InvoiceNumber = data.InvoiceNumber,
                InvoiceDate = data.InvoiceDate,
                InvoiceValue = Math.Round(data.InvoiceValue, 2),
                PendingAmount = Math.Round(data.PendingAmount, 2),
                AmountPaid = Math.Round(data.AmountPaid, 2),
            };
            return result;
        }
        public String UpdateVendorDetails(tbl_VendorDetailsPara Para)
        {
            string message = "Record Updated Successfully!";
            try
            {
                var data = _db.TblVendorDetails.FirstOrDefault(x => x.Id == Para.Id);
                data.VendorName = Para.VendorName;
                data.InvoiceNumber = Para.InvoiceNumber;
                data.InvoiceDate = Para.InvoiceDate;
                data.InvoiceValue = Para.InvoiceValue;
                data.PendingAmount = Para.PendingAmount;
                data.AmountPaid = Para.AmountPaid;
                if (Para.PendingAmount == 0)
                {
                    data.IsPaid = true;
                }
                else
                {
                    data.IsPaid = false;
                }
                data.UpdatedBy = Para.EmployeeId;
                data.UpdatedDate = DateTime.UtcNow;
                _db.Entry(data).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch
            {

            }
            return message;
        }
        public String SetBankDetails(tbl_BankDetailsPara Para)
        {
            string message = "Record Added Successfully!";
            try
            {
                TblBankDetail BD = new TblBankDetail();
                BD.BankName = Para.BankName;
                BD.ClosingDate = Para.ClosingDate;
                BD.ClosingBalance = Para.ClosingBalance;
                BD.IsDeleted = false;
                BD.CreatedBy = Para.EmployeeId;
                BD.CreatedDate = DateTime.UtcNow;
                _db.TblBankDetails.Add(BD);
                _db.SaveChanges();
            }
            catch
            {

            }
            return message;
        }
        public IEnumerable<TblBankDetail> GetBankDetails()
        {
            var result = _db.TblBankDetails.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            return result;
        }
    }
}
