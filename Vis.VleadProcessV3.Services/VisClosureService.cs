using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Services
{
    public class VisClosureService
    {
        private readonly ApplicationDbContext _context;
        public VisClosureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string SendVisClosureAlert()
        {
            try
            {
                var result = _context.VisClosureResults
                    .FromSqlRaw("EXEC [dbo].[SendVISClosurePendingFilesAlert]")
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

        public string SendVisClosureDailyAlert()
        {
            try
            {
                var result = _context.VisClosureResults
                    .FromSqlRaw("EXEC [dbo].[SendVISClosurePendingFilesDailyAlert]")
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