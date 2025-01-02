using System.Security.Claims;
using System.Text.Json;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairGetPKHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<KeyPairGetPKRequirement>
{
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext, KeyPairGetPKRequirement requirement)
    {
        var request = contextAccessor.HttpContext?.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        
       
        var user = userService.GetUserByEmail(email);
        
        // Grabs the userid in which the user wants to retrieve PK From
         var userIdFromRoute = request.RouteValues["id"]?.ToString();
        if (string.IsNullOrEmpty(userIdFromRoute)) return;
        

        //ensure user only Grabs their own PK 
        if (user.Id == userIdFromRoute)
        {
            authorizationHandlerContext.Succeed(requirement); 
        }
    }
}