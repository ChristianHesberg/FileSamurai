using System.Security.Claims;
using System.Text.Json;
using application.dtos;
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
        Console.WriteLine("email:"+email);
        if (email == null) return; 
        
        //Get the groupId from the body of the http request
    
        try
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };
            var dto = JsonSerializer.Deserialize<AddUserToGroupDto>(body,options);
            if (dto == null)return;
            var group = groupService.GetGroup(dto.GroupId);
            if (group == null) return;
            if (group.GroupEmail == email)
            {
                authorizationHandlerContext.Succeed(requirement);
                request.Body.Position = 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
    }
}