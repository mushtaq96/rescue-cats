using Microsoft.AspNetCore.Mvc;
using CatRescueApi.Models;
using CatRescueApi.Services;
using CatRescueApi.DTOs;

namespace CatRescueApi.Controllers
{
    // encapsulation, controller doesn't know how data is fetched, it only interacts with the service
    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatsController(ICatService catService)
        {
            _catService = catService;
        }

        // GET api/cats?tenantId=123
        [HttpGet]
        public async Task<IActionResult> GetCats(string tenantId)
        {
            var cats = await _catService.GetCatsByTenantAsync(tenantId);
            var catDtos = cats.Select(CatDto.MapToDto).ToList();
            return Ok(catDtos);
        }

        // GET api/cats/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatById(int id)
        {
            var cat = await _catService.GetCatByIdAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        // POST api/cats
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Cat cat)
        {
            var result = await _catService.RegisterCat(cat);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}