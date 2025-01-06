namespace application.errors;
using System;  
using System.Collections.Generic;  
  
public class CustomValidationException : Exception  
{  
    public IList<string> Errors { get; private set; }  
  
    public CustomValidationException()  
    {  
        Errors = new List<string>();  
    }  
  
    public CustomValidationException(string message)  
        : base(message)  
    {  
        Errors = new List<string>();  
    }  
  
    public CustomValidationException(IList<string> errors)  
        : base("Validation failed with errors: ")
    {
        Errors = errors;
    }  
 
}  