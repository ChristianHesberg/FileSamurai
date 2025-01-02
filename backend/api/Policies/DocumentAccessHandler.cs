using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json;
using AngleSharp.Io.Dom;
using application.dtos;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class DocumentAccessHandler(IUserService userService, IFileService fileService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentAccessRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        request.EnableBuffering();
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (email == null) return; 
        
        var user = userService.GetUserByEmail(email);

        if (user == null) return;
        
        var userId = user.Id;
        
        // Extract fileId from the body (JSON data)
        string fileIdFromBody = null;
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.ContainsKey("fileId") == true)
            {
                fileIdFromBody = jsonObject["fileId"];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
        Console.WriteLine("FileId from body: " + fileIdFromBody);

        //Retrieve the file + fileaccess for the user, if file or fileaccess is null user dont have access!  
        GetFileOrAccessInputDto retrieveFile = new GetFileOrAccessInputDto()
        {
            FileId = fileIdFromBody,
            UserId = userId
        }; 
        
        var file =  fileService.GetFile(retrieveFile);
        if (file == null) return;

        // GET User Groups and File group
        var userGroup = userService.GetGroupsForUser(userId);
        if (userGroup == null) return;
        
        var fileGroup = fileService.GetFileGroup(file.File.Id);
        if (fileGroup == null)return;
        
        //CHECK USERS IS IN the same GROUP AS THE DOCUMENT && is an editor
        if (userGroup.Contains(fileGroup) && file.UserFileAccess.Role == "Editor")
        {
            authorizationHandlerContext.Succeed(requirement);
            request.Body.Position = 0;
        }            
    
    }
}