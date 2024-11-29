using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using shared;

namespace api.Policies;

public class DocumentAccessHandler(Context context, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        DocumentAccessRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        request.EnableBuffering();
        
        var userId = context1.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return; 
        }


        //CHECK IF USER IS IN TABLE FOR DOCUMENT ACCESS

        var file = await context.Files.FindAsync(requirement.DocumentId);
        
        if (file == null) return;
        

        
        var hasAccess = await context.UserFileAccesses.AnyAsync(db =>
            db.User.Id == userId && db.File.Id == requirement.DocumentId);

        
        //CHECK USERS IS IN the same GROUP AS THE DOCUMENT

        var userFileGroup = await context.UserGroupMemberships.AnyAsync(db =>
            file != null && db.Member.Id == userId && db.Group.GroupId == file.Group.GroupId);


        if (hasAccess && userFileGroup)
        {
            context1.Succeed(requirement);
            request.Body.Position = 0;
        }            
        
        // Deny access if neither condition is met
        return;
    }
}