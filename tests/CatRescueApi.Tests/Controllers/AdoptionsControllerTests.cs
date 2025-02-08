using CatRescueApi.Controllers;
using CatRescueApi.Services;
using CatRescueApi.Models;
using CatRescueApi.DTOs;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

public class AdoptionsControllerTests
{
    private readonly Mock<IAdoptionService> _mockService;
    private readonly AdoptionsController _controller;

    public AdoptionsControllerTests()
    {
        _mockService = new Mock<IAdoptionService>();
        _controller = new AdoptionsController(_mockService.Object);
    }

    [Fact]
    public async Task SubmitAdoption_ReturnsCreatedAdoption()
    {
        var request = new AdoptionRequest
        {
            UserId = "User123",
            CatId = 1,
            Email = "user@example.com",
            Address = "123 Street"
        };

        var adoption = new AdoptionDto { Id = 1, UserId = "User123", CatId = 1, Status = "pending" };
        _mockService.Setup(s => s.SubmitAdoptionAsync(request)).ReturnsAsync(adoption);

        var result = await _controller.SubmitAdoption(request);

        var actionResult = Assert.IsType<ActionResult<AdoptionDto>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var adoptionResult = Assert.IsType<AdoptionDto>(createdResult.Value);
        Assert.Equal("pending", adoptionResult.Status);
    }

    [Fact]
    public async Task GetAdoption_ReturnsNotFoundIfAdoptionDoesNotExist()
    {
        var id = 1;
        _mockService.Setup(s => s.GetAdoptionByIdAsync(id)).ReturnsAsync((AdoptionDto)null);

        var result = await _controller.GetAdoption(id);

        var actionResult = Assert.IsType<ActionResult<AdoptionDto>>(result);
        var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
    }
}