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

        public AdoptionsController(IAdoptionService adoptionService)
        {
            _adoptionService = adoptionService;
        }

        [HttpPost]
        public async Task<ActionResult<AdoptionDto>> SubmitAdoption([FromBody] Adoption adoption)
        {
            // Submit the adoption
            var result = await _adoptionService.SubmitAdoption(adoption);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetAdoption), new { id = result.Value.Id }, AdoptionDto.MapToDto(result.Value))
                : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionDto>> GetAdoption(int id)
        {
            var adoption = await _adoptionService.GetAdoptionById(id);
            if (adoption == null) return NotFound();
            return Ok(AdoptionDto.MapToDto(adoption));
        }
    }
}