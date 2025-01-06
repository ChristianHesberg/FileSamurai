using System.Text.Json;
using System.Text.Json.Serialization;
using application.dtos;

namespace api.Policies.UtilMethods;

public static class BodyToDto
{
    public static async Task<T> BodyToDtoConverter<T>(HttpRequest request)
    {
        try
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body =  await reader.ReadToEndAsync();
            var dto = JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            request.Body.Position = 0;
            if (dto == null) throw new BadHttpRequestException("Request could not be serialized.");
            return dto;
        }
        catch (Exception ex)
        {
            request.Body.Position = 0;
            throw new BadHttpRequestException("Request could not be serialized.");
        }
        
    }
}