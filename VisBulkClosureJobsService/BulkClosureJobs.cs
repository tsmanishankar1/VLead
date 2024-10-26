using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Services;

namespace VisBulkClosureJobsService
{
    public class BulkClosureJobs:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("enableGetBulkClosureAttachJobs")))
            {
                await BulkClosure();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task BulkClosure()
        {
            await _lock.WaitAsync();

            try
            {
                var _service = Program.GetService<WorkflowService>();
              var data= _service.BulkClosureNextTraymovement();
                Log.Information(data + " :" + DateTime.UtcNow);
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
