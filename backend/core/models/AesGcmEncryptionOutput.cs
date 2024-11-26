namespace core.models;

public class AesGcmEncryptionOutput
{
    public byte[] CipherText { get; set; }
    public byte[] Nonce { get; set; }
    public byte[] Tag { get; set; }
}