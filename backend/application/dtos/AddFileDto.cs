using System.ComponentModel.DataAnnotations;
using application.transformers;

namespace application.dtos;

public class AddFileDto
{
    public string FileContents { get; set; }
    public string Nonce { get; set; }
    private string _title;  
    public string Title  
    {  
        get => _title;  
        set => _title = InputSanitizer.Sanitize(value);  
    } 
    public string GroupId { get; set; }
}