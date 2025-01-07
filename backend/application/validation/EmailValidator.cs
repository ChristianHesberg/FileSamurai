using FluentValidation;

namespace application.validation;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x).EmailAddress();
    }
}