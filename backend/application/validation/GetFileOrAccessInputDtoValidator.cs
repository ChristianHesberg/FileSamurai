using application.dtos;
using FluentValidation;

namespace application.validation;

public class GetFileOrAccessInputDtoValidator : AbstractValidator<GetFileOrAccessInputDto>
{
    public GetFileOrAccessInputDtoValidator()
    {
        RuleFor(x => x.UserId).MustBeValidGuid();
        RuleFor(x => x.FileId).MustBeValidGuid();
    }
}