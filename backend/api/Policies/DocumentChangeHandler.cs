using System.Security.Claims;
using System.Text.Json;
using application.dtos;
using application.services;
using core.models;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentChangeHandler(IUserService userService,IFileService fileService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentChangeRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentChangeRequirement requirement)
    {
        var request = contextAccessor.HttpContext?.Request;
        request?.EnableBuffering();
    
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return; 
        
        
        var user = userService.GetUserByEmail(email);
        if (user == null) return;
        
        
        
        // Extract fileId from the body (JSON data)
        string fileIdFromBody = null;
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.TryGetValue("id", out var value) is true)
            {
                fileIdFromBody = value;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
        Console.WriteLine("FileId from body: " + fileIdFromBody);

        
        
        //CHECKS if user has editor role for file
        GetFileOrAccessInputDto retrieveFile = new GetFileOrAccessInputDto()
        {
            FileId = fileIdFromBody,
            UserId = user.Id
        }; 
        
        var file =  fileService.GetFile(retrieveFile);
        if (file == null) return;
        
        
        //TODO REMOVE HARDCODED editor
        if (file?.UserFileAccess.Role == Roles.Editor)
        {
            authorizationHandlerContext.Succeed(requirement); 
            request.Body.Position = 0;

        }
    }
}