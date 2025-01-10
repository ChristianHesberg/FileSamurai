using application.transformers;

namespace application.dtos;

public class PostFileResultDto
{
    public string Id { get; set; }
    private string _title;

    public string Title
    {
        get => _title;
        set => _title = InputSanitizer.Sanitize(value);
    }
}