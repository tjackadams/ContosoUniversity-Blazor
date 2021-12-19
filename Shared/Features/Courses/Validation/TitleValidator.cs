using FluentValidation;

namespace ContosoUniversity.Shared.Features.Courses.Validation
{
    public class TitleValidator : AbstractValidator<string>
    {
        public TitleValidator()
        {
            RuleFor(v => v).NotEmpty().Length(3, 50);
        }
    }
}