﻿using application.dtos;
using FluentValidation;

namespace application.validation;

public class AddFileDtoValidator : AbstractValidator<AddFileDto>
{
    public AddFileDtoValidator()
    {
        RuleFor(x => x.FileContents).NotEmpty();
        RuleFor(x => x.FileContents).MaximumLength(10_500_000);
        RuleFor(x => x.Nonce).NotEmpty();
        RuleFor(x => x.Nonce).MaximumLength(30);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(50);
        RuleFor(x => x.GroupId).MustBeValidGuid("GroupId");
    }
}