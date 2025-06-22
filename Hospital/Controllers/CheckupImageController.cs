using Hospital.DTOs;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckupImageController : ControllerBase
    {
        private readonly ICheckupImageService _checkupImageService;

        public CheckupImageController(ICheckupImageService checkupImageService)
        {
            _checkupImageService = checkupImageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] CreateCheckupImageDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file provided.");

            var result = await _checkupImageService.UploadImageAsync(dto);
            return CreatedAtAction(nameof(GetImageUrl), new { imageId = result.Id }, result);
        }

        [HttpGet("{imageId}/url")]
        public async Task<IActionResult> GetImageUrl(int imageId)
        {
            try
            {
                var url = await _checkupImageService.GetImageUrlAsync(imageId);
                return Ok(new { Url = url });
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Image with ID {imageId} not found.");
            }
        }

        [HttpGet("checkup/{checkupId}")]
        public async Task<IActionResult> GetImagesByCheckupId(int checkupId)
        {
            var images = await _checkupImageService.GetImagesByCheckupIdAsync(checkupId);
            return Ok(images);
        }
    }
}
