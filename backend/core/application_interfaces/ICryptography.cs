using core.models;

namespace core.application_interfaces;

public interface ICryptography
{
    public RsaKeyPair GenerateRsaKeyPair();
    public AesGcmEncryptionOutput Encrypt(byte[] plaintext, byte[] password, int saltSize = 16);
    public byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] password, byte[] salt);
}