using FluentValidation;
using CatRescueApi.Models;

namespace CatRescueApi.Validators
{
    public class CatValidator : AbstractValidator<Cat>
    {
        public CatValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.BreedId).GreaterThan(0).WithMessage("Breed ID is required.");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant ID is required.");
            RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null).WithMessage("Description must be less than 500 characters.");
        }
    }
}