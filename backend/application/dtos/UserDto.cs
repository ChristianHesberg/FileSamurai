using application.transformers;

namespace application.dtos;

public class UserDto
{
    public string Id { get; set; }
    
    private string _email;

    public string Email
    {
        get => _email;
        set => _email = InputSanitizer.Sanitize(value);
    }
}