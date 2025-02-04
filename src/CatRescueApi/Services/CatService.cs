using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CatRescueApi.DTOs;
using CatRescueApi.Models;

// manage operations for cats (e.g., adding new cats, retrieving by location)
namespace CatRescueApi.Services
{
    public class CatService: ICatService
    {
        private readonly ApplicationDbContext _context; // readonly because it should not be changed after initialization
        // inject the database context
        public CatService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get cats by tenant
        public async Task<IEnumerable<CatDto>> GetCatsByTenantAsync(string tenantId)
        {
            var cats = await _context.Cats.Where(x => x.TenantId == tenantId).ToListAsync();
            return cats.Select(MapToDto);
        }
        // Get cat by id
        public async Task<CatDto> GetCatByIdAsync(int id)
        {
            var cat = await _context.Cats.FindAsync(id);
            return cat != null ? MapToDto(cat) : null;
        }
        // Map Cat to CatDto
        private static CatDto MapToDto(Cat cat) =>
            new CatDto
            {
                Id = cat.Id,
                Name = cat.Name,
                BreedId = cat.BreedId,
                Location = cat.Location,
                TenantId = cat.TenantId
            };
    }
}