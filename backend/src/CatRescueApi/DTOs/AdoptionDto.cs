using CatRescueApi.Models;

namespace CatRescueApi.DTOs
{
    public class AdoptionDto
    {
        public string Id { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int CatId { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AddressDto Location { get; set; } = new AddressDto();
        public string Reason { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public static AdoptionDto MapToDto(Adoption adoption)
        {
            return new AdoptionDto
            {
                Id = adoption.Id,
                UserId = adoption.UserId,
                CatId = adoption.CatId,
                Status = adoption.Status,
                CreatedAt = adoption.CreatedAt,
                UpdatedAt = adoption.UpdatedAt,
                Location = new AddressDto
                {
                    Street = adoption.Location.Street,
                    City = adoption.Location.City,
                    State = adoption.Location.State,
                    PostalCode = adoption.Location.PostalCode,
                    Latitude = adoption.Location.Latitude,
                    Longitude = adoption.Location.Longitude
                },
                Reason = adoption.Reason,
                PhoneNo = adoption.PhoneNo,
                FullName = adoption.FullName,
                Email = adoption.Email
            };
        }
    }
}