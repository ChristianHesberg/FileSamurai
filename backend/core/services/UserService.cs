using System.Text;
using core.models;
using core.ports;

namespace core.services;

public class UserService(IUserDataPort userDataPort) : IUserService
{
    private readonly CryptoUtils _utils = new();
    public UserRsaKeyPair GenerateRsaKeyPair(string password, string userId)
    {
        var passwordToBytes = Encoding.UTF8.GetBytes(password);
        var keyPair = _utils.GenerateRsaKeyPair();
        var encryptionOutput = _utils.Encrypt(keyPair.PrivateKey, passwordToBytes);
        var userKeyPair = new UserRsaKeyPair()
        {
            Id = userId,
            PublicKey = keyPair.PublicKey,
            PrivateKey = encryptionOutput.CipherText,
            Nonce = encryptionOutput.Nonce,
            Tag = encryptionOutput.Tag
        };
        
        return userDataPort.AddUserKeyPair(userKeyPair);
    }

    public UserRsaKeyPair? GetUserRsaKeyPair(string userId)
    {
        return userDataPort.GetUserRsaKeyPair(userId);
    } 
}