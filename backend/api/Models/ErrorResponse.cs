namespace api.Models;

public class ErrorResponse  
{  
    public string Message { get; set; }  
    public IList<string>? Errors { get; set; }  
}