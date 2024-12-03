using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class File
{
    public string Id { get; set; }
    [ForeignKey("Group")]
    public string GroupId { get; set; }

    public string FileContents { get; set; }
    public string Title { get; set; }
}