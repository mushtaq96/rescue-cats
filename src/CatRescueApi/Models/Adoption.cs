
namespace CatRescueApi.Models
{
    public class Adoption
    {
        public int Id { get; set; }
        public required string UserId { get; set; }

        public int CatId { get; set; }

        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}