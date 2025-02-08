namespace CatRescueApi.Models
{
    public class Cat
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int BreedId { get; set; }
        public string Location { get; set; } = "";
        public string? Description { get; set; } = null;
        public string TenantId { get; set; } = "";
    }
}