using application.validation;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.SchemaFilters;

public class FluentValidationSchemaFilter(IServiceProvider serviceProvider) : ISchemaFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var scope = _serviceProvider.CreateScope();
        var validatorType = typeof(IValidator<>).MakeGenericType(context.Type);
        var validator = scope.ServiceProvider.GetService(validatorType) as IValidator;
        if (validator == null) return;
        var rules = validator.CreateDescriptor().GetMembersWithValidators();

        Console.WriteLine(rules);
        
        foreach (var group in rules)
        {
            var name = group.Key;
            var rule = group;
            if (schema.Properties.TryGetValue(name, out var property))

            //var a =schema.Properties.Keys;
            property.Description = "this is a filler desc";
            foreach (var (validatorInstance, _) in group)
            {
                if (validatorInstance is GetFileOrAccessInputDtoValidator)
                {
                    property.Format = "uuid"; // Set OpenAPI format to indicate it's a GUID
                    property.Description = "Must be a valid GUID.";
                }
            }
        }
    }
}