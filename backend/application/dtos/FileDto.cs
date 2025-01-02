﻿using application.transformers;

namespace application.dtos;

public class FileDto
{
    public string Id { get; set; }
    public string FileContents { get; set; }
    public string Nonce { get; set; }
    public string Tag { get; set; }
    private string _title;  
    public string Title  
    {  
        get => _title;  
        set => _title = InputSanitizer.Sanitize(value);  
    } 
}