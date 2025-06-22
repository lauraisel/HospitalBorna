using Hospital.DTOs;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _medicalRecordService.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _medicalRecordService.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto createDto)
        {
            var created = await _medicalRecordService.CreateMedicalRecordAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalRecordDto updateDto)
        {
            var updated = await _medicalRecordService.UpdateMedicalRecordAsync(id, updateDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _medicalRecordService.DeleteMedicalRecordAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var records = await _medicalRecordService.GetByPatientIdAsync(patientId);
            return Ok(records);
        }

        [HttpGet("patient/{patientId}/csv")]
        public async Task<IActionResult> GetMedicalRecordsCsv(int patientId)
        {
            var fileStreamResult = await _medicalRecordService.GetMedicalRecordsCsvAsync(patientId);
            if (fileStreamResult == null)
                return NotFound();

            return fileStreamResult;
        }
    }
}
