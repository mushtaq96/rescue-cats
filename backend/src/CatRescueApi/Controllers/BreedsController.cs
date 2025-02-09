using Microsoft.AspNetCore.Mvc;
using CatRescueApi.Services;
using CatRescueApi.DTOs;

// encapsulation, controller doesn't know how data is fetched, it only interacts with the service
namespace CatRescueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedService _breedService;
        public BreedsController(IBreedService breedService)
        {
            _breedService = breedService;
        }

        // GET api/breeds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreedDto>>> GetBreeds()
        {
            var breeds = await _breedService.GetAllBreeds();
            return Ok(breeds);
        }

        // GET api/breeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BreedDto>> GetBreedById(int id)
        {
            var breed = await _breedService.GetBreedById(id);
            if (breed == null) return NotFound();
            return Ok(breed);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterBreeds([FromQuery] string? name = null, [FromQuery] bool? isIsGoodWithKids = null, [FromQuery] bool? isGoodWithDogs = null)
        {
            var filteredBreeds = await _breedService.FilterBreeds(name, isIsGoodWithKids, isGoodWithDogs);
            return Ok(filteredBreeds.Select(BreedDto.MapToDto));
        }
    }
}