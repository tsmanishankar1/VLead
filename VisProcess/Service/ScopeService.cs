using Vis.VleadProcessV3.Models;
namespace VisProcess.Service
{
    public class ScopeService
    {
        private readonly Vis.VleadProcessV3.Services.ScopeService _scopeService;
        public ScopeService(Vis.VleadProcessV3.Services.ScopeService scopeService)
        {
            _scopeService = scopeService;
        }
        public Scope GetScope(int id)
        {
            return _scopeService.GetScopeDetails(id);
        }
    }
}
