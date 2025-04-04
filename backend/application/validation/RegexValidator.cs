﻿using FluentValidation;

namespace application.validation;

public static class RegexValidator  
{  
    private static readonly string GuidRegexPattern = "^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}$"; 

    public static IRuleBuilderOptions<T, string> MustBeValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder, string entity)  
    {  
        return ruleBuilder  
            .NotEmpty() 
            .Matches(GuidRegexPattern)
            .WithMessage($"{entity} must be a valid GUID.");  
    }  
} 