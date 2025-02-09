using CatRescueApi.Models;

namespace CatRescueApi.DTOs
{
    public class CatDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int BreedId { get; set; }
        public required string Location { get; set; }
        public required string TenantId { get; set; }

        public static CatDto MapToDto(Cat cat) => new()
        {
            Id = cat.Id,
            Name = cat.Name,
            BreedId = cat.BreedId,
            Location = cat.Location,
            TenantId = cat.TenantId
        };
    }
}