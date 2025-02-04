using Microsoft.AspNetCore.Mvc;
using CatRescueApi.Models;
using CatRescueApi.Services;

// encapsulation, controller doesn't know how data is fetched, it only interacts with the service
namespace CatRescueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BreedController : ControllerBase
    {
        private readonly IBreedService _breedService;
        public BreadController(IBreedService breedService)
        {
            _breedService = breedService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreedDto>>> GetBreeds()
        {
            var breeds = await _breedService.GetAllBreedsAsync();
            return Ok(breeds);
        }
    }
}