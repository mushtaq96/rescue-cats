using CatRescueApi.Models;

namespace CatRescueApi.DTOs
{
    public class BreedDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsGoodWithKids { get; set; }
        public bool IsGoodWithDogs { get; set; }
        public string? Description { get; set; }

        public static BreedDto MapToDto(Breed breed) => new()
        {
            Id = breed.Id,
            Name = breed.Name,
        };
    }
}