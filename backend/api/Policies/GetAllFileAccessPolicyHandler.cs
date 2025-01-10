using System.Security.Claims;
using AngleSharp.Io.Dom;
using application.dtos;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class GetAllFileAccessPolicyHandler(
    IFileService fileService,
    IUserService userService,
    IHttpContextAccessor contextAccessor)
    : AuthorizationHandler<Requirements.GetAllFileAccessPolicyRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.GetAllFileAccessPolicyRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");

        var request = accessor.Request;

        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null) return;

        //check user is editor on file and in the group

        try
        {
            //get user id
            var user = userService.GetUserByEmail(email);
            var userId = user.Id;
            
            //get file id
            var fileId = request.RouteValues["id"];
            if (fileId == null) throw new BadHttpRequestException("id query parameter must be provided.");
            var fileIdString = fileId.ToString();
            if (string.IsNullOrEmpty(fileIdString)) throw new BadHttpRequestException("id query parameter must be provided.");
            
            //get file
            var dto = new GetFileOrAccessInputDto {FileId = fileIdString,UserId = userId};
            var fileDto=fileService.GetFile(dto);
            
            //Check if has access to share file
            if (fileDto.UserFileAccess.Role != Roles.Editor) return;
            
            //get users group
            var userGroups = userService.GetGroupsForUser(userId);

            //ensure that user is part of group
            if (userGroups.Any(x => x.Id == fileDto.File.GroupId))
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