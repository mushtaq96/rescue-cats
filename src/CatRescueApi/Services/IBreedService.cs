using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;

namespace CatRescueApi.Services
{
    public interface IBreedService
    {
        Task<IEnumerable<BreedDto>> GetAllBreedsAsync();
        Task<IEnumerable<BreedDto>> FilterBreedsAsync(FilterRequest request);
    }
}