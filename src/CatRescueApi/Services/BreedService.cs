using CatRescueApi.Models;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

// handle operations related to cat breeds (e.g., fetching, filtering, updating)
namespace CatRescueApi.Services
{
    public class BreedService : DataService, IBreedService
    {
        private readonly HttpClient _httpClient;
        private readonly IValidator<Breed> _breedValidator;
        private readonly ILogger<BreedService> _logger;
        public BreedService(IValidator<Breed> breedValidtor, ILogger<BreedService> logger, string dataPath = "./Data")
                : base(dataPath)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.thecatapi.com/v1/");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "live_dFUyLpLZCzjoDHgqdomFUigbmyTnuyW1aqkWhfEg6bqYkx4AFLD7QsreDTL6yvwN");
            _breedValidator = breedValidtor;
            _logger = logger;
        }
        public async Task<List<Breed>> GetAllBreeds()
        {
            _logger.LogInformation("Fetching all breeds from local JSON file.");
            var data = await LoadJsonAsync<JObject>("breeds");
            var breedsArray = data?["breeds"] as JArray;
            var breeds = breedsArray?.ToObject<List<Breed>>() ?? new List<Breed>();

            foreach (var breed in breeds)
            {
                if (string.IsNullOrEmpty(breed.ImageUrl))
                {
                    _logger.LogInformation($"Fetching image URL for breed: {breed.Name}");
                    breed.ImageUrl = await GetBreedImageUrl(breed.Name);
                    await SaveImageUrlToJSON(breed);
                }
            }

            _logger.LogInformation("All breeds fetched successfully.");
            return breeds;
        }

        private async Task<string?> GetBreedImageUrl(string breedName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"breeds/search?name={breedName}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch breed details for {breedName}. Status Code: {response.StatusCode}");
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();
                var breeds = JsonConvert.DeserializeObject<List<dynamic>>(content); // dynamic to handle flexible JSON

                if (breeds != null && breeds.Count > 0 && breeds[0].reference_image_id != null)
                {
                    var imageResponse = await _httpClient.GetAsync($"images/{breeds[0].reference_image_id}");
                    if (imageResponse.IsSuccessStatusCode)
                    {
                        var imageContent = await imageResponse.Content.ReadAsStringAsync();
                        var image = JsonConvert.DeserializeObject<dynamic>(imageContent);
                        return image.url?.ToString();
                    }
                }

            }
            catch
            {
                _logger.LogWarning($"No image found for breed: {breedName}.");
            }

            return null;
        }
        private async Task SaveImageUrlToJSON(Breed breed)
        {
            var data = await LoadJsonAsync<JObject>("breeds");
            var breedsArray = data["breeds"] as JArray;
            var breeds = breedsArray?.ToObject<List<Breed>>() ?? new List<Breed>();

            var targetBreed = breeds.FirstOrDefault(b => b.Id == breed.Id);
            if (targetBreed != null)
            {
                targetBreed.ImageUrl = breed.ImageUrl;
                data["breeds"] = JToken.FromObject(breeds);
                await SaveJsonAsync("breeds", data);
            }
        }

        public async Task<Breed?> GetBreedById(int id)
        {
            var breeds = await GetAllBreeds();
            return breeds.FirstOrDefault(b => b.Id == id);
        }
        
        public async Task<List<Breed>> FilterBreeds(string? name = null, bool? isIsGoodWithKids = null, bool? isGoodWithDogs = null)
        {
            _logger.LogInformation("Filtering breeds based on provided criteria.");
            var breeds = await GetAllBreeds();

            if (!string.IsNullOrEmpty(name))
            {
                _logger.LogInformation($"Applying filter: Name contains '{name}'.");
                breeds = breeds.Where(b => b.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (isIsGoodWithKids.HasValue)
            {
                _logger.LogInformation($"Applying filter: IsGoodWithKids = {isIsGoodWithKids.Value}.");
                breeds = breeds.Where(b => b.IsGoodWithKids == isIsGoodWithKids).ToList();
            }

            if (isGoodWithDogs.HasValue)
            {
                _logger.LogInformation($"Applying filter: IsGoodWithDogs = {isGoodWithDogs.Value}.");
                breeds = breeds.Where(b => b.IsGoodWithDogs == isGoodWithDogs).ToList();
            }

            _logger.LogInformation("Breed filtering completed.");
            return breeds;
        }
    }
}