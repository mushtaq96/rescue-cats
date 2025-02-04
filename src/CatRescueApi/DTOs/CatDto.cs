namespace CatRescueApi.DTOs
{
    public class CatDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BreedId { get; set; }
        public string Location { get; set; }
        public string TenantId { get; set; } // For multi-tenancy
    }
}