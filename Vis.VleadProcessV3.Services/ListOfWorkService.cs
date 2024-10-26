using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class ListofWorkService
    {
        private readonly ViewWork _viewWork;
        public ListofWorkService(ViewWork viewWork)
        {

            _viewWork = viewWork;
           
        }
        public IEnumerable<ViewBusinessDay> GetExpiredOrders(string Listofworkinfo)
        {
            if (Listofworkinfo == "Retail")
            {
                return _viewWork.ViewBusinessDaysRepository.Get(x => x.Description == Listofworkinfo && x.WithHolidays > 90).OrderByDescending(x => x.WithHolidays).AsQueryable();
            }
            else if (Listofworkinfo == "Corporate")
            {
                return _viewWork.ViewBusinessDaysRepository.Get(x => x.Description == Listofworkinfo && x.WithHolidays > 5).OrderByDescending(x => x.WithHolidays).AsQueryable();
            }
            else
            {
                var Result1 = _viewWork.ViewBusinessDaysRepository.Get(x => x.Description == "Corporate" && x.WithHolidays > 5).AsQueryable();
                var Result2 = _viewWork.ViewBusinessDaysRepository.Get(x => x.Description == "Retail" && x.WithHolidays > 90).AsQueryable();
                var resultSum = Result1.Concat(Result2).OrderBy(x => x.WithHolidays).AsQueryable();
                return resultSum;
            }
        }
    }
}
