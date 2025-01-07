namespace application.dtos;

public class UserRsaPrivateKeyDto
{
    public string PrivateKey { get; set; }
    public string Nonce { get; set; }
    public string Salt { get; set; }
}