namespace application.dtos;

public class UserRsaKeyPairDto
{
    public string UserId { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public string Nonce { get; set; }
    public string Salt { get; set; } 
}