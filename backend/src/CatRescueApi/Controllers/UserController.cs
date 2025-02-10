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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _userService.RegisterUser(user);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.AuthenticateLogin(request.Email, request.Password);
            if (!result.IsSuccess) return Unauthorized(result.Error);
            // Get the user details
            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null) return Unauthorized();

            // Map to DTO
            var userDto = UserDto.MapToDto(user);

            // Create the response
            var response = new LoginResponseDto
            {
                Token = result.Value,
                User = userDto
            };
            return Ok(response);
        }
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // function to get all users
        [HttpGet("all-users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
        // function to send verification email
        [HttpPost("send-verification-email/{userId}")]
        public async Task<IActionResult> SendVerificationEmail(int userId)
        {
            var result = await _userService.SendVerificationEmail(userId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        // function to verify email
        [HttpGet("verify-email/{token}")]
        public async Task<IActionResult> VerifyEmail([FromRoute] string token)
        {
            var result = await _userService.VerifyEmail(token);
            return result.IsSuccess ? Ok("Email verified successfully!") : BadRequest(result.Error);
        }


    }
}