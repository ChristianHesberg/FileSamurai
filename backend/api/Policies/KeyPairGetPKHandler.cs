using System.Security.Claims;
using System.Text.Json;
using infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairGetPKHandler(Context context, IHttpContextAccessor contextAccessor) : AuthorizationHandler<KeyPairGetPKRequirement>
{
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context1, KeyPairGetPKRequirement requirement)
    {
        var request = contextAccessor.HttpContext?.Request;
        request?.EnableBuffering();
    
        
        var email = context1.User.FindFirst(ClaimTypes.Email)?.Value;
        Console.WriteLine(email);
        
       
        var userId = context.Users.FirstOrDefault(user => user.Email == email).Id;
        
        // Grabs the userid in which the user wants to retreive PK From
         var userIdFromRoute = request.RouteValues["id"]?.ToString();
        if (string.IsNullOrEmpty(userIdFromRoute))
        {
            return;
        }

        //ensure user only Grabs their own PK 
        if (userId == userIdFromRoute)
        {
            context1.Succeed(requirement); 
            request!.Body.Position = 0;
        }
        
        return;
        
        
    }
}