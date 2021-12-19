using FluentValidation;

namespace ContosoUniversity.Shared.Features.Departments.Validation
{
    public class NameValidator : AbstractValidator<string>
    {
        public NameValidator()
        {
            RuleFor(v => v).NotNull().Length(3, 50);
        }
    }
}