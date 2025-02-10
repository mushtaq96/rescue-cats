using CatRescueApi.Models;
using Newtonsoft.Json.Linq;

namespace CatRescueApi.DTOs
{
    public class CatDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int BreedId { get; set; }
        public required string Location { get; set; }
        public required string TenantId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAdopted { get; set; }
        public int Age { get; set; }
        public bool GoodWithKids { get; set; }
        public bool GoodWithDogs { get; set; }
        public int Playfulness { get; set; }
        public string BreedName { get; set; } = string.Empty;

        public static CatDto MapToDto(Cat cat)
        {
            var breedsJson = File.ReadAllText("./Data/breeds.json");
            var breedsData = JObject.Parse(breedsJson);
            var breeds = breedsData["breeds"]?.ToObject<List<Breed>>();

            return new CatDto
            {
                Id = cat.Id,
                Name = cat.Name,
                BreedId = cat.BreedId,
                BreedName = breeds?.FirstOrDefault(b => b.Id == cat.BreedId)?.Name ?? "",
                Location = cat.Location,
                TenantId = cat.TenantId,
                ImageUrl = breeds?.FirstOrDefault(b => b.Id == cat.BreedId)?.ImageUrl ?? "",
                Description = cat.Description ?? "",
                IsAdopted = cat.IsAdopted,
                Age = cat.Age,
                GoodWithKids = breeds?.FirstOrDefault(b => b.Id == cat.BreedId)?.IsGoodWithKids ?? false,
                GoodWithDogs = breeds?.FirstOrDefault(b => b.Id == cat.BreedId)?.IsGoodWithDogs ?? false,
                Playfulness = cat.Playfulness
            };
        }

    }
}