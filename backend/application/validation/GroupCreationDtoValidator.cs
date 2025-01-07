using application.dtos;
using FluentValidation;

namespace application.validation;

public class GroupCreationDtoValidator : AbstractValidator<GroupCreationDto>
{
    public GroupCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(20);
    }
}