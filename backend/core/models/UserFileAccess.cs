using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class UserFileAccess
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EncryptedFileKey { get; set; }
    public string Role { get; set; }
    
    public User User { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public File File { get; set; }
    [ForeignKey("File")]
    public string FileId { get; set; }

}