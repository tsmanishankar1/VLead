using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.Services.WindowsService;

namespace VisJobHistoryExportExcelService
{
    public class JobHistory:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("Excel:enableJobHistoryExportToExcel")))
            {
                await ExportExcel();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task ExportExcel()
        {
            await _lock.WaitAsync();
        
                try
                {
                var _service = Program.GetService<ClientCoOrdinationService>();
                _service.ExportExcel();
                DateTime dtn = DateTime.UtcNow;
                Log.Information("Excel Import fine :" + dtn);
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
