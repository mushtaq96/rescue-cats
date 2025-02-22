using CatRescueApi.DTOs;

namespace CatRescueApi.Models
{
    public class Cat
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BreedId { get; set; }
        public AddressDto Location { get; set; } = new AddressDto();
        public string? Description { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public bool IsAdopted { get; set; }
        public int Age { get; set; }
        public int Playfulness { get; set; }
    }
}