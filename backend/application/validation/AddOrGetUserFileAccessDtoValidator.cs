using application.dtos;
using core.models;
using FluentValidation;

namespace application.validation;

public class AddOrGetUserFileAccessDtoValidator : AbstractValidator<AddOrGetUserFileAccessDto>
{
    public AddOrGetUserFileAccessDtoValidator()
    {
        RuleFor(x => x.EncryptedFileKey).NotEmpty();
        RuleFor(x => x.EncryptedFileKey).MaximumLength(500);
        RuleFor(x => x.Role).IsInEnum();
        RuleFor(x => x.UserId).MustBeValidGuid("UserId");
        RuleFor(x => x.FileId).MustBeValidGuid("FileId");
    }
}