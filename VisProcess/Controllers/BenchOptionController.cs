using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class BenchOptionController : ControllerBase
    {
        public BenchOptionController(BenchOptionService benchOptionService)
        {
            _benchOption = benchOptionService;
        }
        private readonly BenchOptionService _benchOption;
        [HttpGet]
        [Route("GetStatus")]
        public Object GetStatus(int EmployeeId)
        {
            var valid = _benchOption.GetStatus(EmployeeId);
            var result = new
            {
                data = valid
            };
            return result;
        }
        [HttpGet]
        [Route("Getbutton")]
        public Object Getbutton(int EmployeeId)
        {
            var valid = _benchOption.Getbutton(EmployeeId);
            
            var result = new
            {
                data = valid,
                
            };
            return result;
        }
        [HttpPost]
        [Route("Startbench")]
        public Object Startbench(EmployeeAssign1 Startbench, string Worktype)
        {
          
                var valid = _benchOption.Startbench(Startbench, Worktype);

                var result = new
                {
                    data = valid
                };
                return result;
            
           
        }
        [HttpPost]
        [Route("BenchOptionWorkWithRange")]
        public Object BenchOptionWorkWithRange(BenchOptionViewModel benchrange)
        {
            var grid = _benchOption.BenchOptionWorkWithRange(benchrange);
            var result = new
            {
                data = grid
            };
            return result;
        }
        [HttpGet]
        [Route("GetDepartment")]
        public Object GetEmployee(string EmployeeId)
        {
            var grid = _benchOption.GetEmployee(EmployeeId);
            var result = new
            {
                data = grid
            };
            return result;
        }
    }
}
