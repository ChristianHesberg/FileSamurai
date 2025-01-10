using System.Security.Claims;
using api.Policies.UtilMethods;
using application.dtos;
using application.errors;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class ListAccessHandler(
    IUserService userService,
    IFileService fileService,
    IHttpContextAccessor contextAccessor
    ) : AuthorizationHandler<Requirements.ListAccessPolicyRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.ListAccessPolicyRequirement requirement)
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

            var dtoList = await BodyToDto.BodyToDtoConverter<List<AddOrGetUserFileAccessDto>>(request);

            var userGroup = userService.GetGroupsForUser(userId);
            foreach (var dto in dtoList)       
            {
                var fileGroup = fileService.GetFileGroup(dto.FileId);

                var userIsInFileGroup = userGroup.Any(x => x.Id == fileGroup.Id);
                if (!userIsInFileGroup) authorizationHandlerContext.Fail();
                    var access = fileService.GetUserFileAccess(new GetFileOrAccessInputDto()
                    {
                        UserId = userId,
                        FileId = dto.FileId
                    });
                    if (access.Role != Roles.Editor)
                    {
                       authorizationHandlerContext.Fail();
                    }
            }
            authorizationHandlerContext.Succeed(requirement);
        }
        catch (KeyNotFoundException)
        {
            authorizationHandlerContext.Fail();
        }
    }
}