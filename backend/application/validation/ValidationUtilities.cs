using application.errors;
using FluentValidation.Results;

namespace application.validation;

public class ValidationUtilities
{
    public static void ThrowIfInvalid(ValidationResult validationResult)  
    {  
        if (!validationResult.IsValid)  
        {  
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();  
            throw new CustomValidationException(errorMessages);  
        }  
    }  
}