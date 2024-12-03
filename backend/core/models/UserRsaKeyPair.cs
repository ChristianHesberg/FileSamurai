using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class UserRsaKeyPair
{
    [Key]
    public string Id { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public string Nonce { get; set; }
    public string Tag { get; set; }
    public string Salt { get; set; }
    
    public User User { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
}