
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using Prism_EndPoint.Repositories;

namespace Prism_EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly PrismDbContext _qmsDB;
        private readonly IqmsProgram _repository;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IqmsProgram repository, ILogger<HomeController> logger, PrismDbContext prismDb)
        {
            _repository = repository;
            _logger = logger;
            _qmsDB = prismDb;
        }

        [HttpGet("byCode/{code}", Name = "getProgram")]
        public async Task<IActionResult> getProgram(string code)
        {
            try
            {
                var program = await _repository.getProgram(code);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpGet("latestProgram", Name = "getLatestProgram")]
        public async Task<IActionResult> getLatestProgram()
        {
            try
            {
                var latestProgram = _qmsDB.QmsPrograms.OrderByDescending(p => p.Id).FirstOrDefault();

                return Ok(latestProgram);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPost("addProgram", Name = "addProgram")]
        public async Task<IActionResult> createProgram()
        {
            try
            {
                await _repository.AddProgram();
                var latestProgram = _qmsDB.QmsPrograms.OrderByDescending(p => p.Id).FirstOrDefault();
                return Ok(latestProgram);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }


        [HttpPut("updateObjective/{code},{objectives}", Name = "UpdateProgramsObjectives")]
        public async Task<IActionResult> UpdateProgramsObjectives(string code, string objectives)
        {

            try
            {
                await _repository.UpdateProgramObj(code, objectives);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updateScope/{code},{scope}", Name = "UpdateProgramsScope")]
        public async Task<IActionResult> UpdateProgramsScope(string code, string scope)
        {

            try
            {
                await _repository.UpdateProgramScope(code, scope);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updateMetho/{code},{metho}", Name = "UpdateProgramsMetho")]
        public async Task<IActionResult> UpdateProgramsMetho(string code, string metho)
        {

            try
            {
                await _repository.UpdateProgramMetho(code, metho);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updateSecV/{code},{secV}", Name = "UpdateProgramsSecV")]
        public async Task<IActionResult> UpdateProgramsSecV(string code, string secV)
        {

            try
            {
                await _repository.UpdateProgramSecV(code, secV);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updateSecVI/{code},{secVI}", Name = "UpdateProgramsSecVI")]
        public async Task<IActionResult> UpdateProgramsSecVI(string code, string secVI)
        {

            try
            {
                await _repository.UpdateProgramSecVI(code, secVI);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updateSecVII/{code},{secVII}", Name = "UpdateProgramsSecVII")]
        public async Task<IActionResult> UpdateProgramsSecVII(string code, string secVII)
        {

            try
            {
                await _repository.UpdateProgramSecVII(code, secVII);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPost("addnewProcess", Name = "NewProcess")]
        public async Task<IActionResult> NewProcess(NewProcess process)
        {

            try
            {
                await _repository.newProcess(process);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpGet("getProcess", Name = "getProcess")]
        public async Task<IActionResult> getProcess()
        {

            try
            {
                var process = await _repository.getProcess();
                return Ok(process);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpPost("addnewTeam", Name = "newTeam")]
        public async Task<IActionResult> NewTeam(newTeam team)
        {

            try
            {
                await _repository.newTeam(team);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpGet("getteam", Name = "getteam")]
        public async Task<IActionResult> getTeam()
        {

            try
            {
                var process = await _repository.getTeam();
                return Ok(process);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpPost("frequencyMonth", Name = "Frequency")]
        public async Task<IActionResult> frequencyMonth(FrequencyAuditEntities Data)
        {
            try
            {
                await _repository.FrequencyAudit(Data);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpGet("getFrequency/{code}", Name = "getFrequency")]
        public async Task<IActionResult> getFrequency(int code)
        {

            try
            {
                var process = await _repository.getFrequency(code);
                return Ok(process);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }


        [HttpPost("newCredential", Name = "Credential")]
        public async Task<IActionResult> newCredential(Credential Data)
        {
            try
            {
                await _repository.newCredential(Data);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpGet("getCredential", Name = "getCredential")]
        public async Task<IActionResult> getCredential()
        {

            try
            {
                var process = await _repository.GetCredential();
                return Ok(process);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpGet("auditHeadFinalize/{code}", Name = "auditHeadFinalize")]
        public async Task<IActionResult> auditHeadFinalize(string code)
        {

            try
            {
                var program = _qmsDB.QmsPrograms.Where(x =>x.Code == code).FirstOrDefault();
                program.ApprovedAuditHead = "Yes";
                _qmsDB.QmsPrograms.Update(program);
                await _qmsDB.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpGet("qmsHeadFinalize/{code}", Name = "qmsHeadFinalize")]
        public async Task<IActionResult> qmsHeadFinalize(string code)
        {

            try
            {
                var program = _qmsDB.QmsPrograms.Where(x => x.Code == code).FirstOrDefault();
                if(program.ApprovedAuditHead == "Yes" && program.Status == "IQAPass")
                {
                    program.Status = "Completed";
                    _qmsDB.QmsPrograms.Update(program);
                    await _qmsDB.SaveChangesAsync();
                }
                else
                {
                    program.ApprovedQmslead = "Yes";
                    program.Status = "IQAPass";
                    _qmsDB.QmsPrograms.Update(program);
                    await _qmsDB.SaveChangesAsync();

                   
                    
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }



    }
}
