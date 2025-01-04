using application.dtos;
using FluentValidation;

namespace application.validation;

public class FileDtoValidator : AbstractValidator<FileDto>
{
    public FileDtoValidator()
    {
        RuleFor(x => x.Id).MustBeValidGuid();
        RuleFor(x => x.Nonce).NotEmpty();
        RuleFor(x => x.Nonce).MaximumLength(30);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(50);
    }
}