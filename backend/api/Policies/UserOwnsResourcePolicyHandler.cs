using System.Security.Claims;
using application.services;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class UserOwnsResourcePolicyHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.UserOwnsResourcePolicyRequirement>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.UserOwnsResourcePolicyRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");

        var request = accessor.Request;

        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);

            var id = request.RouteValues["id"];
            if (id == null) throw new BadHttpRequestException("id for user must be provided. ");
            if (string.IsNullOrEmpty(id.ToString()))
                throw new BadHttpRequestException("id for user must be provided. ");

            if (user.Id == id.ToString())
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