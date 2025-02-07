using System.Threading.Tasks;
using CatRescueApi.Models;
using CatRescueApi.Services;
using CatRescueApi.Data;
using Xunit;

public class AdoptionServiceTests : IClassFixture<TestFixture>
{
    private readonly IAdoptionService _adoptionService;
    private readonly ApplicationDbContext _context;

    public AdoptionServiceTests(TestFixture fixture)
    {
        _context = fixture.DbContext;
        _adoptionService = new AdoptionService(_context);

        // Add test data
        _context.Cats.Add(new Cat { Id = 1, Name = "Whiskers", BreedId = 1, Location = "Berlin", TenantId = "TenantA" });
        _context.SaveChanges();

        // clear tracked entities to prevent conflicts with multiple tests
        _context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task SubmitAdoptionAsync_CreatesNewAdoption()
    {
        // Arrange
        var request = new AdoptionRequest { UserId = "User123", CatId = 1 , Email = "mushtaq@gmail.com", Address ="123 Cat Street"};

        // Act
        var result = await _adoptionService.SubmitAdoptionAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("pending", result.Status);
        Assert.Equal(1, result.CatId);
    }

    [Fact]
    public async Task GetAdoptionByIdAsync_ReturnsExistingAdoption()
    {
        // Arrange
        var adoption = new Adoption { Id = 1, UserId = "User123", CatId = 1, Status = "pending" };
        _context.Adoptions.Add(adoption);
        _context.SaveChanges();

        // Act
        var result = await _adoptionService.GetAdoptionByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("pending", result.Status);
    }
}