using Microsoft.AspNetCore.Mvc;
using Prism_EndPoint.Models;
using Prism_EndPoint.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using System.Diagnostics;
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

        [HttpGet("byCodePlan/{role},{code},{empNo}", Name = "getPlan")]
        public async Task<IActionResult> getPlan(int role, int code, string empNo)
        {
            try
            {
                var program = await _planrepository.getDivisionPlanTeam(role, code, empNo);
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


        [HttpPost("newPlan/{progId},{divId},{processId}", Name = "newPlan")]
        public async Task<IActionResult> newPlan(int progId, int divId, int processId)
        {
            try
            {
                await _planrepository.addQmsPlan(progId, divId, processId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpPut("updateObjective/{planId},{objectives}", Name = "UpdatePlanObjectives")]
        public async Task<IActionResult> UpdatePlanObjectives(int planId, string objectives)
        {

            try
            {
                await _planrepository.UpdatePlanObj(planId, objectives);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updatemetho/{planId},{methodologies}", Name = "UpdatePlanMethodologies")]
        public async Task<IActionResult> UpdatePlanMethodologies(int planId, string methodologies)
        {

            try
            {
                await _planrepository.updateMetho(planId, methodologies);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPost("updateAudit", Name = "UpdatePlanAudit")]
        public async Task<IActionResult> UpdatePlanAudit(AuditEntry entry)
        {

            try
            {
                await _planrepository.updatePlanAudit(entry);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }


        [HttpPut("updatePlanTeamLead/{planId}", Name = "updatePlanTeamLead")]
        public async Task<IActionResult> updatePlanTeamLead(int planId)
        {

            try
            {
                await _planrepository.UpdateTeamLead(planId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpGet("QMSClause", Name = "getQMSClause")]
        public async Task<IActionResult> getQMSClause()
        {
            try
            {

                var qmsClause = await _qmsDB.Qmsmanuals
                        .Include(x => x.QmsSubClauses) 
                        .OrderBy(x => x.Id) 
                        .ToListAsync();
                List<qmsClauses> process = qmsClause.Select(e => new qmsClauses
                {
                    Clauses = e.Clause,
                    Title = e.ClauseTitle,
                    subClause = e.QmsSubClauses.Select(m => new qmsClauses.ArrayInput
                    {
                        subClause = m.Subclause,
                        Title   = m.ClauseTitle,
                        subTitle = m.SubTitle,
                        status = m.NewColumn

                    }).ToList(),

                }).ToList();


                return Ok(process);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("returnPlan/{planId},{notes}", Name = "returnPlan")]
        public async Task<IActionResult> returnPlan(int planId, string notes)
        {

            try
            {
                await _planrepository.returnPlan(planId,notes);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating the Data", ex);
            }
        }


    }
}
