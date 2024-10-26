using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisDeleteDirUS
{
    public class DeleteFTPDir:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("EnableService")))
            {
                await DeleteFTPDirUS();
            }
            if (Convert.ToBoolean(ConfigurationHelper.GetValue("EnableService")))
            {
                await DeleteFTPDirIND();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task DeleteFTPDirUS()
        {
            await _lock.WaitAsync();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(ConfigurationHelper.GetValue("checkDownloadUSFilespath") + "DeleteDirUSLogSProcessV3.txt")
                .CreateLogger();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Specify the API URL
                    var url = ConfigurationHelper.GetValue("Hosting");
                    HttpResponseMessage response = await httpClient.GetAsync(url + "api/FTPs/DeleteDirUS");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("DeleteFTPDirUS Working Fine :" + dtn);
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
        private async Task DeleteFTPDirIND()
        {
            await _lock.WaitAsync();
            Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .WriteTo.File(ConfigurationHelper.GetValue("checkDownloadUSFilespath") + "DeleteDirINDLogEProcessV3.txt")
               .CreateLogger();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Specify the API URL
                    var url = ConfigurationHelper.GetValue("Hosting");
                    HttpResponseMessage response = await httpClient.GetAsync(url + "api/FTP/DeleteDirIND");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("DeleteFTPDirIND Working Fine :" + dtn);
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
