
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;



namespace VisProcess.Local.Controller
{
    [Route("api/ReportTest")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ReportController(IConfiguration configuration)
        {
                _configuration = configuration;
        }
        [HttpGet]
        public IActionResult InvoiceReport(string InvoiceNo)
        {
            //string strRedirectUrl = string.Empty;
            //try
            //{
            //    {
            //        string ReportUrl = _configuration.GetSection("Report").GetValue<string>("ReportServerURL");// ["ReportServerURL"];
            //        string ReportPath = _configuration.GetSection("Report").GetValue<string>("ReportServerFolder")+ "Invoice"; //FinalInvoice
            //        string strDomain = _configuration.GetSection("Report").GetValue<string>("ReportServerDomain");
            //        string strUserName = _configuration.GetSection("Report").GetValue<string>("ReportServerUserName");
            //        string strPassword = _configuration.GetSection("Report").GetValue<string>("ReportServerPassword");
                   Microsoft.Reporting.WebForms.ReportViewer rptViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            //        rptViewer.ProcessingMode = ProcessingMode.Remote;
            //        rptViewer.ServerReport.ReportServerCredentials =(IReportServerCredentials) new NetworkCredential(strUserName, strPassword, strDomain);
            //        rptViewer.SizeToReportContent = true;
            //        rptViewer.ZoomMode = ZoomMode.PageWidth;
            //        //rptViewer.Width =Microsoft.Web.UI.WebControls.;
            //        //rptViewer.Height = Unit.Pixel(1000);
            //        rptViewer.AsyncRendering = true;
            //        rptViewer.ServerReport.ReportServerUrl = new System.Uri(ReportUrl);
            //        rptViewer.ServerReport.ReportPath = ReportPath;
            //        ReportParameter[] rptParameter = new ReportParameter[1];

            //        rptParameter[0] = new ReportParameter("InvoiceNo", InvoiceNo, false);
            //        rptViewer.ServerReport.SetParameters(rptParameter);
            //        //ViewBag.ReportViewer = rptViewer;
            //        //return View();
            //       // TempData.Add("rptViewer", rptViewer);

            //        strRedirectUrl = _configuration.GetSection("Report").GetValue<string>("ReportsRedirectUrl")+ "FinalInvoice";
            //  return Ok(rptViewer);
            return Ok();
                    // return Redirect("http://localhost:63906/#/FinalInvoice");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //TempData.Add("rptViewerError", ex);
            //    throw ex;
            //}
            //return Redirect("http://61.12.33.236:81/#/JobOrderDetails2");
        }
    }
}
