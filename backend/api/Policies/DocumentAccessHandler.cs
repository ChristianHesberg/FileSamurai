﻿using System.Security.Claims;
using System.Text.Json;
using core.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using infrastructure;

namespace api.Policies;

public class DocumentAccessHandler(Context context, IHttpContextAccessor contextAccessor) : AuthorizationHandler<DocumentAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        DocumentAccessRequirement requirement)
    {
        var request = contextAccessor.HttpContext.Request;
        request.EnableBuffering();
        
        var email = context1.User.FindFirst(ClaimTypes.Email)?.Value;
        Console.WriteLine(email);

        
        if (email == null)
        {
            return; 
        }
        var user = context.Users.Include(u => u.Groups).FirstOrDefault(user => user.Email == email);

        if (user == null) return;
        
        var userId = user.Id;
        
        // Extract fileId from the body (JSON data)
        string fileIdFromBody = null;
        try
        {
            // Read the body and deserialize it into a dictionary (or model)
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (jsonObject?.ContainsKey("fileId") == true)
            {
                fileIdFromBody = jsonObject["fileId"];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading body: " + ex.Message);
        }
        Console.WriteLine("FileId from body: " + fileIdFromBody);

        
        //CHECK IF USER IS IN TABLE FOR DOCUMENT ACCESS
        var file = await context.Files.FindAsync(fileIdFromBody);
        
        if (file == null) return;
        

        
        var hasAccess = await context.UserFileAccesses.AnyAsync(db =>
            db.User.Id == userId && db.File.Id == fileIdFromBody);

        
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