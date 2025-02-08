using CatRescueApi.Controllers;
using CatRescueApi.Services;
using CatRescueApi.DTOs;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

public class CatsControllerTests
{
    private readonly Mock<ICatService> _mockService;
    private readonly CatsController _controller;

    public CatsControllerTests()
    {
        _mockService = new Mock<ICatService>();
        _controller = new CatsController(_mockService.Object);
    }

    [Fact]
    public async Task GetCats_ReturnsCatsForTenant()
    {
        var tenantId = "TenantA";
        var cats = new List<CatDto> { new() { Name = "Whiskers", Location = "Home", TenantId = tenantId } };
        _mockService.Setup(s => s.GetCatsByTenantAsync(tenantId)).ReturnsAsync(cats);

        var result = await _controller.GetCats(tenantId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var catsResult = Assert.IsType<List<CatDto>>(okResult.Value);
        Assert.Single(catsResult);
        Assert.Equal("Whiskers", catsResult.First().Name);
    }

    [Fact]
    public async Task GetCatById_ReturnsNotFoundIfCatDoesNotExist()
    {
        var catId = 1;
        _mockService.Setup(s => s.GetCatByIdAsync(catId)).ReturnsAsync((CatDto)null);

        var result = await _controller.GetCatById(catId);

        Assert.IsType<NotFoundResult>(result);
    }
}