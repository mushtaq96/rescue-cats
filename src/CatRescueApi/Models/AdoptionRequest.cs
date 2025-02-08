using System.ComponentModel.DataAnnotations;

namespace CatRescueApi.Models
{
    public class AdoptionRequest
    {
        [Required(ErrorMessage = "User ID is required.")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Cat ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Cat ID.")]
        public int CatId { get; set; }
    }
}