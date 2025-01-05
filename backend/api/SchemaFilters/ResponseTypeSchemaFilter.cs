using api.Models;

namespace api.SchemaFilters;

using Microsoft.OpenApi.Models;  
using Swashbuckle.AspNetCore.SwaggerGen;  
using System.Net;  

public class ResponseTypeSchemaFilter : IOperationFilter  
{  
    public void Apply(OpenApiOperation operation, OperationFilterContext context)  
    {  
        var statusCodes = new[] {  
            HttpStatusCode.BadRequest,  
            HttpStatusCode.Unauthorized,  
            HttpStatusCode.InternalServerError  
        };  

        foreach (var statusCode in statusCodes)  
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
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResponse), context.SchemaRepository)  
                        }  
                    }  
                });  
            }  
        }  
    }  
}  
