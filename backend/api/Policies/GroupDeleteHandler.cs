using System.Security.Claims;
using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class GroupDeleteHandler(IUserService userService, IGroupService groupService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.GroupDeleteRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.GroupDeleteRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            // Extract groupId from the Query
            var groupId = request.Query["Id"].ToString();
            if (string.IsNullOrEmpty(groupId)) throw new BadHttpRequestException("groupId query parameter must be provided.");
            
            
            var group = groupService.GetGroup(groupId);
            
            //CHECK if user is owner of group
            if (group.GroupEmail == email)
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