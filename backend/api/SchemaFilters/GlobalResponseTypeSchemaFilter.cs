using api.Models;

namespace api.SchemaFilters;

using Microsoft.OpenApi.Models;  
using Swashbuckle.AspNetCore.SwaggerGen;  
using System.Net;  

public class GlobalResponseTypeSchemaFilter : IOperationFilter  
{  
    public void Apply(OpenApiOperation operation, OperationFilterContext context)  
    {  
        var statusCodes = new[] {  
            HttpStatusCode.Unauthorized,  
            HttpStatusCode.InternalServerError,
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
    }

    private void GenerateSchema<T>(OpenApiOperation operation, OperationFilterContext context, HttpStatusCode statusCode)
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
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository)  
                    }  
                }  
            });  
        } 
    }
}  
