namespace core.models;

public class UserRsaKeyPair
{
    public string Id { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public string Nonce { get; set; }
    public string Tag { get; set; }
    public string Salt { get; set; }
    public User User { get; set; }
}