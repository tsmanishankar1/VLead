using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using VisProcess.Mapper;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Models.Configuration;
using Vis.VleadProcessV3.Repositories.Configuration;
using Microsoft.Office.Interop.Excel;
using System.Configuration;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using VisProcess.Local.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VisProcess;
using Microsoft.Extensions.DependencyInjection;
using Vis.VleadProcessV3.Services.WindowsService;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:4200", "*", "http://servicedesk.vleadservices.com:83", "https://vislocal.vleadservices.com");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// Json Cycle Dependency
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
}
    );

// Custom service

//builder.Services.AddRepositoryServices(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddDbContext<TallyDbContext>();

builder.Services.AddTransient<IRepository<TableWork>, Repository<TableWork>>();
//builder.Services.AddTransient<IRepository<ProcedureWork>, Repository<ProcedureWork>>();
builder.Services.AddTransient<ProcedureWork>();
builder.Services.AddTransient<IRepository<ViewWork>, Repository<ViewWork>>();
builder.Services.AddTransient<IQueryBuilder<UnitWork>, QueryBuilder<UnitWork>>();
builder.Services.AddTransient<IQueryBuilder<UnitViewWork>, QueryBuilder<UnitViewWork>>();
builder.Services.AddTransient<UnitProcedure>();
builder.Services.AddTransient<TableWork>();
builder.Services.AddTransient<UnitWork>();
builder.Services.AddTransient<ViewWork>();
builder.Services.AddTransient<UnitViewWork>();


builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<AdvanceAdjustmentService>();
builder.Services.AddTransient<AllocationService>();
builder.Services.AddTransient<ArtistInventoryReportService>();
builder.Services.AddTransient<AsyncWindowsService>();
builder.Services.AddTransient<BenchOptionService>();
builder.Services.AddTransient<BJWishService>();
builder.Services.AddTransient<BillingCycleMonthlyService>();
builder.Services.AddTransient<CapacityFormService>();
builder.Services.AddTransient<ClientCoOrdinationService>();
builder.Services.AddTransient<ClientCommunicationService>();
builder.Services.AddTransient<ClientOrderService>();
builder.Services.AddTransient<CustomerService>();
builder.Services.AddTransient<CustomerVsChecklistService>();
builder.Services.AddTransient<CustomerVsManagerService>();
builder.Services.AddTransient<CustomerMappingService>();
builder.Services.AddTransient<CustomerQueryService>();
builder.Services.AddTransient<CustomerVsDivisionService>();
builder.Services.AddTransient<CustomerVsEmployeeService>();
builder.Services.AddTransient<CustomerVsProcessService>();
builder.Services.AddTransient<DormantClientService>();
builder.Services.AddTransient<EmailAutomationService>();
builder.Services.AddTransient<EmployeeService>();
builder.Services.AddTransient<EmployeeFileUploadService>();
builder.Services.AddTransient<EmployeeVsDivisionService>();
builder.Services.AddTransient<EmployeeVsSkillsetService>();
builder.Services.AddTransient<ErrorCategoryService>();
builder.Services.AddTransient<ExternalErrorReportService>();
builder.Services.AddTransient<FileStatusDashBoardService>();
builder.Services.AddTransient<FileUpload>();
builder.Services.AddTransient<FtpFilesService>();
builder.Services.AddTransient<JobOrderService>();
builder.Services.AddTransient<HolidayService>();
builder.Services.AddTransient<HolidayMasterService>();
builder.Services.AddTransient<InventoryService>();
builder.Services.AddTransient<InvoiceService>();
builder.Services.AddTransient<ITAssetService>();
builder.Services.AddTransient<JobOrderService>();
builder.Services.AddTransient<JobTransferService>();
builder.Services.AddTransient<ListofWorkService>();
builder.Services.AddTransient<LeaveTrackService>();
builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<MailContactService>();
builder.Services.AddTransient<NewsService>();
builder.Services.AddTransient<OutStandingService>();
builder.Services.AddTransient<OutsourceCountService>();
builder.Services.AddTransient<PricingService>();
builder.Services.AddTransient<PricingTriggerService>();
builder.Services.AddTransient<ProcessService>();
builder.Services.AddTransient<ProjectedRevenueService>();
builder.Services.AddTransient<ReportService>();
builder.Services.AddTransient<ReceivableService>();
builder.Services.AddTransient<DashboardReportService>();
builder.Services.AddTransient<ScopeService>();
builder.Services.AddTransient<ShiftingService>();
builder.Services.AddTransient<SingleEntryService>();
builder.Services.AddTransient<TrainingService>();
builder.Services.AddTransient<TimeZoneService>();
builder.Services.AddTransient<TrainingFeedbackFormService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<VisClosureService>();
builder.Services.AddTransient<VoucherService>();
builder.Services.AddTransient<WorkflowService>();
builder.Services.AddTransient<WorkFromHomeNormService>();
//Service
builder.Services.AddTransient<VisProcess.Service.CustomerService>();
builder.Services.AddTransient<VisProcess.Service.DepartmentService>();
builder.Services.AddTransient<VisProcess.Service.ReportService>();
builder.Services.AddTransient<VisProcess.Service.ReportTrialCilentSummaryService>();
builder.Services.AddTransient<VisProcess.Service.ScopeService>();

//Local Service
builder.Services.AddTransient<DropdownService>();//Service



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAutoMapper(typeof(VisProcess.Mapper.AutoMapper));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
app.Run();


public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>();
        services.AddDbContext<TallyDbContext>();
        services.AddSingleton<IWebHostEnvironment>(new WebHostEnvironment());

        services.AddSingleton<IConfiguration>(ConfigureWebApiAppSettings());
        services.AddTransient<IRepository<TableWork>, Repository<TableWork>>();
        //builder.Services.AddTransient<IRepository<ProcedureWork>, Repository<ProcedureWork>>();
        services.AddTransient<ProcedureWork>();
        services.AddTransient<IRepository<ViewWork>, Repository<ViewWork>>();
        services.AddTransient<IQueryBuilder<UnitWork>, QueryBuilder<UnitWork>>();
        services.AddTransient<IQueryBuilder<UnitViewWork>, QueryBuilder<UnitViewWork>>();
        services.AddTransient<UnitProcedure>();
        services.AddTransient<TableWork>();
        services.AddTransient<UnitWork>();
        services.AddTransient<ViewWork>();
        services.AddTransient<UnitViewWork>();

        services.AddTransient<AsyncWindowsService>();
        services.AddTransient<AccountService>();
        services.AddTransient<AdvanceAdjustmentService>();
        services.AddTransient<AllocationService>();
        services.AddTransient<ArtistInventoryReportService>();
        services.AddTransient<BillingCycleMonthlyService>();
        services.AddTransient<BenchOptionService>();
        services.AddTransient<BJWishService>();
        services.AddTransient<CapacityFormService>();
        services.AddTransient<ClientCoOrdinationService>();
        services.AddTransient<ClientCommunicationService>();
        services.AddTransient<ClientOrderService>();
        services.AddTransient<CustomerService>();
        services.AddTransient<CustomerVsChecklistService>();
        services.AddTransient<CustomerMappingService>();
        services.AddTransient<CustomerQueryService>();
        services.AddTransient<CustomerVsDivisionService>();
        services.AddTransient<CustomerVsEmployeeService>();
        services.AddTransient<CustomerVsProcessService>();
        services.AddTransient<DormantClientService>();
        services.AddTransient<EmailAutomationService>();
        services.AddTransient<EmployeeService>();
        services.AddTransient<EmployeeVsDivisionService>();
        services.AddTransient<EmployeeVsSkillsetService>();
        services.AddTransient<ErrorCategoryService>();
        services.AddTransient<ExternalErrorReportService>();
        services.AddTransient<FileStatusDashBoardService>();
        services.AddTransient<FileUpload>();
        services.AddTransient<JobOrderService>();
        services.AddTransient<HolidayService>();
        services.AddTransient<InventoryService>();
        services.AddTransient<InvoiceService>();
        services.AddTransient<ITAssetService>();
        services.AddTransient<JobOrderService>();
        services.AddTransient<JobTransferService>();
        services.AddTransient<ListofWorkService>();
        services.AddTransient<LocationService>();
        services.AddTransient<MailContactService>();
        services.AddTransient<NewsService>();
        services.AddTransient<OutStandingService>();
        services.AddTransient<OutsourceCountService>();
        services.AddTransient<PricingService>();
        services.AddTransient<PricingTriggerService>();
        services.AddTransient<ProcessService>();
        services.AddTransient<ProjectedRevenueService>();
        services.AddTransient<ReportService>();
        services.AddTransient<ReceivableService>();
        services.AddTransient<DashboardReportService>();
        services.AddTransient<ScopeService>();
        services.AddTransient<ShiftingService>();
        services.AddTransient<SingleEntryService>();
        services.AddTransient<TrainingService>();
        services.AddTransient<TimeZoneService>();
        services.AddTransient<TrainingFeedbackFormService>();
        services.AddTransient<UserService>();
        services.AddTransient<VisClosureService>();
        services.AddTransient<VoucherService>();
        services.AddTransient<WorkflowService>();
        services.AddTransient<WorkFromHomeNormService>();
        //Service
        services.AddTransient<VisProcess.Service.CustomerService>();
        services.AddTransient<VisProcess.Service.DepartmentService>();
        services.AddTransient<VisProcess.Service.ReportService>();
        services.AddTransient<VisProcess.Service.ReportTrialCilentSummaryService>();
        services.AddTransient<VisProcess.Service.ScopeService>();

        //Local Service
        services.AddTransient<DropdownService>();
        //Service
        return services;
    }
    public static IConfigurationRoot ConfigureWebApiAppSettings()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        return configurationBuilder.Build();
    }
}