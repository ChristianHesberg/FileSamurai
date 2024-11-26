using System.Text;
using core.ports;

namespace core.services;

public class UserService(IUserDataPort userDataPort)
{
    private readonly CryptoUtils _utils = new();
    private void GenerateRsaKeyPair(string password, string userId)
    {
        var passwordToBytes = Encoding.UTF8.GetBytes(password);
        var keyPair = _utils.GenerateRsaKeyPair();
        var encryptionOutput = _utils.Encrypt(keyPair.PrivateKey, passwordToBytes);
        
        userDataPort.AddUserKeyPair(userId, keyPair.PublicKey, encryptionOutput);
    }
}