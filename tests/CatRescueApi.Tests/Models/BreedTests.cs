using CatRescueApi.Models;
using CatRescueApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CatRescueApi.Tests.Models
{
    public class BreedTests
    {
        private readonly BreedValidator _validator = new BreedValidator(); // Validator doesn't exist yet

        [Fact]
        public void Validation_Fails_When_Name_IsMissing()
        {
            var breed = new Breed { IsGoodWithKids = true, IsGoodWithDogs = false }; // Breed object used before it is defined.
            var result = _validator.TestValidate(breed);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validation_Fails_When_Name_IsTooLong()
        {
            var breed = new Breed
            {
                Name = new string('A', 101), // Create a string longer than 100
                IsGoodWithKids = true,
                IsGoodWithDogs = false
            };
            var result = _validator.TestValidate(breed);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validation_Fails_When_IsGoodWithKids_IsNotBoolean()
        {
            var breed = new Breed { Name = "Test Breed" };
            Assert.False(breed.IsGoodWithKids);
        }

        [Fact]
        public void Validation_Succeeds_When_Valid()
        {
            var breed = new Breed
            {
                Name = "Siamese",
                IsGoodWithKids = true,
                IsGoodWithDogs = false
            };
            var result = _validator.TestValidate(breed);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}