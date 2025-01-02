using System.Security.Claims;
using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class DocumentGetUserFileAccessHandler(IUserService userService,IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentGetRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentGetRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return; 
        
        var user = userService.GetUserByEmail(email);
        if (user == null) return;

        // Extract fileId from the Query
        var userId = request.Query["userId"].ToString();
        if (string.IsNullOrEmpty(userId))return;
        

        if (userId == user.Id)
        {
            authorizationHandlerContext.Succeed(requirement);
        }            
    }
}