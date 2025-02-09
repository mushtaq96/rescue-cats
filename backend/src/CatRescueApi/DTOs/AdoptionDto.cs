using CatRescueApi.Models;

namespace CatRescueApi.DTOs
{
    public class AdoptionDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CatId { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static AdoptionDto MapToDto(Adoption adoption) => new()
        {
            Id = adoption.Id,
            UserId = adoption.UserId,
            CatId = adoption.CatId,
            Status = adoption.Status,
            CreatedAt = adoption.CreatedAt,
            UpdatedAt = adoption.UpdatedAt
        };
    }
}