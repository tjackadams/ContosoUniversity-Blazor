using FluentValidation;

namespace ContosoUniversity.Features.Courses.Validation
{
    public class CreditsValidator : AbstractValidator<int?>
    {
        public CreditsValidator()
        {
            RuleFor(v => v).NotEmpty().InclusiveBetween(0, 5);
        }
    }
}