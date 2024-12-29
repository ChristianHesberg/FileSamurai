using application.transformers;

namespace application.dtos;

public class GroupCreationDto
{
    private string _name;  
    public string Name  
    {  
        get => _name;  
        set => _name = InputSanitizer.Sanitize(value);  
    } 
}