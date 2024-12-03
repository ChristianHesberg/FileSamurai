using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class User
{
    [Key]
    public string Id { get; set; }
    
    public UserRsaKeyPair UserRsaKeyPair { get; set; }
    public List<UserFileAccess>? UserFileAccesses { get; set; }
    public List<Group>? Groups { get; set; }
}