using System.Security.Claims;
using System.Text.Json;
using application.dtos;
using application.ports;
using application.services;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentGetHandler(IUserPort userAdapter, IFilePort fileAdapter, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentGetRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentGetRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return; 
        
        var user = userAdapter.GetUserByEmail(email);

        // Extract fileId from the Query
        var fileId = request.Query["fileId"].ToString();
        if (string.IsNullOrEmpty(fileId)) throw new BadHttpRequestException("fileId query parameter must be provided.");
        
        var file =  fileAdapter.GetFile(fileId);
        
        // GET User Groups and File group
        var userGroup = userAdapter.GetGroupsForUser(user.Id);
        
        var fileGroup = fileAdapter.GetFileGroup(file.Id);

        fileAdapter.GetUserFileAccess(user.Id, file.Id);
        
        //CHECK USERS IS IN the same GROUP AS THE DOCUMENT
        if (userGroup.Any(x => x.Id == fileGroup.Id))
        {
            authorizationHandlerContext.Succeed(requirement);
        }            
    }
}