using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ITAssetController : ControllerBase
    {
        public ITAssetController(ITAssetService iTAssetService)
        {
            _itassetservice = iTAssetService;
        }
        private readonly ITAssetService _itassetservice;

        //------------------------------------Existing code-slowness ------------------------------
        //[HttpGet]
        //[Route("nGetHardwareSoftware")]
        //public IActionResult GetHardwareSoftware()
        //{
        //    var HSDetails = _itassetservice.GetHardwareSoftware();
        //    var Details = new
        //    {
        //        HSDetailList = HSDetails,
        //    };
        //    return Ok(Details);
        //}
        //------------------------------------Existing code-slowness ------------------------------

        //-----------------------------------------------------------------------------------slowness bugs fixed--------------------------------------------------------
        [HttpGet]
        [Route("nGetHardwareSoftware")]
        public Object GetHardwareSoftware()
        {
            return _itassetservice.GetHardwareSoftware();
        }
        //-----------------------------------------------------------------------------------slowness bugs fixed--------------------------------------------------------

        [HttpPost]
        [Route("nSetITHData")]
        public IActionResult SetITHData(tbl_ITAssetPara Para)
        {
            var ITHDetails = _itassetservice.SetITHData(Para);
            var Details = new
            {
                ITHDetailList = ITHDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nUpdateITHData")]
        public IActionResult UpdateITHData(tbl_ITAssetPara Para)
        {
            var ITUpHDetails = _itassetservice.UpdateITHData(Para);
            var Details = new
            {
                ITUpHDetailList = ITUpHDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("nGetTableITAsset")]
        public IActionResult GetTableITAsset()
        {
            var TITDetails = _itassetservice.GetTableITAsset();
            var Details = new
            {
                TITDetailList = TITDetails,
            };
            return Ok(Details);
        }

        [HttpPost]
        [Route("nSetITSData")]
        public IActionResult SetITSData(tbl_ITAssetPara Para)
        {
            var ITSDetails = _itassetservice.SetITSData(Para);
            var Details = new
            {
                ITSDetailList = ITSDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nUpdateITSData")]
        public IActionResult UpdateITSData(tbl_ITAssetPara Para)
        {
            var ITUpSDetails = _itassetservice.UpdateITSData(Para);
            var Details = new
            {
                ITUpSDetailList = ITUpSDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nGetTableITSEAsset")]
        public IActionResult GetTableITSEAsset(Vis.VleadProcessV3.Services.ITAssetService.SinglePara Para)
        {
            var TITSDetails = _itassetservice.GetTableITSEAsset(Para);
            var Details = new
            {
                TITSDetailList = TITSDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("nGetTableITSAsset")]
        public IActionResult GetTableITSAsset()
        {
            var TITSDetails = _itassetservice.GetTableITSAsset();
            var Details = new
            {
                TITSDetailList = TITSDetails,
            };
            return Ok(Details);
        }

        [HttpPost]
        [Route("nDeleteITSAsset")]
        public IActionResult DeleteITSAsset(Vis.VleadProcessV3.Services.ITAssetService.SinglePara Para)
        {
            var ITSDDetails = _itassetservice.DeleteITSAsset(Para);
            var Details = new
            {
                ITSDDetailList = ITSDDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nDeleteITHAsset")]
        public IActionResult DeleteITHAsset(Vis.VleadProcessV3.Services.ITAssetService.SinglePara Para)
        {
            var ITHDDetails = _itassetservice.DeleteITHAsset(Para);
            var Details = new
            {
                ITHDDetailList = ITHDDetails,
            };
            return Ok(Details);
        }

        [HttpPost]
        [Route("nGetEditedITAsset")]
        public IActionResult GetEditedITAsset(Vis.VleadProcessV3.Services.ITAssetService.SinglePara Para)
        {
            var ITHEDetails = _itassetservice.GetEditedITAsset(Para);
            var Details = new
            {
                ITHEDetailList = ITHEDetails,
            };
            return Ok(Details);
        }


        [HttpGet]
        [Route("nGetVendorData")]
        public IActionResult GetVendorData()
        {
            var VendorGDetails = _itassetservice.GetVendorData();
            var Details = new
            {
                VendorGDetailList = VendorGDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nSetVendorDetails")]
        public IActionResult SetVendorDetails(tbl_VendorDetailsPara Para)
        {
            var SetVDetails = _itassetservice.SetVendorDetails(Para);
            var Details = new
            {
                SetVDetailList = SetVDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nUpdateVendorDetails")]
        public IActionResult UpdateVendorDetails(tbl_VendorDetailsPara Para)
        {
            var UpdateVDetails = _itassetservice.UpdateVendorDetails(Para);
            var Details = new
            {
                UpdateVDetailList = UpdateVDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("nGetVendorEditDetail")]
        public IActionResult GetVendorEditDetail(Vis.VleadProcessV3.Services.ITAssetService.SinglePara Para)
        {
            var GetVEDetails = _itassetservice.GetVendorEditDetail(Para);
            var Details = new
            {
                GetVEDetailList = GetVEDetails,
            };
            return Ok(Details);
        }

        [HttpPost]
        [Route("nSetBankDetails")]
        public IActionResult SetBankDetails(tbl_BankDetailsPara Para)
        {
            var SetBDetails = _itassetservice.SetBankDetails(Para);
            var Details = new
            {
                SetBDetailList = SetBDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("nGetBankDetails")]
        public IActionResult GetBankDetails()
        {
            var GetBDetails = _itassetservice.GetBankDetails();
            var Details = new
            {
                GetBDetailList = GetBDetails,
            };
            return Ok(Details);
        }
    }
}
