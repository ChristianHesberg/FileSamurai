using core.models;

namespace application.ports;

public interface IUserPort
{
    public UserRsaKeyPair AddUserKeyPair(UserRsaKeyPair userRsaKeyPair);
    public UserRsaKeyPair? GetUserRsaKeyPair(string userId);
    public string? GetUserPublicKey(string userId);
}