using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
using CatRescueApi.Data;
using Microsoft.EntityFrameworkCore; // needed for ToListAsync

// manage operations for cats (e.g., adding new cats, retrieving by location)
namespace CatRescueApi.Services
{
    public class CatService : ICatService
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
            return cats.Select(CatDto.MapToDto);
        }
        // Get cat by id
        public async Task<CatDto?> GetCatByIdAsync(int id)
        {
            var cat = await _context.Cats.FindAsync(id);
            return cat != null ? CatDto.MapToDto(cat) : null;
        }
    }
}