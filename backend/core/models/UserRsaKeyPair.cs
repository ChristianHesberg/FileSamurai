namespace core.models;

public class UserRsaKeyPair
{
    public string Id { get; set; }
    public byte[] PublicKey { get; set; }
    public byte[] PrivateKey { get; set; }
    public byte[] Nonce { get; set; }
    public byte[] Tag { get; set; }
}