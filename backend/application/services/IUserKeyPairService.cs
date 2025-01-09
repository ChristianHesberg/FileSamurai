using application.dtos;
using core.models;

namespace application.services;

public interface IUserKeyPairService
{
    public string GetUserPublicKey(string userId);
    public List<string> GetUserPublicKeys(string[] idList);
    public UserRsaPrivateKeyDto GetUserPrivateKey(string userId);
    public void AddUserRsaKeyPair(UserRsaKeyPairDto keyPair);
}