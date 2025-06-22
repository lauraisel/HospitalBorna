using Hospital.DTOs;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prescriptions = await _prescriptionService.GetAllAsync();
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prescription = await _prescriptionService.GetByIdAsync(id);
            if (prescription == null)
                return NotFound();

            return Ok(prescription);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto createDto)
        {
            var created = await _prescriptionService.CreatePrescriptionAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrescriptionDto updateDto)
        {
            var updated = await _prescriptionService.UpdatePrescriptionAsync(id, updateDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _prescriptionService.DeletePrescriptionAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("checkup/{checkupId}")]
        public async Task<IActionResult> GetByCheckupId(int checkupId)
        {
            var prescriptions = await _prescriptionService.GetByCheckupIdAsync(checkupId);
            return Ok(prescriptions);
        }
    }
}
