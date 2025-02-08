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

        public AdoptionService(ApplicationDbContext context) => _context = context;

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

            return AdoptionDto.MapToDto(adoption);
        }

        public async Task<AdoptionDto?> GetAdoptionByIdAsync(int id)
        {
            var adoption = await _context.Adoptions.FindAsync(id);
            return adoption != null ? AdoptionDto.MapToDto(adoption) : null;
        }
    }
}