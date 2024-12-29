namespace application.errors;
using System;  
using System.Collections.Generic;  
  
public class CustomValidationException : Exception  
{  
    public IList<string> ValidationErrors { get; private set; }  
  
    public CustomValidationException()  
    {  
        ValidationErrors = new List<string>();  
    }  
  
    public CustomValidationException(string message)  
        : base(message)  
    {  
        ValidationErrors = new List<string>();  
    }  
  
    public CustomValidationException(IList<string> validationErrors)  
        : base("Validation failed with errors: ")
    {
        ValidationErrors = validationErrors;
    }  
 
}  