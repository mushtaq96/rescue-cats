using FluentValidation;
using CatRescueApi.Models;

namespace CatRescueApi.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                                 .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Details.FirstName).NotEmpty().WithMessage("First name is required.")
                                             .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");
            RuleFor(x => x.Details.LastName).NotEmpty().WithMessage("Last name is required.")
                                            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");
            RuleFor(x => x.Location.Street).NotEmpty().WithMessage("Street address is required.");
            RuleFor(x => x.Location.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Location.State).NotEmpty().WithMessage("State is required.");
            RuleFor(x => x.Location.PostalCode).NotEmpty().WithMessage("Postal code is required.");
            RuleFor(x => x.Location.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
            RuleFor(x => x.Location.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}