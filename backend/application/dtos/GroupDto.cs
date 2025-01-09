using application.transformers;

namespace application.dtos;

public class GroupDto
{
    public string Id { get; set; }
    private string _name;  
    public string Name  
    {  
        get => _name;  
        set => _name = InputSanitizer.Sanitize(value);  
    } 
    
    public string GroupEmail { get; set; }
}