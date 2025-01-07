using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using api.Policies.UtilMethods;
using application.dtos;
using application.ports;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;


namespace api.Policies;

public class DocumentAccessHandler(
    IUserPort userAdapter,
    IFilePort fileAdapter,
    IHttpContextAccessor contextAccessor,
    IFilePort filePort) : AuthorizationHandler<DocumentAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentAccessRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;

        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null) return;

        try
        {
            var user = userAdapter.GetUserByEmail(email);

            var userId = user.Id;

            var dto = await BodyToDto.BodyToDtoConverter<AddOrGetUserFileAccessDto>(request);

            var userGroup = userAdapter.GetGroupsForUser(userId);

            var fileGroup = fileAdapter.GetFileGroup(dto.FileId);

            var userIsInFileGroup = userGroup.Any(x => x.Id == fileGroup.Id);
            if (!userIsInFileGroup) return;

            //If no userFileAccess on file
            if (fileAdapter.GetAllUserFileAccess(dto.FileId).Count == 0)
            {
                authorizationHandlerContext.Succeed(requirement);
            }
            else
            {
                var access = fileAdapter.GetUserFileAccess(userId, dto.FileId);
                if (access.Role == Roles.Editor)
                {
                    authorizationHandlerContext.Succeed(requirement);
                }
            }
        }
        catch (KeyNotFoundException)
        {
            authorizationHandlerContext.Fail();
        }
    }
}