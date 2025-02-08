using CatRescueApi.Models;
using CatRescueApi.Services;
using CatRescueApi.DTOs;
using Xunit;

public class BreedServiceTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly BreedService _breedService;

    public BreedServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        var context = _fixture.CreateContext();
        _breedService = new BreedService(context);

        // Seed data
        context.Breeds.AddRange(new[]
        {
            new Breed { Id = 1, Name = "Siamese", IsGoodWithKids = true, IsGoodWithDogs = false },
            new Breed { Id = 2, Name = "Persian", IsGoodWithKids = false, IsGoodWithDogs = true }
        });
        context.SaveChanges();

        // clear tracked entities to prevent conflicts with multiple tests
        context.ChangeTracker.Clear();
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