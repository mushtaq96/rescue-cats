
using System.Text.Json.Serialization;

namespace CatRescueApi.Models
{
    public class Adoption
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string UserId { get; set; }

        public int CatId { get; set; }

        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}