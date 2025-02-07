using System.Linq;
using System.Threading.Tasks;
using CatRescueApi.Models;
using CatRescueApi.Services;
using CatRescueApi.Data;
using Xunit;

public class CatServiceTests : IClassFixture<TestFixture>
{
    private readonly ICatService _catService;
    private readonly ApplicationDbContext _context;

    public CatServiceTests(TestFixture fixture)
    {
        _context = fixture.DbContext;
        _catService = new CatService(_context);

        // Seed data
        _context.Cats.AddRange(new[]
        {
            new Cat { Id = 1, Name = "Whiskers", BreedId = 1, Location = "Berlin", TenantId = "TenantA" },
            new Cat { Id = 2, Name = "Mittens", BreedId = 2, Location = "Hamburg", TenantId = "TenantB" }
        });
        _context.SaveChanges();
        
        // clear tracked entities to prevent conflicts with multiple tests
        _context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task GetCatsByTenantAsync_ReturnsCatsForTenant()
    {
        // Act
        var result = await _catService.GetCatsByTenantAsync("TenantA");

        // Assert
        Assert.Single(result); // Only one cat belongs to TenantA
        Assert.Equal("Whiskers", result.First().Name);
    }

    [Fact]
    public async Task GetCatByIdAsync_ReturnsCorrectCat()
    {
        // Act
        var result = await _catService.GetCatByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Whiskers", result.Name);
    }
}