using core.models;

namespace core.services;

public interface IUserService
{
    public UserRsaKeyPair? GetUserRsaKeyPair(string userId);
    public UserRsaKeyPair AddUserRsaKeyPair(UserRsaKeyPair keyPair);
}