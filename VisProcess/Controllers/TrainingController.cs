using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/Allocation")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly TrainingService _trainingService;
        public TrainingController(TrainingService trainingService)
        {
            _trainingService = trainingService;
        }
        [HttpGet]
        [Route("getJobsAndEmployeesForTraining/{EmployeeId}/{ProcessId}/{IsPendingJob}")]
        public Object GetPendingJobsAndEmployeesForTraining(int EmployeeId, int ProcessId, int IsPendingJob)
        {
            return _trainingService.GetPendingJobsAndEmployeesForTraining(EmployeeId, ProcessId, IsPendingJob);
        }
    }
}
