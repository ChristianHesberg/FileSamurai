using core.models;

namespace core.application_interfaces;

public interface ICryptography
{
    public RsaKeyPair GenerateRsaKeyPair();
    public AesGcmEncryptionOutput Encrypt(byte[] plaintext, byte[] key);
    public byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] key);
}