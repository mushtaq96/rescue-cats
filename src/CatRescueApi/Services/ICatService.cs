using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;

namespace CatRescueApi.Services
{
    public interface ICatService
    {
        Task<IEnumerable<CatDto>> GetCatsByTenantAsync(string tenantId);
        Task<CatDto> GetCatByIdAsync(int id);
    }
}