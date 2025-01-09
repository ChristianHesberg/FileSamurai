using application.transformers;
using core.models;

namespace application.dtos;

public class FileOptionDto
{
    public string Id { get; set; }
    private string _name;
    public string GroupId { get; set; }

    public string Name
    {
        get => _name;
        set => _name = InputSanitizer.Sanitize(value);
    }
    public Roles Role { get; set; }
}