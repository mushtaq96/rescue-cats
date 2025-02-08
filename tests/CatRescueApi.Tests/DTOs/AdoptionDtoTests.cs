using CatRescueApi.DTOs;
using CatRescueApi.Models;
using Xunit;

public class AdoptionDtoTests
{
    [Fact]
    public void MapToDto_MapsPropertiesCorrectly()
    {
        var adoption = new Adoption
        {
            Id = 1,
            UserId = "User123",
            CatId = 1,
            Status = "pending"
        };

        var dto = AdoptionDto.MapToDto(adoption);

        Assert.Equal(adoption.Id, dto.Id);
        Assert.Equal(adoption.UserId, dto.UserId);
        Assert.Equal(adoption.CatId, dto.CatId);
        Assert.Equal(adoption.Status, dto.Status);
    }
}