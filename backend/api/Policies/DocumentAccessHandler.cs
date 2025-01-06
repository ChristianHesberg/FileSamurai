using System.Security.Claims;
using System.Text.Json;
using application.dtos;
using application.ports;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class DocumentAccessHandler(
    IUserService userService,
    IFileService fileService,
    IHttpContextAccessor contextAccessor,
    IFilePort filePort) : AuthorizationHandler<DocumentAccessRequirement>
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
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var dto = JsonSerializer.Deserialize<AddOrGetUserFileAccessDto>(body, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            if (dto == null) return;

            //Retrieve the file + fileaccess for the user, if file or fileaccess is null user dont have access!  
            var retrieveFile = new GetFileOrAccessInputDto()
            {
                FileId = dto.FileId,
                UserId = userId
            };

            var file = filePort.GetFile(retrieveFile.FileId);
            if (file == null) return;

            // GET User Groups and File group
            var userGroup = userService.GetGroupsForUser(userId);
            if (userGroup == null) return;

            var fileGroup = fileService.GetFileGroup(file.Id);
            if (fileGroup == null) return;


            var userIsInFileGroup = userGroup.Any(x => x.Id == fileGroup.Id);
            if (!userIsInFileGroup) return;

            //If no userFileAccess
            if (fileService.GetAllUserFileAccess(file.Id).Count == 0)
            {
                authorizationHandlerContext.Succeed(requirement);
            }
            else
            {
                var access = fileService.GetUserFileAccess(new GetFileOrAccessInputDto()
                    { UserId = userId, FileId = file.Id });
                if (access == null) return;
                if (access.Role == Roles.Editor)
                {
                    //CHECK USERS IS IN the same GROUP AS THE DOCUMENT && is an editor
                    authorizationHandlerContext.Succeed(requirement);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
        request.Body.Position = 0;
    }
}