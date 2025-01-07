using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json;
using api.Policies.UtilMethods;
using application.dtos;
using application.ports;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairPostHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<KeyPairPostRequirement>
{
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext, KeyPairPostRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);

            var dto = await BodyToDto.BodyToDtoConverter<UserRsaKeyPairDto>(request);

            if (user.Id == dto.UserId)
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