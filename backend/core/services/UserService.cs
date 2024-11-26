using System.Text;
using core.application_interfaces;
using core.models;
using core.ports;

namespace core.services;

public class UserService(IUserPort userPort, ICryptography cryptography) : IUserService
{
    public UserRsaKeyPair GenerateRsaKeyPair(string password, string userId)
    {
        var passwordToBytes = Encoding.UTF8.GetBytes(password);
        var keyPair = cryptography.GenerateRsaKeyPair();
        var encryptionOutput = cryptography.Encrypt(keyPair.PrivateKey, passwordToBytes);
        var userKeyPair = new UserRsaKeyPair()
        {
            Id = userId,
            PublicKey = keyPair.PublicKey,
            PrivateKey = encryptionOutput.CipherText,
            Nonce = encryptionOutput.Nonce,
            Tag = encryptionOutput.Tag,
            Salt = encryptionOutput.Salt
        };
        
        return userPort.AddUserKeyPair(userKeyPair);
    }

    public UserRsaKeyPair? GetUserRsaKeyPair(string userId)
    {
        return userPort.GetUserRsaKeyPair(userId);
    } 
}