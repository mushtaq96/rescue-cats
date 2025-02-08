using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
using CatRescueApi.Data;
using Microsoft.EntityFrameworkCore; // needed for AsQueryable and ToListAsync
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// handle operations related to cat breeds (e.g., fetching, filtering, updating)
namespace CatRescueApi.Services
{
    public class BreedService : DataService, IBreedService
    {
        public BreedService(string dataPath = "./Data")
                : base(dataPath)
        {
        }

        public async Task<List<Breed>> GetAllBreeds()
        {
            var data = await LoadJsonAsync<Dictionary<string, object>>("breeds");
            return ((JArray)data["breeds"])?.ToObject<List<Breed>>() ?? new List<Breed>();
        }

        public async Task<Breed?> GetBreedById(int id)
        {
            var breeds = await GetAllBreeds();
            return breeds.FirstOrDefault(b => b.Id == id);
        }
    }
}