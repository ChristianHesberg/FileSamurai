using application.dtos;
using application.ports;
using core.models;

namespace application.services;

public class UserService(IUserPort userPort) : IUserService
{
    public string? GetUserPublicKey(string userId)
    {
        return userPort.GetUserPublicKey(userId);
    }

    public UserRsaPrivateKeyDto? GetUserPrivateKey(string userId)
    {
        var keyPair = userPort.GetUserRsaKeyPair(userId);
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
        userPort.AddUserKeyPair(converted);
    }
}