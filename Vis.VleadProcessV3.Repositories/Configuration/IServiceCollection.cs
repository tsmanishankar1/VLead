using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models.Configuration;

namespace Vis.VleadProcessV3.Repositories.Configuration
{
    public static class IServiceCollectionExtension1
    {
        public static void AddRepositoryServices(this IServiceCollection services,IConfiguration configuration)
        {
           services.AddSingleton<IRepository<TableWork>, Repository<TableWork>>();
            services.AddSingleton<IRepository<ProcedureWork>, Repository<ProcedureWork>>();
            services.AddSingleton<IRepository<ViewWork>, Repository<ViewWork>>();
            services.AddSingleton<IQueryBuilder<UnitWork>, QueryBuilder<UnitWork>>();
            services.AddSingleton<IQueryBuilder<UnitViewWork>, QueryBuilder<UnitViewWork>>();
            services.AddSingleton<UnitProcedure>();
           // return services;
        }
    }
}
