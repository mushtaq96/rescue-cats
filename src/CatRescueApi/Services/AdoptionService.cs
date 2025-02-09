using CatRescueApi.Models;
using Newtonsoft.Json.Linq;
using CatRescueApi.Validators;

namespace CatRescueApi.Services
{
    public class AdoptionService : DataService, IAdoptionService
    {
        public AdoptionService(string dataPath = "./Data") : base(dataPath) { }

        public async Task<Result<Adoption>> SubmitAdoption(Adoption adoption)
        {
            var validationResult = await new AdoptionValidator().ValidateAsync(adoption);
            if (!validationResult.IsValid)
            {
                return Result<Adoption>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var data = await LoadJsonAsync<JObject>("applications");
            if (data == null)
            {
                data = new JObject();
                data["applications"] = new JArray();
            }
            var applicationsList = ((JArray)data["applications"])?.ToObject<List<Adoption>>() ?? new List<Adoption>();

            // Assign a unique ID
            adoption.Id = applicationsList.Count > 0 ? applicationsList.Max(a => a.Id) + 1 : 1;
            adoption.CreatedAt = DateTime.UtcNow;

            applicationsList.Add(adoption);
            await SaveJsonAsync("applications", data);
            return Result<Adoption>.Ok(adoption);
        }

        public async Task<List<Adoption>> GetAllAdoptions()
        {
            var data = await LoadJsonAsync<JObject>("applications");
            var applicationsArray = data["applications"] as JArray;
            return applicationsArray?.ToObject<List<Adoption>>() ?? new List<Adoption>();
        }
        public async Task<Adoption?> GetAdoptionById(int id)
        {
            var adoptions = await GetAllAdoptions();
            return adoptions.FirstOrDefault(a => a.Id == id);
        }
    }
}