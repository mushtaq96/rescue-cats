namespace CatRescueApi.DTOs
{
    public class BreedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsGoodWithKids { get; set; }
        public bool IsGoodWithDogs { get; set; }
        public string Description { get; set; }
    }
}