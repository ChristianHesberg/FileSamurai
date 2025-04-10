﻿using System.Security.Claims;
using application.dtos;
using application.ports;
using application.services;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class DocumentGetUserFileAccessHandler(IUserService userService, IHttpContextAccessor contextAccessor) : AuthorizationHandler<Requirements.DocumentGetUserFileAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext,
        Requirements.DocumentGetUserFileAccessRequirement requirement)
    {
        var accessor = contextAccessor.HttpContext;
        if (accessor == null) throw new Exception("Http context is somehow null");
        
        var request = accessor.Request;
        
        var email = authorizationHandlerContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return;

        try
        {
            var user = userService.GetUserByEmail(email);

            // Extract userId from the Query
            var userId = request.Query["userId"].ToString();
            if (string.IsNullOrEmpty(userId)) throw new BadHttpRequestException("userId query parameter required");

            if (userId == user.Id)
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