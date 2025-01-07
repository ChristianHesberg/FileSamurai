using FluentValidation;

namespace application.validation;

public class GuidValidator : AbstractValidator<string>
{
    public GuidValidator()
    {
        RuleFor(x => x).MustBeValidGuid("id");
    }
}