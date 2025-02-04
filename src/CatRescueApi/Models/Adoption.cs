namespace CatRescueApi.Models
{
    // Adoption model
    public class Adoption
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CatId { get; set; }
        public string Status { get; set; } = "pending" ;
    }
}