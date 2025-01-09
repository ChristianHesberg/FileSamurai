using core.models;

namespace application.dtos;

public class FileAccessDto
{
    public string UserId { get; set; }
    public string FileId { get; set; }
    public Roles Role { get; set; }
}