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

        [HttpPost("send-verification-email/{userId}")]
        public async Task<IActionResult> SendVerificationEmail(int userId)
        {
            var result = await _userService.SendVerificationEmail(userId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("verify-email/{token}")]
        public async Task<IActionResult> VerifyEmail([FromRoute] string token)
        {
            var users = await _userService.GetAllUsers();
            var user = users.FirstOrDefault(u => u.VerificationToken == token && u.TokenExpiresAt > DateTime.UtcNow);
            if (user == null)
            {
                return BadRequest("Invalid or expired token.");
            }

            var verificationResult = await _userService.VerifyEmail(user.Id);
            return verificationResult.IsSuccess ? Ok("Email verified successfully!") : BadRequest(verificationResult.Error);
        }
    }
}