using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisTrailJobsMailService
{
    public class TrailMail:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("enableTrialMailCalculation")))
            {
                await VisTrailJobsMail();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task VisTrailJobsMail()
        {
            await _lock.WaitAsync();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Specify the API URL
                    var url = ConfigurationHelper.GetValue("Hosting");
                    HttpResponseMessage response = await httpClient.GetAsync(url + "api/TrailMail/SendMail");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("Service in TrialMail fine :" + dtn);
                }
                catch (Exception ex)
                {
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("Date :" + dtn);
                    Log.Information(ex.ToString());

                }
                finally
                {
                    _lock.Release();
                }
            }


        }

    }
}
