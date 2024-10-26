using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VisUSFilesDownloadService
{
    public class DownloadUSFiles:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("enableDownloadUSFiles")))
            {
                await AutoDownloadUSFiles();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task AutoDownloadUSFiles()
        {
            await _lock.WaitAsync();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Specify the API URL
                    var url = ConfigurationHelper.GetValue("Hosting");
                    HttpResponseMessage response = await httpClient.GetAsync(url + "api/Transfer/File");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("Service in DownloadUSFiles fine :" + dtn);
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
