using System.Security.Claims;
using application.services;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class GroupGetHandler(IUserService userService, IGroupService groupService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.GroupGetRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.GroupGetRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            // Extract groupId from the Query
            var groupId = request.RouteValues["groupId"];
            if (groupId == null) throw new BadHttpRequestException("groupId query parameter must be provided.");
         
            
            if (string.IsNullOrEmpty(groupId.ToString())) throw new BadHttpRequestException("groupId query parameter must be provided.");

            var user = userService.GetUserByEmail(email);

            var userGroups = userService.GetGroupsForUser(user.Id);
            
            
            
            //CHECK if user belongs to group
            if (userGroups.Any(x => x.Id == groupId.ToString()))
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