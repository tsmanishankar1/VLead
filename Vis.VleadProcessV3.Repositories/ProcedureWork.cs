using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
//using Microsoft.EntityFrameworkCore;



namespace Vis.VleadProcessV3.Repositories
{
    public class ProcedureWork
    {

        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        public ProcedureWork(ApplicationDbContext context, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public virtual IEnumerable<TEntity> ExecStoredProcedure<TEntity>(string query, SqlParameter[] parameters) where TEntity : class
        {
            try
            {

                DbSet<TEntity> dbSet = context.Set<TEntity>();


                var result = dbSet.FromSqlRaw(query, parameters).ToList();

                return result;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual IEnumerable<TEntity> ExecProcedureOrView<TEntity>(string query) where TEntity : class
        {
            try
            {
                using (var context = new ApplicationDbContext(configuration))
                {
                    DbSet<TEntity> dbSet = context.Set<TEntity>();
                    var result = dbSet.FromSqlRaw(query).ToList();

                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public virtual int ExecSQL(string command, SqlParameter[] parameters)
        {
            try
            { 
                return context.Database.ExecuteSqlRaw(command, parameters);

            }
            catch
            {
                return 0;
            }
        }
    }
}
