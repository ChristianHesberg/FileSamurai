using System.Security.Claims;
using System.Text.Json;
using api.Policies.UtilMethods;
using application.dtos;
using application.ports;
using application.services;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class GroupAddUserHandler(IGroupPort groupAdapter, IHttpContextAccessor contextAccessor) : AuthorizationHandler<GroupAddUserRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        GroupAddUserRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var dto = await BodyToDto.BodyToDtoConverter<AddUserToGroupDto>(request);
        
            var group = groupAdapter.GetGroup(dto.GroupId);
        
            if (group.CreatorEmail == email)
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