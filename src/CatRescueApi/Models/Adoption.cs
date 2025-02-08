using System.ComponentModel.DataAnnotations;

namespace CatRescueApi.Models
{
    public class Adoption
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User ID is required.")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Cat ID is required.")]
        public int CatId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}