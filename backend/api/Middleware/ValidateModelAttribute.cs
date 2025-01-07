using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Middleware;

public class ValidateModelAttribute : ActionFilterAttribute  
{  
    private readonly Type _validatorType;  

    public ValidateModelAttribute(Type validatorType)  
    {  
        _validatorType = validatorType;  
    }  

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)  
    {  
        var validator = (IValidator)context.HttpContext.RequestServices.GetService(_validatorType);  

        if (validator != null)  
        {  
            var argument = context.ActionArguments.Values.FirstOrDefault();
            if ( argument == null)
            {
                context.Result = new BadRequestResult();
                return;
            }
            var validationResult = await validator.ValidateAsync(new ValidationContext<object>(argument));  

            if (!validationResult.IsValid)  
            {  
                context.Result = new BadRequestObjectResult(validationResult.Errors);  
                return;  
            }  
        }  
        await next();  
    }  
}