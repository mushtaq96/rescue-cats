using System.Linq;
using System.Threading.Tasks;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
using CatRescueApi.Data;

namespace CatRescueApi.Services
{
    public class AdoptionService : IAdoptionService
    {
        private readonly ApplicationDbContext _context;
        public AdoptionService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get adoption by id
        public async Task<AdoptionDto> GetAdoptionByIdAsync(int id)
        {
            var adoption = await _context.Adoptions.FindAsync(id);
            return adoption != null ? MapToDto(adoption) : null;
        }
        public async Task<AdoptionDto> SubmitAdoptionAsync(AdoptionRequest request)
        {
            var adoption = new Adoption
            {
                UserId = request.UserId,
                CatId = request.CatId,
                Status = "pending"
            };
            _context.Adoptions.Add(adoption);
            await _context.SaveChangesAsync();

            return MapToDto(adoption);
        }
        private static AdoptionDto MapToDto(Adoption adoption) => new AdoptionDto
        {
            Id = adoption.Id,
            CatId = adoption.CatId,
            UserId = adoption.UserId,
            Status = adoption.Status
        };
    }
}