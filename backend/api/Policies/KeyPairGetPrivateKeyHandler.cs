using System.Security.Claims;
using System.Text.Json;
using application.ports;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairGetPrivateKeyHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<KeyPairGetPrivateKeyRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext, KeyPairGetPrivateKeyRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);
        
            // Grabs the userid in which the user wants to retrieve private key from
            var userIdFromRoute = request.RouteValues["id"];
            if (userIdFromRoute == null) throw new BadHttpRequestException("id for user must be provided. ");
            if (string.IsNullOrEmpty(userIdFromRoute.ToString())) throw new BadHttpRequestException("id for user must be provided. ");
        
            //ensure user only Grabs their own PK 
            if (user.Id == userIdFromRoute.ToString())
            {
                authorizationHandlerContext.Succeed(requirement); 
            }
        }
        catch (KeyNotFoundException)
        {
            authorizationHandlerContext.Fail();
        }
    }
}