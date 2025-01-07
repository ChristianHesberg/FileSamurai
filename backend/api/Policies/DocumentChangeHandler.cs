using System.Security.Claims;
using System.Text.Json;
using api.Policies.UtilMethods;
using application.dtos;
using application.ports;
using application.services;
using core.models;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentChangeHandler(IUserPort userAdapter, IFilePort fileAdapter, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentChangeRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        DocumentChangeRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
    
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userAdapter.GetUserByEmail(email);

            var dto = await BodyToDto.BodyToDtoConverter<FileDto>(request);
        
            var file = fileAdapter.GetFile(dto.Id);

            var userAccess = fileAdapter.GetUserFileAccess(user.Id, file.Id); 
  
            if (userAccess.Role == Roles.Editor)
            {
                authorizationHandlerContext.Succeed(requirement); 
            }
        }
        catch (KeyNotFoundException)
        {
            authorizationHandlerContext.Fail();
        }
    }
}