using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using Prism_EndPoint.Repositories;

namespace Prism_EndPoint.Controllers
{
    [Authorize]
    [Route("api/plan/[controller]")]
    [ApiController]
    public class ChecklistController : Controller
    {
        private readonly PrismDbContext _qmsDB;
        private readonly IqmsChecklist _repository;
        private readonly ILogger<HomeController> _logger;
        private readonly IqmsPlan _planrepository;
        public ChecklistController(IqmsChecklist repository, IqmsPlan planrepository, ILogger<HomeController> logger, PrismDbContext prismDb)
        {
            _repository = repository;
            _planrepository = planrepository;
            _logger = logger;
            _qmsDB = prismDb;
        }

        [HttpGet("checkList/{code}", Name = "getCheckList")]
        public async Task<IActionResult> getCheckList(int code)
        {
            try
            {
                var program = await _repository.getCheckListItem(code);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPost("createCheckList/{code}", Name = "newcreateCheckList")]
        public async Task<IActionResult> newcreateCheckList(int code)
        {
            try
            {

               await _repository.createCheckList(code);
                return Ok();
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }

        [HttpPut("updateChecklisTData", Name = "updateChecklisTData")]
        public async Task<IActionResult> updateChecklisTData(updateCheckList checklist)
        {

            try
            {

                await _repository.updateCheckListData(checklist);
                return Ok(new { message = "Checklist saved successfully!" });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }

        [HttpPut("updateStatus/{id}", Name = "updateStatus")]
        public async Task<IActionResult> updateStatus(int id)
        {
            try
            {

                await _repository.updateStatus(id);
                return Ok(new { message = "Checklist saved successfully!" });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding process to the database", ex);
            }
        }




        //For the Checklist

        [HttpGet("checkList/report/{code}", Name = "getReport")]
        public async Task<IActionResult> getReport(int code)
        {
            try
            {
                var program = await _repository.GetChecklistReport(code);
                return Ok(program);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding employee to the database", ex);
            }
        }


        [HttpPut("updateReport", Name = "UpdateReport")]
        public async Task<IActionResult> UpdateReport(ReportUpdateData update)
        {
            if (update == null)
            {
                return BadRequest(new { message = "Invalid data provided." });
            }

            try
            {
                await _repository.updateReport(update);
                return Ok(new { message = "Checklist updated successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception (use a logger instead of throwing a new exception)
                return StatusCode(500, new { message = "An error occurred while updating the report.", error = ex.Message });
            }
        }



    }
}
