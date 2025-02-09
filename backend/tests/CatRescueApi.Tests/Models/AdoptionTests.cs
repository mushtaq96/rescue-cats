using CatRescueApi.Models;
using FluentValidation.TestHelper;
using CatRescueApi.Validators;
using Xunit;


public class AdoptionTests
{
    private readonly AdoptionValidator _validator = new AdoptionValidator();

    [Fact]
    public void Validation_Fails_When_UserId_IsMissing()
    {
        var model = new Adoption { UserId = null! }; // Initialize required fields
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validation_Succeeds_When_Valid()
    {
        var model = new Adoption
        {
            UserId = "User123",
            CatId = 1,
            Status = "pending"
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }
}