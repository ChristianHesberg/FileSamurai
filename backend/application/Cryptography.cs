using System.Security.Cryptography;
using core.application_interfaces;
using core.models;

namespace application;

public class Cryptography : ICryptography
{
    public RsaKeyPair GenerateRsaKeyPair()
    {
        using var rsa = RSA.Create();
        rsa.KeySize = 2048;
        
        var publicKey = rsa.ExportRSAPublicKey();  
        var privateKey = rsa.ExportRSAPrivateKey();
        
        return new RsaKeyPair()
        {
            PrivateKey = privateKey,
            PublicKey = publicKey
        };
    }
    
    public AesGcmEncryptionOutput Encrypt(byte[] plaintext, byte[] key)
    {
        using var aes = new AesGcm(key, 16);
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);

        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        var ciphertext = new byte[plaintext.Length];

        aes.Encrypt(nonce, plaintext, ciphertext, tag);

        return new AesGcmEncryptionOutput()
        {
            CipherText = ciphertext,
            Nonce = nonce,
            Tag = tag
        };
    }
    
    public byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] key)
    {
        using var aes = new AesGcm(key, 16);
        var plaintextBytes = new byte[ciphertext.Length];

        aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

        return plaintextBytes;
    }
}