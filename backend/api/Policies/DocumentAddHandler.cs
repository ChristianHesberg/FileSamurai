using System.Security.Claims;
using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace api.Policies;

public class DocumentAddHandler(IUserService userService, IHttpContextAccessor contextAccessor)
    : AuthorizationHandler<DocumentAddRequirement>
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
        try
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);

            var body = await reader.ReadToEndAsync();


            var dto = JsonSerializer.Deserialize<AddFileDto>(body, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            Console.WriteLine("1:" + dto);
            if (dto == null) return;

            // GET User Groups and File group
            var userGroup = userService.GetGroupsForUser(userId);
            Console.WriteLine("2:" + userGroup);
            if (userGroup == null) return;

            var res = userGroup.Any(group => group.Id == dto.GroupId);
            if (res)
            {
                request.Body.Position = 0;
                authorizationHandlerContext.Succeed(requirement);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
    }
}