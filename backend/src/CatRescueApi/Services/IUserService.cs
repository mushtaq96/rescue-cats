using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
public interface IUserService
{
    Task<Result<User>> RegisterUser(User user);
    Task<Result<User>> SendVerificationEmail(int userId);
    Task<Result<User>> VerifyEmail(string token);
    Task<Result<string>> AuthenticateLogin(string email, string password);
    Task<List<User>> GetAllUsers();
    Task<User> GetUserByEmail(string email);

}
