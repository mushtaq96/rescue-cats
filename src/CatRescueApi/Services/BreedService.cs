using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CatRescueApi.DTOs;
using CatRescueApi.Models;
using CatRescueApi.Data;
using Microsoft.EntityFrameworkCore; // needed for AsQueryable and ToListAsync

// handle operations related to cat breeds (e.g., fetching, filtering, updating)
namespace CatRescueApi.Services
{
    // Implement the IBreedService interface
    public class BreedService: IBreedService
    {
        // inject the database context
        private readonly ApplicationDbContext _context;
        public BreedService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all breeds
        public async Task<IEnumerable<BreedDto>> GetAllBreedsAsync()
        {
            var breeds = await _context.Breeds.ToListAsync();
            return breeds.Select(MapToDto);
        }
        public async Task<IEnumerable<BreedDto>> FilterBreedsAsync(FilterRequest request)
        {
            // Start with all breeds
            var query = _context.Breeds.AsQueryable();
            // Apply filters
            if (request.IsGoodWithKids.HasValue)
                query = query.Where(x => x.IsGoodWithKids == request.IsGoodWithKids);
            if (request.IsGoodWithDogs.HasValue)
                query = query.Where(x => x.IsGoodWithDogs == request.IsGoodWithDogs);
            // Execute query
            var breeds = await query.ToListAsync();
            return breeds.Select(MapToDto);
        }

        // Map Breed to BreedDto
        private static BreedDto MapToDto(Breed breed) => new BreedDto
        {
            Id = breed.Id,
            Name = breed.Name,
            IsGoodWithKids = breed.IsGoodWithKids,
            IsGoodWithDogs = breed.IsGoodWithDogs,
            Description = breed.Description
        };
    }
}