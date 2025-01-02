using application.errors;

namespace api.Middleware;

using Microsoft.AspNetCore.Http;  
using Microsoft.Extensions.Logging;  
using System;  
using System.Net;  
using System.Threading.Tasks;  
using Newtonsoft.Json;  
  
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
  
        // Customize your response based on the exception type  
        if (exception is CustomValidationException validationException)
        {
            statusCode = HttpStatusCode.BadRequest;  
            message = validationException.Message;
            validationFailures = validationException.ValidationErrors;
        }  
        else if (exception is UnauthorizedAccessException)  
        {  
            statusCode = HttpStatusCode.Unauthorized;  
            message = "Unauthorized access.";  
        }  
        else  
        {  
            statusCode = HttpStatusCode.InternalServerError;  
            message = "Internal server error.";  
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