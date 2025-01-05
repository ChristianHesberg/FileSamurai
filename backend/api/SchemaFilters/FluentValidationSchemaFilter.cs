using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.SchemaFilters;

public class AddQueryParameterDescription : IOperationFilter  
{  
    public void Apply(OpenApiOperation operation, OperationFilterContext context)  
    {  
        // Check if the method has parameters with custom attributes  
        var parametersWithAttributes = context.MethodInfo.GetParameters()  
            .SelectMany(p => p.GetCustomAttributes(false).Select(attr => new { Parameter = p, Attribute = attr }))  
            .ToList();  
  
        foreach (var paramWithAttr in parametersWithAttributes)  
        {  
            if (paramWithAttr.Attribute is CustomDescriptionAttribute descriptionAttribute)  
            {  
                var parameter = operation.Parameters.FirstOrDefault(p => p.Name == paramWithAttr.Parameter.Name);  
                if (parameter != null)  
                {  
                    parameter.Description = descriptionAttribute.Description;  
                }  
            }  
        }  
    }  
}  
  
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]  
public class CustomDescriptionAttribute : Attribute  
{  
    public string Description { get; }  
  
    public CustomDescriptionAttribute(string description)  
    {  
        Description = description;  
    }  
}