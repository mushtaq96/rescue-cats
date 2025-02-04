using Microsoft.AspNetCore.Mvc;
using CatRescueApi.Models;
using CatRescueApi.Services;

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
        public async Task<ActionResult<AdoptionDto>> SubmitAdoption([FromBody] AdoptionRequest request)
        {
            var adoption = await _adoptionService.SubmitAdoptionAsync(request);
            return CreatedAtAction(nameof(GetAdoption), new { id = adoption.Id }, adoption);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionDto>> GetAdoption(int id)
        {
            var adoption = await _adoptionService.GetAdoptionByIdAsync(id);
            if (adoption == null) return NotFound();
            return Ok(adoption);
        }
    }
}