using Microsoft.AspNetCore.Mvc;
using CatRescueApi.Models;

namespace CatRescueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) // Inject IUserService
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _userService.RegisterUser(user);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        // function to get all users
        [HttpGet("All")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
    }
}