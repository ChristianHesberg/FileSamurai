using System.Security.Claims;
using System.Text.Json;
using application.dtos;
using application.ports;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentGetHandler(IUserService userService, IFileService fileService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.DocumentGetRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.DocumentGetRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);

            // Extract fileId from the Query
            var fileId = request.Query["fileId"].ToString();
            if (string.IsNullOrEmpty(fileId)) throw new BadHttpRequestException("fileId query parameter must be provided.");
        
            var file =  fileService.GetFile(new GetFileOrAccessInputDto()
            {
                FileId = fileId,
                UserId = user.Id
            });
        
            // GET User Groups and File group
            var userGroup = userService.GetGroupsForUser(user.Id);
        
            var fileGroup = fileService.GetFileGroup(file.File.Id);
        
            //CHECK USERS IS IN the same GROUP AS THE DOCUMENT
            if (userGroup.Any(x => x.Id == fileGroup.Id))
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