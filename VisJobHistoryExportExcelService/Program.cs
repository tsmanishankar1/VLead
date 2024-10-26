using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Serilog;
using VisJobHistoryExportExcelService;

class Program
{
    private static IServiceProvider _serviceProvider;
    static async Task Main(string[] args)
    {


        var services = new ServiceCollection();

        _serviceProvider = ServiceConfiguration.ConfigureServices().BuildServiceProvider();

        ConfigurationHelper.Initialize("appsettings.json");


        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(ConfigurationHelper.GetValue("Excel:logFilePath") + "VIS_logProcessV3.txt")
            .CreateLogger();

        var scheduler = await GetScheduler();

        var job = JobBuilder.Create<JobHistory>()
            .WithIdentity("JobHistory", "JobHistoryGroup")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("JobHistoryTrigger", "JobHistoryGroup")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(60 * Convert.ToInt32(ConfigurationHelper.GetValue("Excel:jobHistoryExportToExcelInterval")))
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        await scheduler.Start();
        var manualResetEvent = new ManualResetEvent(false);
        manualResetEvent.WaitOne();

        // await scheduler.Shutdown();
    }

    static async Task<IScheduler> GetScheduler()
    {
        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = await schedulerFactory.GetScheduler();
        return scheduler;
    }
    public static T GetService<T>()
    {
        return _serviceProvider.GetService<T>();
    }
}



public static class ConfigurationHelper
{
    private static IConfiguration _configuration;

    public static void Initialize(string appSettingsPath)
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true)
            .Build();
    }

    public static string GetValue(string key)
    {
        return _configuration[key];
    }

}
