using System.Collections.Generic;
using System.Threading.Tasks;
using CatRescueApi.DTOs;
using CatRescueApi.Models;

namespace CatRescueApi.Services
{
    public interface IBreedService
    {
        Task<List<Breed>> GetAllBreeds();
        Task<Breed?> GetBreedById(int id);
    }
}