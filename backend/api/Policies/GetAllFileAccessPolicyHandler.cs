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
            var user = userService.GetUserByEmail(email);
            var userId = user.Id;
            
            var fileId = request.RouteValues["id"];
            if (fileId == null) throw new BadHttpRequestException("id query parameter must be provided.");
            var fileIdString = fileId.ToString();
            if (string.IsNullOrEmpty(fileIdString)) throw new BadHttpRequestException("id query parameter must be provided.");
            
            var dto = new GetFileOrAccessInputDto {FileId = fileIdString,UserId = userId};

            var access=fileService.GetUserFileAccess(dto);

            if (access.Role != Roles.Editor) return;
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        throw new NotImplementedException();
    }
}