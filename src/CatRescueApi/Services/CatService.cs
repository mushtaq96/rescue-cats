using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
using CatRescueApi.Data;
using Microsoft.EntityFrameworkCore; // needed for ToListAsync
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CatRescueApi.Validators;

// manage operations for cats (e.g., adding new cats, retrieving by location)
namespace CatRescueApi.Services
{
    public class CatService : DataService, ICatService
    {
        private readonly IBreedService _breedService;

        public CatService(string dataPath = "./Data")
            : base(dataPath)
        {
            _breedService = new BreedService(dataPath);
        }

        public async Task<List<Cat>> GetAllCatsAsync()
        {
            var data = await LoadJsonAsync<Dictionary<string, object>>("cats");
            return ((JArray)data["cats"])?.ToObject<List<Cat>>() ?? new List<Cat>();
        }

        public async Task<Cat?> GetCatByIdAsync(int id)
        {
            var cats = await GetAllCatsAsync();
            return cats.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Cat>> GetCatsByTenantAsync(string tenantId)
        {
            var cats = await GetAllCatsAsync();
            return cats.Where(c => c.TenantId == tenantId).ToList();
        }

        public async Task<Result<Cat>> RegisterCat(Cat cat)
        {
            var validationResult = await new CatValidator().ValidateAsync(cat);
            if (!validationResult.IsValid)
            {
                return Result<Cat>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var data = await LoadJsonAsync<JObject>("cats");
            if (data == null)
            {
                data = new JObject();
                data["cats"] = new JArray();
            }
            var catsList = ((JArray)data["cats"])?.ToObject<List<Cat>>() ?? new List<Cat>();

            if (catsList.Any(c => c.Name == cat.Name))
            {
                return Result<Cat>.Fail("Cat name already registered");
            }
            cat.Id = catsList.Count > 0 ? catsList.Max(c => c.Id) + 1 : 1;

            catsList.Add(cat);
            data["cats"] = JToken.FromObject(catsList);
            await SaveJsonAsync("cats", data);

            return Result<Cat>.Ok(cat);
        }
    }

}