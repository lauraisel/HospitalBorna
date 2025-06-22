using Hospital.DTOs;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckupController : ControllerBase
    {
        private readonly ICheckupService _checkupService;

        public CheckupController(ICheckupService checkupService)
        {
            _checkupService = checkupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var checkups = await _checkupService.GetAllAsync();
            return Ok(checkups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var checkup = await _checkupService.GetByIdAsync(id);
            if (checkup == null)
                return NotFound();

            return Ok(checkup);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCheckupDto createDto)
        {
            var created = await _checkupService.CreateCheckupAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCheckupDto updateDto)
        {
            var updated = await _checkupService.UpdateCheckupAsync(id, updateDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _checkupService.DeleteCheckupAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var checkups = await _checkupService.GetByPatientIdAsync(patientId);
            return Ok(checkups);
        }
    }
}
