using application.dtos;
using FluentValidation;

namespace application.validation;

public class AddUserToGroupDtoValidator : AbstractValidator<AddUserToGroupDto>
{
    public AddUserToGroupDtoValidator()
    {
        RuleFor(x => x.UserEmail).EmailAddress();
        RuleFor(x => x.UserEmail).MaximumLength(40);
        RuleFor(x => x.GroupId).NotEmpty();
        RuleFor(x => x.GroupId).MaximumLength(36);
    }
}