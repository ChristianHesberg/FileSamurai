using System.Security.Claims;
using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using api.Policies.UtilMethods;
using application.ports;

namespace api.Policies;

public class DocumentAddHandler(IUserService userService, IHttpContextAccessor contextAccessor)
    : AuthorizationHandler<Requirements.DocumentAddRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.DocumentAddRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;

        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);

            var userId = user.Id;
        
            var dto = await BodyToDto.BodyToDtoConverter<AddFileDto>(request);

            // GET User Groups and File group
            var userGroup = userService.GetGroupsForUser(userId);

            var res = userGroup.Any(group => group.Id == dto.GroupId);
            if (res)
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