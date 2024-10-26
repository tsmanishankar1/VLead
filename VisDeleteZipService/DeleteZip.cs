using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisDeleteZipService
{
    public class DeleteZip:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("enableDeleteZip")))
            {
                await DeleteZipService();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task DeleteZipService()
        {
            await _lock.WaitAsync();
           try
                {
                    DirectoryInfo info = new DirectoryInfo(ConfigurationHelper.GetValue("zipFolderPath"));
                    var delayTime = Convert.ToInt32(ConfigurationHelper.GetValue("deleteZipInterval"));
                    FileInfo[] files = info.GetFiles().Where(x => x.CreationTime < DateTime.Now.Subtract(new TimeSpan(0, delayTime, 0))).OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo file in files)
                    {
                        if ((System.IO.File.Exists(file.FullName)))
                        {
                            System.IO.File.Delete(file.FullName);
                        }
                    }
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("DeleteZip Working Fine :" + dtn);
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
