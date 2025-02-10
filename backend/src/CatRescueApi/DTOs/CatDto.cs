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

        public static CatDto MapToDto(Cat cat) => new()
        {
            Id = cat.Id,
            Name = cat.Name,
            BreedId = cat.BreedId,
            Location = cat.Location,
            TenantId = cat.TenantId,
            ImageUrl = GetBreedImageUrl(cat.BreedId),
            Description = cat.Description ?? string.Empty
        };

        private static string GetBreedImageUrl(int breedId)
        {
            var breedsJson = File.ReadAllText("./Data/breeds.json"); // Adjust path as needed
            var breedsData = JObject.Parse(breedsJson);
            var breeds = breedsData["breeds"]?.ToObject<List<Breed>>(); // Deserialize JSON to list of Breed objects
            var breed = breeds?.FirstOrDefault(b => b.Id == breedId);

            return breed?.ImageUrl ?? ""; // Return ImageUrl or empty string if not found
        }
    }
}