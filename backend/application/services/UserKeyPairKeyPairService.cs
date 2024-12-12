using application.dtos;
using application.ports;
using core.models;

namespace application.services;

public class UserKeyPairKeyPairService(IUserKeyPairPort userKeyPairPort) : IUserKeyPairService
{
    public string? GetUserPublicKey(string userId)
    {
        return userKeyPairPort.GetUserPublicKey(userId);
    }

    public UserRsaPrivateKeyDto? GetUserPrivateKey(string userId)
    {
        var keyPair = userKeyPairPort.GetUserRsaKeyPair(userId);
        if (keyPair == null) return null;
        return new UserRsaPrivateKeyDto()
        {
            PrivateKey = keyPair.PrivateKey,
            Nonce = keyPair.Nonce,
            Tag = keyPair.Tag,
            Salt = keyPair.Salt
        };
    }

    public void AddUserRsaKeyPair(UserRsaKeyPairDto keyPair)
    {
        var converted = new UserRsaKeyPair()
        {
            UserId = keyPair.UserId,
            PublicKey = keyPair.PublicKey,
            PrivateKey = keyPair.PrivateKey,
            Nonce = keyPair.Nonce,
            Tag = keyPair.Tag,
            Salt = keyPair.Salt
        };
        userKeyPairPort.AddUserKeyPair(converted);
    }
}