using application.dtos;
using core.models;

namespace application.services;

public interface IUserKeyPairService
{
    public string GetUserPublicKey(string userId);
    public UserRsaPrivateKeyDto GetUserPrivateKey(string userId);
    public void AddUserRsaKeyPair(UserRsaKeyPairDto keyPair);
}