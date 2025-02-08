namespace CatRescueApi.Models
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsGoodWithKids { get; set; } = false;
        public bool IsGoodWithDogs { get; set; } = false;
        public string? Description { get; set; } = null;

    }
}