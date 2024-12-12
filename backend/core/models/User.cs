using System.ComponentModel.DataAnnotations;

namespace core.models;

public class User
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; }
    
    public UserRsaKeyPair UserRsaKeyPair { get; set; }
    public List<UserFileAccess>? UserFileAccesses { get; set; }
    public List<Group>? Groups { get; set; }
}