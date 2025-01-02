using System.Security.Claims;
using System.Text.Json;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentGetHandler(Context context, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentGetRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        DocumentGetRequirement requirement)
    {
        Console.WriteLine("starting getting file policy");
        var request = contextAccessor.HttpContext.Request;
        request.EnableBuffering();
        
        var email = context1.User.FindFirst(ClaimTypes.Email)?.Value;
        Console.WriteLine(email);

        
        if (email == null)
        {
            return; 
        }
        var user = context.Users.Include(u => u.Groups).FirstOrDefault(user => user.Email == email);
        Console.WriteLine(user);

        if (user == null) return;
        
        var userId = user.Id;
        Console.WriteLine(userId);

        // Extract fileId from the Query
        var fileIdFromQuery = request.Query["fileId"].ToString();
        if (string.IsNullOrEmpty(fileIdFromQuery))
        {
            return;
        }

        
        //CHECK IF USER IS IN TABLE FOR DOCUMENT ACCESS
        var file = await context.Files.FindAsync(fileIdFromQuery);
        
        if (file == null) return;
        

        
        var hasAccess = await context.UserFileAccesses.AnyAsync(db =>
            db.User.Id == userId && db.File.Id == fileIdFromQuery);

        
        //CHECK USERS IS IN the same GROUP AS THE DOCUMENT

        bool userFileGroup = user.Groups.Contains(file.Group);

        Console.WriteLine(hasAccess);
        Console.WriteLine(userFileGroup);
        if (hasAccess && userFileGroup)
        {
            Console.WriteLine("passed");
            context1.Succeed(requirement);
            request.Body.Position = 0;
        }            
        
        // Deny access if neither condition is met
        return;
    }
}