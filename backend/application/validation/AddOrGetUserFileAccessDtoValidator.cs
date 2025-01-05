﻿using application.dtos;
using core.models;
using FluentValidation;

namespace application.validation;

public class AddOrGetUserFileAccessDtoValidator : AbstractValidator<AddOrGetUserFileAccessDto>
{
    public AddOrGetUserFileAccessDtoValidator()
    {
        RuleFor(x => x.EncryptedFileKey).NotEmpty();
        RuleFor(x => x.EncryptedFileKey).MaximumLength(500);
        RuleFor(x => x.Role).NotEmpty();
        RuleFor(x => x.Role).MaximumLength(30);
        RuleFor(x => x.UserId).MustBeValidGuid();
        RuleFor(x => x.FileId).MustBeValidGuid();
    }
}