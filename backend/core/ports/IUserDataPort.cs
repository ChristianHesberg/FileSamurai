using core.models;

namespace core.ports;

public interface IUserDataPort
{
    public void AddUserKeyPair(string userId, byte[] publicKey,  AesGcmEncryptionOutput encryptedPrivateKey);
}