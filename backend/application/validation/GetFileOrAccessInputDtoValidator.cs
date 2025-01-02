using application.dtos;
using FluentValidation;

namespace application.validation;

public class GetFileOrAccessInputDtoValidator : AbstractValidator<GetFileOrAccessInputDto>
{
    public GetFileOrAccessInputDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.UserId).MaximumLength(36);
        RuleFor(x => x.FileId).NotEmpty();
        RuleFor(x => x.FileId).MaximumLength(36);
    }
}