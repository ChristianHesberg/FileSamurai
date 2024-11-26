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
    
    public AesGcmEncryptionOutput Encrypt(byte[] plaintext, byte[] password,  int saltSize = 16)
    {
        var salt = new byte[saltSize];  
        RandomNumberGenerator.Fill(salt);  
        
        var key = DeriveKeyFromPassword(password, salt);  
        
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
            Tag = tag,
            Salt = salt
        };
    }
    
    public byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] password, byte[] salt)
    {
        var key = DeriveKeyFromPassword(password, salt); 
        
        using var aes = new AesGcm(key, 16);
        var plaintextBytes = new byte[ciphertext.Length];

        aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

        return plaintextBytes;
    }
    
    private static byte[] DeriveKeyFromPassword(byte[] password, byte[] salt, int keySize = 32,  int iterations = 10000)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
        var key = pbkdf2.GetBytes(keySize);  
        return key;
    }
}