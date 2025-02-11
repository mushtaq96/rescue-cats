
using System.Text.Json.Serialization;

namespace CatRescueApi.Models
{
    public class Adoption
    {
        public string Id { get; set; } = string.Empty;
        public int UserId { get; set; }

        public int CatId { get; set; }

        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Address Location { get; set; } = new Address();
        public string Reason { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}