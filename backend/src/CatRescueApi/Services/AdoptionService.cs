using CatRescueApi.Models;
using Newtonsoft.Json.Linq;
using CatRescueApi.Validators;
using FluentValidation;

namespace CatRescueApi.Services
{
    public class AdoptionService : DataService, IAdoptionService
    {
        private readonly IValidator<Adoption> _adoptionValidator;
        public AdoptionService(IValidator<Adoption> adoptionValidator) : base("./Data")
        {
            _adoptionValidator = adoptionValidator;
        }

        public async Task<Result<Adoption>> SubmitAdoption(Adoption adoption)
        {
            var validationResult = await _adoptionValidator.ValidateAsync(adoption);
            if (!validationResult.IsValid)
            {
                return Result<Adoption>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            // Perform background check
            if (!PerformBackGroundCheck(adoption.UserId))
            {
                return Result<Adoption>.Fail("Background check failed.");
            }

            var data = await LoadJsonAsync<JObject>("applications");
            if (data == null)
            {
                data = new JObject();
                data["applications"] = new JArray();
            }
            var applicationsList = ((JArray)data["applications"])?.ToObject<List<Adoption>>() ?? new List<Adoption>();
            if (applicationsList.Any(a => a.CatId == adoption.CatId && a.Status == "pending"))
            {
                return Result<Adoption>.Fail("Another application for this cat is already pending.");
            }
            // Assign a unique ID
            adoption.Id = applicationsList.Count > 0 ? applicationsList.Max(a => a.Id) + 1 : 1;
            adoption.CreatedAt = DateTime.UtcNow;
            adoption.Status = "pending";

            applicationsList.Add(adoption);
            data["applications"] = JToken.FromObject(applicationsList);
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
        private bool PerformBackGroundCheck(string userId)
        {
            // Simulate a background check
            Thread.Sleep(5000);
            return true;
        }

        // Define valid status transitions
        private Dictionary<string, List<string>> _validTransitions = new()
        {
            ["pending"] = new() { "approved", "rejected" },
            ["approved"] = new() { "completed" }
        };

        // Update status method
        public async Task<Result<bool>> UpdateStatus(int id, string newStatus)
        {
            var adoptions = await GetAllAdoptions();
            var adoption = adoptions.FirstOrDefault(a => a.Id == id);
            if (adoption == null)
            {
                return Result<bool>.Fail("Adoption application not found.");
            }

            // Validate transition
            if (!_validTransitions.ContainsKey(adoption.Status) || !_validTransitions[adoption.Status].Contains(newStatus))
            {
                return Result<bool>.Fail("Invalid status transition.");
            }

            // Update status
            adoption.Status = newStatus;
            adoption.UpdatedAt = DateTime.UtcNow; // Add an UpdatedAt property to track changes

            // Save updated data
            var data = await LoadJsonAsync<JObject>("applications");
            data["applications"] = JToken.FromObject(adoptions);
            await SaveJsonAsync("applications", data);

            return Result<bool>.Ok(true);
        }
    }
}