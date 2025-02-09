using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<Result<User>> RegisterUser(User user);
    Task<Result<User>> SendVerificationEmail(int userId);
    Task<Result<User>> VerifyEmail(int userId);
}
