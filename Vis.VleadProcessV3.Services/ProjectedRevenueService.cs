using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Services
{
    public class ProjectedRevenueService
    {
        private readonly ApplicationDbContext _context;
        public ProjectedRevenueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public FteProjectedRevenue CreateProjectedRevenue(ProjectedRevenueRequest revenue)
        {
            var projectedRevenue = new FteProjectedRevenue
            {
                DivisionId = revenue.DivisionId,
                CustomerId = revenue.CustomerId,
                ProjectedRevenueMonth = revenue.ProjectedRevenueMonth,
                ProjectedRevenue = revenue.ProjectedRevenue,
                IsDeleted = false,
                CreatedBy = revenue.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.FteProjectedRevenues.Add(projectedRevenue);
            _context.SaveChanges();

            return projectedRevenue;
        }

        public List<ProjectedRevenueResponse> GetProjectedRevenue()
        {
            var projectedRevenue = _context.FteProjectedRevenues.Where(p => p.IsDeleted == false).ToList();
            var getProjectedRevenue = (from p in projectedRevenue
                                       join d in _context.Divisions on p.DivisionId equals d.Id
                                       join c in _context.Customers on p.CustomerId equals c.Id
                                       select new ProjectedRevenueResponse
                                       {
                                           Division = d.DivisionName,
                                           Customer = c.ShortName,
                                           ProjectedRevenueMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.ProjectedRevenueMonth),
                                           ProjectedRevenue = p.ProjectedRevenue
                                       }).ToList();

            return getProjectedRevenue;
        }
    }
}