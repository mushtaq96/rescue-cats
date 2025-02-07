using System.Linq;
using System.Threading.Tasks;
using CatRescueApi.Models;
using CatRescueApi.Services;
using CatRescueApi.DTOs;
using CatRescueApi.Data;
using Xunit;

public class BreedServiceTests : IClassFixture<TestFixture>
{
    private readonly IBreedService _breedService;
    private readonly ApplicationDbContext _context;

    public BreedServiceTests(TestFixture fixture)
    {
        _context = fixture.DbContext;
        _breedService = new BreedService(_context);

        // Seed data
        _context.Breeds.AddRange(new[]
        {
            new Breed { Id = 1, Name = "Siamese", IsGoodWithKids = true, IsGoodWithDogs = false },
            new Breed { Id = 2, Name = "Persian", IsGoodWithKids = false, IsGoodWithDogs = true }
        });
        _context.SaveChanges();

        // clear tracked entities to prevent conflicts with multiple tests
        _context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task GetAllBreedsAsync_ReturnsAllBreeds()
    {
        // Act
        var result = await _breedService.GetAllBreedsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task FilterBreedsAsync_ReturnsFilteredBreeds()
    {
        // Arrange
        var request = new FilterRequest { IsGoodWithKids = true };

        // Act
        var result = await _breedService.FilterBreedsAsync(request);

        // Assert
        Assert.Single(result); // Only one breed should match
        Assert.Equal("Siamese", result.First().Name);
    }
}