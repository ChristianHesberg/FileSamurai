using System.Text;

namespace core;

public class UserService
{
    private readonly CryptoUtils utils;
    public UserService()
    {
        utils = new CryptoUtils();
    }

    public void GenerateRsaKeyPair(string password)
    {
        var passwordToBytes = Encoding.UTF8.GetBytes(password);
        var keyPair = utils.GenerateRsaKeyPair();
        var encryptionOUtput = utils.Encrypt(keyPair.PrivateKey, passwordToBytes);
        
        
    }
}