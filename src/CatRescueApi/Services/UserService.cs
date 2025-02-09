using CatRescueApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FluentValidation;
public class UserService : DataService, IUserService
{
    private readonly IValidator<User> _userValidator;
    // private static int _nextUserId = 1;

    public UserService(IValidator<User> userValidator) : base("./Data")
    {
        _userValidator = userValidator;
    }

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

        usersList.Add(user);
        data["users"] = JToken.FromObject(usersList);
        await SaveJsonAsync("users", data);

        return Result<User>.Ok(user);
    }
    public async Task<List<User>> GetAllUsers()
    {
        var data = await LoadJsonAsync<JObject>("users");
        return ((JArray)data["users"])?.ToObject<List<User>>() ?? new List<User>();
    }
}