using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;

namespace VisScheduled
{
    public class VisScheduledMail:IJob
    {
        private readonly SemaphoreSlim _lock = new(1);
        async Task IJob.Execute(IJobExecutionContext context)
        {

            if (Convert.ToBoolean(ConfigurationHelper.GetValue("EnableScheduleService")))
            {
                await Schedule();
            }
            await Task.CompletedTask;
            return;
        }

        private async Task Schedule()
        {
            await _lock.WaitAsync();
            var _allocationService = Program.GetService<AllocationService>();
            var _unitofwork = Program.GetService<UnitWork>();
            try
            {
                
                List<JobOrder> completedJobs = new List<JobOrder>();
                completedJobs = _allocationService.ScheduledMailSend(0).ToList();
                Serilog.Log.Information("Number Of Job Orders :" + completedJobs.Count());
                foreach (var item in completedJobs)
                {
                   
                    Serilog.Log.Information("Jid : " + item.Id);
                    var check = _unitofwork.JobOrder.Where(x => x.Id == item.Id && x.IsDeleted == false).FirstOrDefault();
                    if (check != null)
                    {
                        check.IsAutoUploadCount = 6;
                        _unitofwork.JobOrder.Update(check);
                        _unitofwork.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                Vis.VleadProcessV3.Models.Log savelog = new Vis.VleadProcessV3.Models.Log();
                savelog.Module = "Scheduled Mail Send Service";
                savelog.Description = "Email is Not send because of " + ex.InnerException + ": Message =" + ex.Message + "Data :" + ex.Data + "Source : " + ex.Source;
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                _unitofwork.Log.Add(savelog);
                _unitofwork.SaveChanges();
                Serilog.Log.Information("Date :" + DateTime.UtcNow);
                Serilog.Log.Information(ex.ToString());

            }
            finally
            {
                _lock.Release();
            }



        }
    }
}
