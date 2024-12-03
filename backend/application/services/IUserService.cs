using application.dtos;
using core.models;

namespace application.services;

public interface IUserService
{
    public string? GetUserPublicKey(string userId);
    public UserRsaPrivateKeyDto? GetUserPrivateKey(string userId);
    public void AddUserRsaKeyPair(UserRsaKeyPairDto keyPair);
}