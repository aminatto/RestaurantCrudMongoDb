using FluentValidation;
using FluentValidation.Results;

namespace RestaurantCrudMongoDb.Domain.ValueObjects
{
    public class Rating : AbstractValidator<Rating>
    {
        public int Stars { get; private set; }

        public string Comment { get; private set; }

        public ValidationResult ValidationResult { get; private set; }

        public Rating(int stars, string comment)
        {
            Stars = stars;
            Comment = comment;
        }

        public Rating() { }

        public virtual bool Validate()
        {
            ValidateStars();
            ValidateComment();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        private void ValidateComment()
        {
            RuleFor(c => c.Comment)
                .NotEmpty().WithMessage("Comment cannot be empty.")
                .MaximumLength(100).WithMessage("Comment can have a maximum of 100 characters");
        }

        private void ValidateStars()
        {
            RuleFor(s => s.Stars)
                .GreaterThan(0).WithMessage("Stars must be greater than 0")
                .LessThanOrEqualTo(5).WithMessage("Stars must be lesser than or equal to 5");
        }


    }
}
