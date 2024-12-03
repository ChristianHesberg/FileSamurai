using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class File
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FileContents { get; set; }
    public string Title { get; set; }
    
    public Group Group { get; set; }
    [ForeignKey("Group")]
    public string GroupId { get; set; }
    public List<UserFileAccess> UserFileAccesses { get; set; }
}