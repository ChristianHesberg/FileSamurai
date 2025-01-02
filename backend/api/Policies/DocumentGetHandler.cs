using System.Security.Claims;
using System.Text.Json;
using application.dtos;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentGetHandler(IUserService userService,IFileService fileService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentGetRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentGetRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return; 
        
        var user = userService.GetUserByEmail(email);
        if (user == null) return;

        // Extract fileId from the Query
        var fileId = request.Query["fileId"].ToString();
        if (string.IsNullOrEmpty(fileId))return;
        
        
        //CHECK IF USER IS IN TABLE FOR DOCUMENT ACCESS
        GetFileOrAccessInputDto retrieveFile = new GetFileOrAccessInputDto()
        {
            FileId = fileId,
            UserId = user.Id
        }; 
        
        //if file is meaning either file or fileaccess was null and therefor no allowed to view file
        var file =  fileService.GetFile(retrieveFile);
        if (file == null) return;
        
        
        // GET User Groups and File group
        var userGroup = userService.GetGroupsForUser(user.Id);
        if (userGroup == null) return;
        
        var fileGroup = fileService.GetFileGroup(file.File.Id);
        if (fileGroup == null)return;
        
        //CHECK USERS IS IN the same GROUP AS THE DOCUMENT
        if (userGroup.Contains(fileGroup))
        {
            authorizationHandlerContext.Succeed(requirement);
        }            
    }
}