using Microsoft.AspNetCore.Mvc;
using Prism_EndPoint.Models;
using Prism_EndPoint.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Prism_EndPoint.Controllers
{
    [Route("api/plan/[controller]")]
    [ApiController]
    public class PlanController : Controller
    {

        private readonly PrismDbContext _qmsDB;
        private readonly IqmsProgram _repository;
        private readonly ILogger<HomeController> _logger;
        private readonly IqmsPlan _planrepository;
        public PlanController(IqmsProgram repository, IqmsPlan planrepository, ILogger<HomeController> logger, PrismDbContext prismDb)
        {
            _repository = repository;
            _planrepository = planrepository;
            _logger = logger;
            _qmsDB = prismDb;
        }

        [HttpGet("byCodePlan/{code},{empNo}", Name = "getPlan")]
        public async Task<IActionResult> getPlan(int code, string empNo)
        {
            try
            {
                var program = await _planrepository.getDivisionPlanTeam(code, empNo);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }


        [HttpGet("statusPlan/{code},{division}", Name = "getPlanDivision")]
        public async Task<IActionResult> getProcessDivision(int code, string division)
        {
            try
            {
                var program = await _planrepository.getDivisionprocessPlan(division, code);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }


        [HttpGet("processplan/{code},{divId},{progId}", Name = "getProcessPlan")]
        public async Task<IActionResult> getProcessPlan(int code, int divId, int progId)
        {
            try
            {
                var program = await _planrepository.getProcessPlan(code, divId, progId);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

    }
}
