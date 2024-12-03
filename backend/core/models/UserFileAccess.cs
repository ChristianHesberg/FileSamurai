using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class UserFileAccess
{
    //needs to become composite key
    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("File")]
    public string FileId { get; set; }
    public string EncryptedFileKey { get; set; }
    public string Role { get; set; }
}