using CatRescueApi.Models;
using FluentValidation;
using Newtonsoft.Json.Linq;

// handle operations related to cat breeds (e.g., fetching, filtering, updating)
namespace CatRescueApi.Services
{
    public class BreedService : DataService, IBreedService
    {
        private readonly IValidator<Breed> _breedValidator;
        public BreedService(IValidator<Breed> breedValidtor, string dataPath = "./Data")
                : base(dataPath)
        {
            _breedValidator = breedValidtor;
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
        public async Task<List<Breed>> FilterBreeds(string? name = null, bool? isIsGoodWithKids = null, bool? isGoodWithDogs = null)
        {
            var breeds = await GetAllBreeds();
            if (!string.IsNullOrEmpty(name))
            {
                breeds = breeds.Where(b => b.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (isIsGoodWithKids.HasValue)
            {
                breeds = breeds.Where(b => b.IsIsGoodWithKids == isIsGoodWithKids).ToList();
            }
            if (isGoodWithDogs.HasValue)
            {
                breeds = breeds.Where(b => b.IsGoodWithDogs == isGoodWithDogs).ToList();
            }
            return breeds;
        }
    }
}