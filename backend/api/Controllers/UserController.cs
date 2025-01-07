using application.dtos;
using application.services;
using application.transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public ActionResult<UserDto> PostUser(UserCreationDto user)
    {
        var res = userService.AddUser(user);
        return Ok(res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<UserDto> GetUser(string id)
    {
        var user = userService.GetUser(id);
        return user == null ? NotFound() : Ok(user);
    }


    [HttpGet("email/{email}")]
    public ActionResult<UserDto> GetUserByEmail(string email)
    {
        var user = userService.GetUserByEmail(email);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet("groups/{id}")]
    [Authorize]
    public ActionResult<List<GroupDto>> GetGroupsForUser(string id)
    {
        var groups = userService.GetGroupsForUser(id);
        return groups == null ? NotFound() : Ok(groups);
    }

    [HttpGet("getUserIfNullRegister/{userEmail}")]
    public ActionResult<UserDto> GetUserIfNullRegister(string userEmail)
    {
        //TODO GET MAIL FROM AUTH HEADER?
        var user = userService.GetUserByEmail(userEmail)
                   ?? userService.AddUser(new UserCreationDto() { Email = userEmail });
        return Ok(user);
    }

    [HttpPost("createUser")]
    public ActionResult<UserDto> CreateUser(UserCreationDto creationDto)
    {
        var userDto = userService.AddUser(creationDto);
        return Ok(userDto);
    }

    [HttpGet("validatePassword")]
    public ActionResult<bool> ValidatePassword(string password)
    {
        var authHeaders = Request.Headers.Authorization.ToString();
        var email = JwtDecoder.DecodeJwtEmail(authHeaders);
        var result = userService.ValidatePassword(password, email);
        if (result) return Ok(result);
        return Unauthorized();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser( string id)
    {
        userService.DeleteUser(id);
        return Ok();
    }
}