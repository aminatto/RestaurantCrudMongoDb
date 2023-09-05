using FluentValidation;
using FluentValidation.Results;

namespace Mongo.Api.Domain.ValueObjects
{
    public class Address : AbstractValidator<Address>
    {
        public Address(string publicPlace, string number, string city, string state, string postalCode)
        {
            PublicPlace = publicPlace;
            Number = number;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string PublicPlace { get; private set; }

        public string Number { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public string PostalCode { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public virtual bool Validate()
        {
            ValidatePublicPlace();
            ValidateCity();
            ValidateState();
            ValidatePostalCode();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
        private void ValidatePublicPlace()
        {
            RuleFor(p => p.PublicPlace)
                .NotEmpty().WithMessage("Public Place cannot be empty.")
                .MaximumLength(50).WithMessage("Public Place can have a maximum of 50 characters.");
        }
        private void ValidateCity()
        {
            RuleFor(c => c.City)
                .NotEmpty().WithMessage("City cannot be empty")
                .MaximumLength(100).WithMessage("City can have a maximum of 100 characters");
        }

        private void ValidateState()
        {
            RuleFor(s => s.State)
                .NotEmpty().WithMessage("State cannot be empty")
                .Length(2).WithMessage("State must have of 2 characters");
        }

        private void ValidatePostalCode()
        {
            RuleFor(pc => pc.PostalCode)
                .NotEmpty().WithMessage("Postal Code cannot be empty")
                .Length(8).WithMessage("Postal Code must have 8 characters");

        }

    }
}
