using application.errors;
using FluentValidation;
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

    public static IValidator<string> GetValidator<T>(IEnumerable<IValidator<string>> validators)
    {
        foreach (var validator in validators)
        {
            if (validator is T) return validator;
        }

        throw new Exception("Validator does not exist of given type");
    }
}