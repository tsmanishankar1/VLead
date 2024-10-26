using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.Services.WindowsService;
using Vis.VleadProcessV3.ViewModels;

namespace VisAutoUploadNonVisService
{
    public class AutoUploadNonVis:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("VisNonUpload:enableGetCompletedJobs")))
            {
                await AutpUploadNon();
            }             
            await Task.CompletedTask;
            return;
        }
        private async Task AutpUploadNon()
        {
            await _lock.WaitAsync();
                int holdjid = 0;
                try
                {
                    var empid = ConfigurationHelper.GetValue("VisNonUpload:employeeid");
                    var _windowsService = Program.GetService<AsyncWindowsService>();

                var completedJobs = await _windowsService.GetCompletedJobsforService(0);
                    var clientJobs = completedJobs.Where(x => x.ClientOrderId != null && (x.IsAutoUploadCount == null || x.IsAutoUploadCount <= 2) && (x.IsManualUpload == false || x.IsManualUpload == null)).OrderBy(x => x.TranId).ToList();
                    var pm = new ProcessMovement();
                    var processMovement = new List<ProcessMovement>();
                    foreach (var item in clientJobs)
                    {
                        holdjid = item.Jid;
                        processMovement.Add(new ProcessMovement
                        {
                            TranMasterId = item.TranMasterId,
                            DepartmentId = item.DepartmentId,
                            ClientId = item.ClientId,
                            TranId = item.TranId,
                            TimeStamp = item.TimeStamp,
                            JId = item.Jid,
                            JobId = item.JobId,
                            CommentsToClient = item.CommentsToClient,
                            IsJobFilesNotTransfer = item.IsJobFilesNotTransfer//
                        });
                    }
                    pm.SelectedRows = processMovement;
                    pm.EmployeeId = Convert.ToInt16(empid);//staging-2218//live-274//
                    pm.StatusId = 12;
                    pm.ProcessId = 1;
                    pm.CopyFiles = true;
                    pm.AutoUploadJobs = true;

                var _allocationService = Program.GetService<AllocationService>();
                var allocRepActionCall = _allocationService.ProcessMovement(pm);
                    int totalcount = clientJobs.Count;
                    DateTime dtn = DateTime.UtcNow;

                    Log.Information("Date :" + dtn);
                    Log.Information("TotalCount : " + totalcount);

                }
                catch (Exception ex)
                {
                    DateTime dtn = DateTime.UtcNow;
                    Log.Information("Date :" + dtn);
                    Log.Information("Catched JId : " + holdjid);
                  
                }
            finally
            {
                _lock.Release();
            }
            
        }
    }
}
