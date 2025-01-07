using application.dtos;
using application.ports;
using core.models;

namespace application.services;

public class UserKeyPairService(IUserKeyPairPort userKeyPairPort) : IUserKeyPairService
{
    public string GetUserPublicKey(string userId)
    {
        return userKeyPairPort.GetUserPublicKey(userId);
    }

    public UserRsaPrivateKeyDto GetUserPrivateKey(string userId)
    {
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