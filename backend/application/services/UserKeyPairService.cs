using application.dtos;
using application.ports;
using application.validation;
using core.models;
using FluentValidation;

namespace application.services;

public class UserKeyPairService(
    IUserKeyPairPort userKeyPairPort,
    IValidator<UserRsaKeyPairDto> rsaKeyPairValidator,
    IEnumerable<IValidator<string>> stringValidators 
        ) : IUserKeyPairService
{
    public string GetUserPublicKey(string userId)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(userId);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
        return userKeyPairPort.GetUserPublicKey(userId);
    }
    
    public List<RsaPublicKeyWithId> GetUserPublicKeys(string[] idList)
    {
        foreach (var id in idList)
        {
            var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
            var validationResult = guidValidator.Validate(id);  
            ValidationUtilities.ThrowIfInvalid(validationResult); 
        }
        
        return userKeyPairPort.GetPublicKeys(idList);
    }

    public UserRsaPrivateKeyDto GetUserPrivateKey(string userId)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(userId);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
        var keyPair = userKeyPairPort.GetUserRsaKeyPair(userId);

        return new UserRsaPrivateKeyDto()
        {
            PrivateKey = keyPair.PrivateKey,
            Nonce = keyPair.Nonce,
            Salt = keyPair.Salt
        };
    }

    public void AddUserRsaKeyPair(UserRsaKeyPairDto keyPair)
    {
        var validationResult = rsaKeyPairValidator.Validate(keyPair);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
        var converted = new UserRsaKeyPair()
        {
            UserId = keyPair.UserId,
            PublicKey = keyPair.PublicKey,
            PrivateKey = keyPair.PrivateKey,
            Nonce = keyPair.Nonce,
            Salt = keyPair.Salt
        };
        userKeyPairPort.AddUserKeyPair(converted);
    }
}