using CatRescueApi.Models;
using Newtonsoft.Json.Linq;
using FluentValidation;
using RestSharp;
using RestSharp.Authenticators;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public class UserService : DataService, IUserService
{
    private readonly IValidator<User> _userValidator;
    private readonly ILogger<UserService> _logger;
    private readonly string _jwtSecretKey;
    private readonly string _mailgunDomain;
    private readonly string _mailgunApiKey;
    private readonly string _senderEmail;

    public UserService(IValidator<User> userValidator, ILogger<UserService> logger, IConfiguration configuration) : base("./Data")
    {
        _userValidator = userValidator;
        _logger = logger;
        _jwtSecretKey = configuration["JwtSecretKey"] ?? throw new InvalidOperationException("JwtSecretKey is missing in appsettings.json");
        _mailgunDomain = configuration["MailgunDomain"] ?? throw new InvalidOperationException("MailgunDomain is missing in appsettings.json");
        _mailgunApiKey = configuration["MailgunApiKey"] ?? throw new InvalidOperationException("MailgunApiKey is missing in appsettings.json");
        _senderEmail = configuration["SenderEmail"] ?? throw new InvalidOperationException("SenderEmail is missing in appsettings.json");
    }

    // Existing method to get all users
    public async Task<List<User>> GetAllUsers()
    {
        var data = await LoadJsonAsync<JObject>("users");
        return ((JArray)data["users"])?.ToObject<List<User>>() ?? new List<User>();
    }

    // Register a new user (existing method)
    public async Task<Result<User>> RegisterUser(User user)
    {
        var validationResult = await _userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return Result<User>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var data = await LoadJsonAsync<JObject>("users");
        if (data == null)
        {
            data = new JObject();
            data["users"] = new JArray();
        }

        var usersList = ((JArray)data["users"])?.ToObject<List<User>>() ?? new List<User>();
        if (usersList.Any(u => u.Email == user.Email))
        {
            return Result<User>.Fail("Email already registered");
        }

        user.Id = usersList.Count > 0 ? usersList.Max(u => u.Id) + 1 : 1;
        user.IsVerified = false; // Ensure new users are not verified by default
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash the password
        usersList.Add(user);

        data["users"] = JToken.FromObject(usersList);
        await SaveJsonAsync("users", data);

        return Result<User>.Ok(user);
    }

    // Verify the user's email
    public async Task<Result<User>> VerifyEmail(string token)
    {
        var users = await GetAllUsers();
        var user = users.FirstOrDefault(u => u.VerificationToken == token && u.TokenExpiresAt > DateTime.UtcNow);
        if (user == null)
        {
            return Result<User>.Fail("Invalid or expired token.");
        }

        user.IsVerified = true;
        user.VerificationToken = null; // Clear the token after verification
        user.TokenExpiresAt = null;   // Clear the expiration time

        var data = await LoadJsonAsync<JObject>("users");
        data["users"] = JToken.FromObject(users);
        await SaveJsonAsync("users", data);

        return Result<User>.Ok(user);
    }

    // Generate a unique verification token
    private string GenerateVerificationToken()
    {
        return Guid.NewGuid().ToString("N");
    }
    // Send a verification email to the user
    public async Task<Result<User>> SendVerificationEmail(int userId)
    {
        var users = await GetAllUsers();
        var user = users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return Result<User>.Fail("User not found.");
        }

        if (user.IsVerified)
        {
            return Result<User>.Fail("User is already verified.");
        }

        user.VerificationToken = GenerateVerificationToken();
        try
        {
            user.VerificationToken = GenerateVerificationToken();
            user.TokenExpiresAt = DateTime.UtcNow.AddHours(24);

            var data = await LoadJsonAsync<JObject>("users");
            data["users"] = JToken.FromObject(users);
            await SaveJsonAsync("users", data);

            var result = await SendEmailUsingMailgun(user.Email, user.VerificationToken);

            if (!result.IsSuccessStatusCode)
            {
                var error = result.Content;
                _logger.LogError($"Mailgun error {result.StatusCode}: {error}");
                return Result<User>.Fail($"Failed to send verification email: {error}");
            }

            return Result<User>.Ok(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            _logger.LogError(ex, "Failed to send verification email.");
            return Result<User>.Fail("Failed to send verification email.");
        }


    }
    // Send an email using the Mailgun API
    private async Task<RestResponse> SendEmailUsingMailgun(string recipient, string token)
    {
        // Configuration
        var mailgunDomain = _mailgunDomain;
        var mailgunApiKey = _mailgunApiKey;
        var senderEmail = _senderEmail;
        var subject = "Verify Your Email";

        // Create HTML body with proper encoding
        var body = $@"
        <html>
            <body>
                <p>Click the link to verify your email: 
                   <a href=""https://localhost:{5197}/verify?token={System.Web.HttpUtility.HtmlEncode(token)}"">
                   https://localhost:{5197}/verify?token={System.Web.HttpUtility.HtmlEncode(token)}
                   </a>
                </p>
            </body>
        </html>";

        try
        {
            // Configure client with modern authentication
            var options = new RestClientOptions($"https://api.mailgun.net/v3/{mailgunDomain}/messages")
            {
                Authenticator = new HttpBasicAuthenticator("api", mailgunApiKey)
            };

            using var client = new RestClient(options);

            // Create request with proper HTML content
            var request = new RestRequest()
                .AddParameter("from", senderEmail)
                .AddParameter("to", recipient)
                .AddParameter("subject", subject)
                .AddParameter("html", body);

            // Execute with retry logic
            var response = await client.PostAsync(request);

            // Handle response
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Invalid API key or authentication failed");
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    throw new Exception("Invalid request format");
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    throw new Exception("Rate limit exceeded");

                throw new Exception($"Mailgun error: {response.Content}");
            }
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
    }
    // Authenticate a user
    public async Task<User?> GetUserByEmail(string email)
    {
        var data = await LoadJsonAsync<JObject>("users");
        var usersList = ((JArray)data["users"])?.ToObject<List<User>>() ?? new List<User>();
        return usersList.FirstOrDefault(u => u.Email == email);
    }
    public async Task<Result<string>> AuthenticateLogin(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return Result<string>.Fail("Invalid credentials.");
        }
        return Result<string>.Ok(GenerateJwtToken(user));
    }
    // Generate a JWT token for the user
    private string GenerateJwtToken(User user)
    {
        // Create security key that will be used to sign the token
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
        // Create credentials from the security key and the algorithm
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: "we-rescue-cats",
            audience: "we-rescue-cats",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}