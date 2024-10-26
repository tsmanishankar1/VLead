using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Local.Service
{
    public class DropdownService
    {
        private readonly TableWork _tableWork;
        public DropdownService(TableWork tableWork)
        {
            _tableWork= tableWork;
        }

        public IList<CustomerViewModel> GetCustomers()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsBlacklisted == false && x.IsDeleted == false).Select(x => new CustomerViewModel { Id = x.Id, Name = x.Name, ShortName = x.ShortName }).ToList();
        }
    }
}
