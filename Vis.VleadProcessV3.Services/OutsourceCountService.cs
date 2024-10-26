using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Services
{
    public class OutsourceCountService
    {
        private readonly ApplicationDbContext _context;

        public OutsourceCountService(ApplicationDbContext context)
        {
            _context = context;
        }
        public object CreateOutsourceCount(OutsourceCount outsourceCount)
        {
            if (outsourceCount == null)
            {
                throw new ArgumentException("OutsourceCount details cannot be null.");
            }
            outsourceCount.CreatedUTC = DateTime.UtcNow;
            outsourceCount.IsDeleted = false;
            _context.OutsourceCounts.Add(outsourceCount);
            _context.SaveChanges();
            var division = _context.Divisions.FirstOrDefault(d => d.Id == outsourceCount.DivisionId);
            var customer = _context.Customers.FirstOrDefault(c => c.Id == outsourceCount.CustomerId);
            return new
            {
                success = true,
                message = "Outsource Count Created Successfully"
            };
        }
        public object GetAllOutsourceCounts()
        {
            var outsourceCounts = _context.OutsourceCounts.ToList();
            return new
            {
                success = true,
                result = outsourceCounts
            };
        }
        public object UpdateOutsourceCount(OutsourceCount outsourceCount)
        {
            if (outsourceCount == null)
            {
                throw new ArgumentException("OutsourceCount details cannot be null.");
            }
            var existingOutsourceCount = _context.OutsourceCounts
                                                 .FirstOrDefault(oc => oc.Id == outsourceCount.Id && !oc.IsDeleted);

            if (existingOutsourceCount == null)
            {
                throw new ArgumentException("OutsourceCount not found.");
            }
            existingOutsourceCount.DivisionId = outsourceCount.DivisionId;
            existingOutsourceCount.CustomerId = outsourceCount.CustomerId;
            existingOutsourceCount.Date = outsourceCount.Date;
            existingOutsourceCount.OutSourceFilecount = outsourceCount.OutSourceFilecount;
            existingOutsourceCount.OutsourceAmount = outsourceCount.OutsourceAmount;
            existingOutsourceCount.UpdatedBy = outsourceCount.UpdatedBy;
            existingOutsourceCount.UpdatedUTC = DateTime.UtcNow;
            _context.SaveChanges();

            return new
            {
                success = true,
                message = "Outsource Count Updated Successfully"
            };
        }
        public object DeleteOutsourceCount(int id)
        {
            var existingOutsourceCount = _context.OutsourceCounts
                                                 .FirstOrDefault(oc => oc.Id == id && !oc.IsDeleted);

            if (existingOutsourceCount == null)
            {
                throw new ArgumentException("OutsourceCount not found.");
            }
            existingOutsourceCount.IsDeleted = true;
            _context.SaveChanges();

            return new
            {
                success = true,
                message = "Outsource Count Deleted Successfully"
            };
        }
    }
}