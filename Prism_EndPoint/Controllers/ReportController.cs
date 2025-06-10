using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism_EndPoint.Models;
using Prism_EndPoint.Repositories;

namespace Prism_EndPoint.Controllers
{

    [Route("api/report/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly PrismDbContext _qmsDB;
        private readonly IqmsProgram _repository;
        private readonly ILogger<HomeController> _logger;
        private readonly IqmsReport _Report;

        public ReportController(IqmsProgram repository, IqmsReport planrepository, ILogger<HomeController> logger, PrismDbContext prismDb)
        {
            _repository = repository;
            _Report = planrepository;
            _logger = logger;
            _qmsDB = prismDb;
        }


        [HttpGet("byCode/{code}", Name = "getReports")]
        public async Task<IActionResult> getReports(string code)
        {
            try
            {
               var program = await _Report.GetReportAsync(code);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }
    }
}
