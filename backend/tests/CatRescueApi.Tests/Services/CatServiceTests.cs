using CatRescueApi.Models;
using CatRescueApi.Services;
using Xunit;

public class CatServiceTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly CatService _catService;

    public CatServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        var context = _fixture.CreateContext();
        _catService = new CatService(context);

        // Seed data
        context.Cats.AddRange(
        [
            new Cat { Id = 1, Name = "Whiskers", BreedId = 1, Location = "Berlin", TenantId = "TenantA" },
            new Cat { Id = 2, Name = "Mittens", BreedId = 2, Location = "Hamburg", TenantId = "TenantB" }
        ]);
        context.SaveChanges();

        // clear tracked entities to prevent conflicts with multiple tests
        context.ChangeTracker.Clear();
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