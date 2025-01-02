using System.Security.Claims;
using System.Text.Json;
using infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairPostHandler(Context context, IHttpContextAccessor contextAccessor) : AuthorizationHandler<KeyPairPostRequirement>
{
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context1, KeyPairPostRequirement requirement)
    {
        var request = contextAccessor.HttpContext?.Request;
        request?.EnableBuffering();
    
        
        var email = context1.User.FindFirst(ClaimTypes.Email)?.Value;
        Console.WriteLine(email);
        
       
        var userId = context.Users.FirstOrDefault(user => user.Email == email).Id;
        
        // Extract fileId from the body (JSON data)
        string userIdFromBody = null;
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.ContainsKey("userId") == true)
            {
                userIdFromBody = jsonObject["userId"];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }

        if (userId == userIdFromBody)
        {
            context1.Succeed(requirement); 
            request!.Body.Position = 0;
        }
        
        return;
        
        
    }
}