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
            var parameters = new[]
            {
         new SqlParameter("@ManagerId", managerId)
     };
            var results = _procedureWork.ExecStoredProcedure<DormantClient>("EXEC GetDormantClient @ManagerId", parameters).ToList();
            return results;
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