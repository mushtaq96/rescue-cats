using Microsoft.AspNetCore.Mvc;
using CatRescueApi.Models;
using CatRescueApi.Services;
using CatRescueApi.DTOs;

namespace CatRescueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdoptionsController : ControllerBase
    {
        private readonly IAdoptionService _adoptionService;
        private readonly ILogger<AdoptionService> _logger;

        public AdoptionsController(IAdoptionService adoptionService, ILogger<AdoptionService> iLogger)
        {
            _adoptionService = adoptionService;
            _logger = iLogger;
        }

        [HttpPost]
        public async Task<ActionResult<AdoptionDto>> SubmitAdoption([FromBody] Adoption adoption)
        {
            // Submit the adoption
            var result = await _adoptionService.SubmitAdoption(adoption);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionDto>> GetAdoption(string id)
        {
            var adoption = await _adoptionService.GetAdoptionById(id);
            if (adoption == null) return NotFound();
            return Ok(AdoptionDto.MapToDto(adoption));
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string newStatus)
        {
            var result = await _adoptionService.UpdateStatus(id, newStatus);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("check")]
        public async Task<ActionResult<bool>> CheckIfUserHasApplied([FromQuery] string catId, [FromQuery] string userId)
        {
            try
            {
                var hasApplied = await _adoptionService.CheckIfUserHasApplied(catId, userId);
                return Ok(hasApplied);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking adoption application status for catId: {CatId}, userId: {UserId}", catId, userId);
                return StatusCode(500, false);
            }
        }
    }
}