using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairPostHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<KeyPairPostRequirement>
{
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext, KeyPairPostRequirement requirement)
    {
        var request = contextAccessor.HttpContext?.Request;
        request?.EnableBuffering();
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        var user = userService.GetUserByEmail(email);
        if (user == null) return;
        
        // Extract userid from the body
        string userIdFromBody = "";
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.TryGetValue("userId", out var value) is true)
            {
                userIdFromBody = value;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
            
        }

        if (user.Id == userIdFromBody)
        {
            authorizationHandlerContext.Succeed(requirement); 
            request!.Body.Position = 0;
        }
        
    }
}