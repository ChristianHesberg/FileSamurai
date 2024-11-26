using core.models;

namespace core.ports;

public interface IUserPort
{
    public UserRsaKeyPair AddUserKeyPair(UserRsaKeyPair userRsaKeyPair);
    public UserRsaKeyPair? GetUserRsaKeyPair(string userId);
}