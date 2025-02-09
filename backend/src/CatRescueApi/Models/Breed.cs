namespace CatRescueApi.Models
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsGoodWithKids { get; set; } = false;
        public bool IsGoodWithDogs { get; set; } = false;
        public string? Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;

    }
}