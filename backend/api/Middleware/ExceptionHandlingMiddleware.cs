namespace api.Middleware;

using Microsoft.AspNetCore.Http;  
using Microsoft.Extensions.Logging;  
using System;  
using System.Net;  
using System.Threading.Tasks;  
using Newtonsoft.Json;  
using application.errors;
using core.errors;
  
public class ExceptionHandlingMiddleware  
{  
    private readonly RequestDelegate _next;  
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;  
  
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)  
    {  
        _next = next;  
        _logger = logger;  
    }  
  
    public async Task InvokeAsync(HttpContext context)  
    {  
        try  
        {  
            await _next(context);  
        }  
        catch (Exception ex)  
        {  
            _logger.LogError($"An exception occurred: {ex.Message}");  
            await HandleExceptionAsync(context, ex);  
        }  
    }  
  
    private Task HandleExceptionAsync(HttpContext context, Exception exception)  
    {  
        HttpStatusCode statusCode;  
        string message;
        IList<string> validationFailures = [];

        switch (exception)
        {
            case CustomValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;  
                message = validationException.Message;
                validationFailures = validationException.Errors;
                break;
            
            case BadHttpRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message; 
                break;  
            
            case UnauthorizedAccessException:  
                statusCode = HttpStatusCode.Unauthorized;  
                message = "Unauthorized access.";  
                break;  
  
            case EntityAlreadyExistsException:  
                statusCode = HttpStatusCode.Conflict;  
                message = exception.Message;  
                break;  
            
            case KeyNotFoundException:  
                statusCode = HttpStatusCode.NotFound;  
                message = exception.Message;  
                break;
            
            case DatabaseUpdateException:
                statusCode = HttpStatusCode.InternalServerError;  
                message = exception.Message;  
                break;
  
            default:  
                statusCode = HttpStatusCode.InternalServerError;  
                message = "Internal server error.";  
                break; 
        }
  
        context.Response.StatusCode = (int)statusCode;  
        context.Response.ContentType = "application/json"; 

        return context.Response.WriteAsync(JsonConvert.SerializeObject(validationFailures.Count == 0 ? new  
        {  
            Message = message,
        } : new 
        {
            Message = message,
            Errors = validationFailures
        })); 
    }  
}  