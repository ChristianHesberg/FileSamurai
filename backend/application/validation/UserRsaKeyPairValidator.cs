using application.dtos;
using FluentValidation;

namespace application.validation;

public class UserRsaKeyPairValidator : AbstractValidator<UserRsaKeyPairDto>
{
    public UserRsaKeyPairValidator()
    {
        RuleFor(x => x.UserId).MustBeValidGuid("GroupId");
        RuleFor(x => x.Nonce).NotEmpty();
        RuleFor(x => x.Nonce).MaximumLength(30);
        RuleFor(x => x.Salt).NotEmpty();
        RuleFor(x => x.Salt).MaximumLength(100);        
        RuleFor(x => x.PublicKey).NotEmpty();
        RuleFor(x => x.PublicKey).MaximumLength(500);        
        RuleFor(x => x.PrivateKey).NotEmpty();
        RuleFor(x => x.PrivateKey).MaximumLength(500);
    }
}