using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisCreditDaysCalcElapse
{
    public class Elapse:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("enableCreditDaysElapse")))
            {
                await CreditDaysElapse();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task CreditDaysElapse()
        {
            await _lock.WaitAsync();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Specify the API URL
                    var url = ConfigurationHelper.GetValue("Hosting");
                    HttpResponseMessage response = await httpClient.GetAsync(url + "api/JobOrder/nGetServiceforCreditDays");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("Service in CreditDaysElapse fine :" + dtn);
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
