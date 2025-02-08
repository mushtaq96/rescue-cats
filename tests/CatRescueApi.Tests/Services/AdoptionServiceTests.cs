using CatRescueApi.Models;
using CatRescueApi.Services;
using Xunit;

public class AdoptionServiceTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    public AdoptionServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SubmitAdoptionAsync_CreatesNewAdoption()
    {
        // Arrange
        var context = _fixture.CreateContext();
        var adoptionService = new AdoptionService(context);

        var cat = new Cat
        {
            Id = 1,
            Name = "Whiskers",
            BreedId = 1,
            Location = "Berlin",
            TenantId = "TenantA"
        };
        context.Cats.Add(cat);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var request = new AdoptionRequest
        {
            UserId = "User123",
            CatId = 1,
            Email = "mushtaq@gmail.com",
            Address = "123 Cat Street"
        };

        // Act
        var result = await adoptionService.SubmitAdoptionAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("pending", result.Status);
        Assert.Equal(1, result.CatId);

        // Dispose the context after the test
        context.Dispose();
    }

    [Fact]
    public async Task GetAdoptionByIdAsync_ReturnsExistingAdoption()
    {
        // Arrange
        var context = _fixture.CreateContext();
        var adoptionService = new AdoptionService(context);

        var adoption = new Adoption { Id = 1, UserId = "User123", CatId = 1, Status = "pending" };
        context.Adoptions.Add(adoption);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        // Act
        var result = await adoptionService.GetAdoptionByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("pending", result.Status);

        // Dispose the context after the test
        context.Dispose();
    }
}