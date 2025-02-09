using CatRescueApi.Models;
using Newtonsoft.Json.Linq;
using FluentValidation;
using RestSharp;

public class UserService : DataService, IUserService
{
    private readonly IValidator<User> _userValidator;

    public UserService(IValidator<User> userValidator) : base("./Data")
    {
        _userValidator = userValidator;
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
        usersList.Add(user);

        data["users"] = JToken.FromObject(usersList);
        await SaveJsonAsync("users", data);

        return Result<User>.Ok(user);
    }

    // Verify the user's email
    public async Task<Result<User>> VerifyEmail(int userId)
    {
        var users = await GetAllUsers();
        var user = users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return Result<User>.Fail("User not found.");
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
        user.TokenExpiresAt = DateTime.UtcNow.AddHours(24); // Token expires in 24 hours

        var data = await LoadJsonAsync<JObject>("users");
        data["users"] = JToken.FromObject(users);
        await SaveJsonAsync("users", data);

        try
        {
            await SendEmailUsingMailgun(user.Email, user.VerificationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return Result<User>.Fail("Failed to send verification email.");
        }

        return Result<User>.Ok(user);
    }
    private async Task SendEmailUsingMailgun(string recipient, string token)
    {
        var mailgunDomain = "sandboxc13dda39f6184308942ce19649289724.mailgun.org";
        var mailgunApiKey = "af9bcd0ffe67963ed1cb44523d900906-667818f5-7cdaaf98";
        var senderEmail = "postmaster@sandboxc13dda39f6184308942ce19649289724.mailgun.org";
        var subject = "Verify Your Email";
        var body = $"Click the link to verify your email: https://we-rescue-cats.io/verify?token={token}";

        var client = new RestClient($"https://api.mailgun.net/v3/{mailgunDomain}/messages");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddParameter("from", senderEmail);
        request.AddParameter("to", recipient);
        request.AddParameter("subject", subject);
        request.AddParameter("text", body);
        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"api:{mailgunApiKey}"))}");

        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"Mailgun error: {response.Content}");
        }
    }
}