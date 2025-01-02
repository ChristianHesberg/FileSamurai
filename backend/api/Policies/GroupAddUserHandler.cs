using System.Security.Claims;
using System.Text.Json;
using application.services;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class GroupAddUserHandler(IUserService userService,IGroupService groupService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<GroupAddUserRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        GroupAddUserRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        request.EnableBuffering();
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return; 
        
        //Get the groupId from the body of the http request
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
        
        if (groupId == null) return;
        var group = groupService.GetGroup(groupId);
        if (group == null) return;
        
        
        if (group.GroupEmail == email)
        {
            authorizationHandlerContext.Succeed(requirement);
            request.Body.Position = 0;
        }
    
    }
}