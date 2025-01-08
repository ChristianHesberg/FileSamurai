using System.Security.Claims;
using api.Policies.UtilMethods;
using application.dtos;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class DocumentDeleteAccessHandler(
    IUserService userService,
    IFileService fileService,
    IHttpContextAccessor contextAccessor
) : AuthorizationHandler<Requirements.DocumentDeleteAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.DocumentDeleteAccessRequirement requirement)
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
            
            // Extract groupId from the Query
            var fileId =  request.Query["fileId"].ToString();
            if (string.IsNullOrEmpty(fileId)) throw new BadHttpRequestException("id for user must be provided. ");
            
            var dto = new GetFileOrAccessInputDto()
            {
                FileId = fileId,
                UserId = userId
            };

            var access = fileService.GetUserFileAccess(dto);
            
            if (access.Role == Roles.Editor)
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