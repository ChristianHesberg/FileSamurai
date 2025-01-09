using core.models;

namespace application.ports;

public interface IUserKeyPairPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair);
    public UserRsaKeyPair GetUserRsaKeyPair(string userId);
    public string GetUserPublicKey(string userId);
    public List<string> GetPublicKeys(string[] idList);
}