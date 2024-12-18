using System.Security.Claims;
using System.Text.Json;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api.Policies;

public class DocumentChangeHandler(Context context, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentChangeRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        DocumentChangeRequirement requirement)
    {
        Console.WriteLine("startet");
        var request = contextAccessor.HttpContext?.Request;
        request?.EnableBuffering();
    
        var email = context1.User.FindFirst(ClaimTypes.Email)?.Value;
        Console.WriteLine(email);
        
        if (email == null)
        {
            return; 
        }
        var userId = context.Users.FirstOrDefault(user => user.Email == email).Id;

        if (userId == null) return;
        
        
        
        // Extract fileId from the body (JSON data)
        string fileIdFromBody = null;
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.ContainsKey("id") == true)
            {
                fileIdFromBody = jsonObject["id"];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
        Console.WriteLine("FileId from body: " + fileIdFromBody);

        
        
        //CHECKS if user has editor role for file
        var userAccess = context.UserFileAccesses.FirstOrDefault(db =>
            db.User.Id == userId && db.File.Id == fileIdFromBody);

        //TODO REMOVE HARDCODED editor
        if (userAccess?.Role == "editor")
        {
            Console.WriteLine(userAccess.Role);

            context1.Succeed(requirement); 
            request.Body.Position = 0;

        }
     
        // Deny access if neither condition is met
        return;
    }
}