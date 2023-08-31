using FluentValidation;
using FluentValidation.Results;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;

namespace Mongo.Api.Domain.Entities
{
    public class Restaurant : AbstractValidator<Restaurant>
    {
        public Restaurant(string id, string name, EKitchen kitchen)
        {
            Id = id;
            Name = name;
            Kitchen = kitchen;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public EKitchen Kitchen { get; private set; }

        public Address Address { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public void AssignAddress(Address address)
        {
            Address = address;
        }

        public virtual bool Validate()
        {
            ValidateName();
            ValidationResult = Validate(this);

            ValidateAddress();

            return ValidationResult.IsValid;

        }
        private void ValidateName()
        {
            RuleFor(n => n.Name)
                .NotEmpty().WithMessage("Name cannot be empty")
                .MaximumLength(30).WithMessage("Name can have a maximum of 30 characters");
        }

        private void ValidateAddress()
        {
            if (Address.Validate())
                return;

            foreach (var error in Address.ValidationResult.Errors)
                ValidationResult.Errors.Add(error);


        }

    }
}
