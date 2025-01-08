using System.Security.Claims;
using System.Text.Json;
using api.Policies.UtilMethods;
using application.dtos;
using application.ports;
using application.services;
using core.models;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentChangeHandler(IUserService userService, IFileService fileService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.DocumentChangeRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.DocumentChangeRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
    
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);

            var dto = await BodyToDto.BodyToDtoConverter<FileDto>(request);
        
            var file = fileService.GetFile(new GetFileOrAccessInputDto()
            {
                UserId = user.Id,
                FileId = dto.Id
            });
  
            if (file.UserFileAccess.Role == Roles.Editor)
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