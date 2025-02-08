using FluentValidation;
using CatRescueApi.Models;

namespace CatRescueApi.Validators
{
    public class AdoptionValidator : AbstractValidator<Adoption>
    {
        public AdoptionValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.CatId).GreaterThan(0).WithMessage("Invalid Cat ID.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.");
        }
    }
}