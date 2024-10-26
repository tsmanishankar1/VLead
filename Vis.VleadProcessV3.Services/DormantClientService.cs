using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using static System.Formats.Asn1.AsnWriter;

namespace Vis.VleadProcessV3.Services
{
    public class DormantClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProcedureWork _procedureWork;
        public DormantClientService(ApplicationDbContext context, ProcedureWork procedureWork)
        {
            _context = context;
            _procedureWork = procedureWork;
        }

        public List<DormantClient> GetDormantClient(int managerId)
        {
            var sixtyDaysAgo = DateTime.Now.AddDays(-60);

            var result = (from jo in _context.JobOrders
                          join cm in _context.CustomerVsManagers on jo.ClientId equals cm.CustomerId
                          join c in _context.Customers on jo.ClientId equals c.Id
                          where cm.ManagerId == managerId
&& jo.IsDeleted == false
&& cm.IsDeleted == false
&& c.IsDeleted == false
                          group jo by new { c.Id, c.ShortName, c.Name } into grouped
                          where grouped.Max(j => j.JobDate) < sixtyDaysAgo
                          select new
                          {
                              CustomerId = grouped.Key.Id,
                              ShortName = grouped.Key.ShortName
                          }).Distinct().ToList();

            var dormantClients = result.Select(item => new DormantClient
            {
                CustomerId = item.CustomerId,
                CustomerShortName = item.ShortName
            }).ToList();

            return dormantClients;
        }

        public string MailSendToDormantClient(int customerId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CustomerId", customerId)
                };

                var result = _context.DormantClientResults
                    .FromSqlRaw("EXEC [dbo].[DormantClient] @CustomerId", parameters)
                    .AsEnumerable()
                    .Select(r => r.Result)
                    .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}