using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class SingleEntryService
    {
      
      
        private readonly ApplicationDbContext context;
        public SingleEntryService(ApplicationDbContext dbContext)
        {
            
            context = dbContext;
        }
        public IEnumerable<Object> GetTableValuesByTableName(string TableName)
        {
          
                var query = context.Set<SingleEntry1>().FromSqlRaw($"select Id, Description from {TableName} where IsDeleted = 0").OrderBy(x => x.Description).ToList();
                return query;
           
        }
        public bool AddSingleEntry(SingleEntry singleEntry)
        {
            try
            {
               
                    var dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:sss");
                    context.Database.ExecuteSqlRaw("insert into " + singleEntry.TableName + " values ('" + singleEntry.TableValueText + "', '" + singleEntry.IsDeleted + "',  '" + dateTime + "', '', '" + singleEntry.CreatedBy + "', '')");
                    return true;
             
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateSingleEntry(SingleEntry singleEntry)
        {
            try
            {
               
                    var dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:sss");
                    context.Database.ExecuteSqlRaw("update " + singleEntry.TableName + " set Description = '" + singleEntry.TableValueText + "', IsDeleted = '" + singleEntry.IsDeleted + "', UpdatedUTC = '" + dateTime + "', UpdatedBy = '" + singleEntry.UpdatedBy + "' where id = '" + singleEntry.TableValue + "'");
                    return true;
               
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
