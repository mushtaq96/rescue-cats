using CatRescueApi.Models;

namespace CatRescueApi.DTOs
{
    public class AdoptionDto
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int CatId { get; set; }
        public required string Status { get; set; }

        public static AdoptionDto MapToDto(Adoption adoption) => new()
        {
            Id = adoption.Id,
            UserId = adoption.UserId,
            CatId = adoption.CatId,
            Status = adoption.Status
        };
    }
}