using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.SchemaFilters;

using Microsoft.OpenApi.Models;  
using Swashbuckle.AspNetCore.SwaggerGen;  
using System.Net;  

public class GlobalResponseTypeSchemaFilter : IOperationFilter  
{  
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var statusCodes = new[] {  
            HttpStatusCode.InternalServerError,
            HttpStatusCode.Conflict
        };

        var statusCodesWithErrors = new[]
        {
            HttpStatusCode.BadRequest,
        };

        foreach (var statusCode in statusCodes)  
        {  
            GenerateSchema<ErrorMessageResponse>(operation, context, statusCode);
        }
        foreach (var statusCode in statusCodesWithErrors)  
        {  
            GenerateSchema<ErrorResponse>(operation, context, statusCode);
        }  
        // Add 200 OK response  
        var returnType = context.MethodInfo.ReturnType;  
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ActionResult<>))  
        {  
            returnType = returnType.GetGenericArguments()[0]; 
        }  
        else if (returnType == typeof(ActionResult))  
        {  
            returnType = typeof(void); 
        }
        
        GenerateSchema(operation, context, HttpStatusCode.OK, returnType); 
    }
    
    private void GenerateSchema<T>(OpenApiOperation operation, OperationFilterContext context, HttpStatusCode statusCode)  
    {  
        GenerateSchema(operation, context, statusCode, typeof(T));  
    }  

    private void GenerateSchema(OpenApiOperation operation, OperationFilterContext context, HttpStatusCode statusCode, Type type)
    {
        if (!operation.Responses.ContainsKey(((int)statusCode).ToString()))  
        {  
            operation.Responses.Add(((int)statusCode).ToString(), new OpenApiResponse  
            {  
                Description = statusCode.ToString(),  
                Content = new Dictionary<string, OpenApiMediaType>  
                {  
                    ["application/json"] = new OpenApiMediaType  
                    {  
                        Schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository)  
                    },
                    ["text/plain"] = new OpenApiMediaType  
                    {  
                        Schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository)  
                    }  
                }  
            });  
        } 
    }
}  
