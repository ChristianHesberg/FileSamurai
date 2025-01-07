using application.dtos;
using FluentValidation;

namespace application.validation;

public class UserCreationDtoValidator : AbstractValidator<UserCreationDto>
{
    public UserCreationDtoValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}