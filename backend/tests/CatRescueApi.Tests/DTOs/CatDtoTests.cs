using CatRescueApi.DTOs;
using CatRescueApi.Models;
using CatRescueApi.Services;
using Xunit;

public class CatDtoTests
{
    [Fact]
    public void MapToDto_MapsPropertiesCorrectly()
    {
        var cat = new Cat
        {
            Id = 1,
            Name = "Whiskers",
            BreedId = 1,
            Location = "Berlin",
            TenantId = "TenantA"
        };

        var dto = CatDto.MapToDto(cat);

        Assert.Equal(cat.Id, dto.Id);
        Assert.Equal(cat.Name, dto.Name);
        Assert.Equal(cat.BreedId, dto.BreedId);
        Assert.Equal(cat.Location, dto.Location);
        Assert.Equal(cat.TenantId, dto.TenantId);
    }
}