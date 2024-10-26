using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerQueryService
    {
        private readonly ViewWork _viewWork;
        public CustomerQueryService(IConfiguration configuration,ViewWork viewWork)
        {
            _viewWork = viewWork;
        }
        public int GetNotApprovedQueryForSPJobsToCCCount()
        {
            return _viewWork.ViewJobQueryInCCRepository.Get(x => x.StatusId == 22 && x.IsActive == true).Count();
        }
        public IEnumerable<ViewJobQueryInCc> GetNotApprovedQueryForSPJobsToCC()
        {
            return _viewWork.ViewJobQueryInCCRepository.Get(x => x.StatusId == 22 && x.IsActive == true).ToList();
        }

    }
}
