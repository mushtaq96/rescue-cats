using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;
using CatRescueApi.Models;

namespace CatRescueApi.Services
{
    public interface ICatService
    {
        Task<List<Cat>> GetAllCatsAsync();
        Task<Cat?> GetCatByIdAsync(int id);
        Task<List<Cat>> GetCatsByTenantAsync(string tenantId);
        Task<Result<Cat>> RegisterCat(Cat cat);
    }
}