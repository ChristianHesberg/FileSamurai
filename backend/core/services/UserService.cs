using System.Text;
using core.models;
using core.ports;

namespace core.services;

public class UserService(IUserPort userPort) : IUserService
{
    public UserRsaKeyPair AddUserRsaKeyPair(UserRsaKeyPair keyPair)
    {
        return userPort.AddUserKeyPair(keyPair);
    }
    public UserRsaKeyPair? GetUserRsaKeyPair(string userId)
    {
        return userPort.GetUserRsaKeyPair(userId);
    } 
}