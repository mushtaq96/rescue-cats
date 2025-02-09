using CatRescueApi.Models;
using CatRescueApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CatRescueApi.Tests.Models
{
    public class CatTests
    {
        private readonly CatValidator _validator = new CatValidator();

        [Fact]
        public void Validation_Fails_When_Name_IsMissing()
        {
            var model = new Cat
            {
                Name = null,
                Location = "Berlin", // Initialize required property
                TenantId = "TenantA" // Initialize required property
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validation_Succeeds_When_Valid()
        {
            var model = new Cat
            {
                Name = "Whiskers",
                BreedId = 1,
                Location = "Berlin",
                TenantId = "TenantA"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}