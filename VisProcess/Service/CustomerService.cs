using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Service
{
    public class CustomerService
    {
      
        private readonly Vis.VleadProcessV3.Services.CustomerService _cutomerService;
        public CustomerService(Vis.VleadProcessV3.Services.CustomerService customerService)
        {
           
            _cutomerService = customerService;
        }
        public Customer GetCustomer(int customerId, bool includeCustomerClassification = false)
        {
            return _cutomerService.GetCustomer(customerId, includeCustomerClassification);
        }
    }
}
