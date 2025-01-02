using System.Security.Claims;
using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace api.Policies;

public class DocumentAddHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentAddRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentAddRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        request.EnableBuffering();
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (email == null) return; 
        
        var user = userService.GetUserByEmail(email);

        if (user == null) return;
        
        var userId = user.Id;
        
        // Extract groupId from the body 
        string groupId = null;
        try
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.TryGetValue("GroupId", out var value) is true)
            {
                groupId = value;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
        Console.WriteLine("FileId from body: " + groupId);

    
        
   
        // GET User Groups and File group
        var userGroup = userService.GetGroupsForUser(userId);
        if (userGroup == null) return;

        var res = userGroup.Where(group => group.Id == groupId);
        if (res.Any())
        {
            authorizationHandlerContext.Succeed(requirement);
            request.Body.Position = 0;
        }

    }
}