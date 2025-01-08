using System.Security.Claims;
using api.Policies.UtilMethods;
using application.dtos;
using application.errors;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class DocumentAccessHandler(
    IUserService userService,
    IFileService fileService,
    IHttpContextAccessor contextAccessor
    ) : AuthorizationHandler<Requirements.DocumentAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.DocumentAccessRequirement requirement)
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

            var dto = await BodyToDto.BodyToDtoConverter<AddOrGetUserFileAccessDto>(request);

            var userGroup = userService.GetGroupsForUser(userId);

            var fileGroup = fileService.GetFileGroup(dto.FileId);

            var userIsInFileGroup = userGroup.Any(x => x.Id == fileGroup.Id);
            if (!userIsInFileGroup) return;

            //If no userFileAccess on file
            if (fileService.GetAllUserFileAccess(dto.FileId).Count == 0)
            {
                authorizationHandlerContext.Succeed(requirement);
            }
            else
            {
                var access = fileService.GetUserFileAccess(new GetFileOrAccessInputDto()
                {
                    UserId = userId,
                    FileId = dto.FileId
                });
                if (access.Role == Roles.Editor)
                {
                    authorizationHandlerContext.Succeed(requirement);
                }
            }
        }
        catch (KeyNotFoundException)
        {
            authorizationHandlerContext.Fail();
        }
    }
}