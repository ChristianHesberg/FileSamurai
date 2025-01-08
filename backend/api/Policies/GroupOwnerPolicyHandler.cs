using System.Security.Claims;
using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class GroupOwnerPolicyHandler(IGroupService groupService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.GroupOwnerPolicyRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.GroupOwnerPolicyRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            
            // Extract groupId from the Query
            var groupId = request.RouteValues["groupId"] != null ? request.RouteValues["groupId"] : request.Query["groupId"];
            if (groupId == null) throw new BadHttpRequestException("id for user must be provided. ");
            
            var groupIdString = groupId.ToString();
            if (string.IsNullOrEmpty(groupIdString)) throw new BadHttpRequestException("id for user must be provided. ");
            var group = groupService.GetGroup(groupIdString);
            
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