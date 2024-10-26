using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models.Configuration
{
    public static class IServiceCollectionExtension
    {
        //public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBConnection")));
        //    services.AddDbContext<TallyDbContext>();
        //    //services.AddRepositoryServices(configuration);
        //    //services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(configuration.GetConnectionString("DBConnection")));
        //    //services.AddDbContext<TallyDbContext>((options => options.UseSqlServer(configuration.GetConnectionString("DBConnection"))));
        //    //services.AddTransient<ApplicationDbContext>();
        //    //services.AddTransient<TallyDbContext>();
        //    // return services;
        //}
    }
}
