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
    [Authorize]
    public ActionResult<UserDto> PostUser()
    {
        var authHeaders = Request.Headers.Authorization.ToString();
        var email = JwtDecoder.DecodeJwtEmail(authHeaders);

        var dto = new UserCreationDto
        {
            Email = email
        };
        
        var res = userService.AddUser(dto);
        return Ok(res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<UserDto> GetUser(string id)
    {
        var user = userService.GetUser(id);
        return Ok(user);
    }


    [HttpGet("email/{email}")]
    [Authorize]
    public ActionResult<UserDto> GetUserByEmail(string email)
    {
        var user = userService.GetUserByEmail(email);
        return Ok(user);
    }

    [HttpGet("groups/{id}")]
    [Authorize(Policy = "OwnsResourcePolicy")]
    public ActionResult<List<GroupDto>> GetGroupsForUser(string id)
    {
        var groups = userService.GetGroupsForUser(id);
        return Ok(groups);
    }

    [HttpGet("getUserIfNullRegister/{userEmail}")]
    [Authorize]
    public ActionResult<UserDto> GetUserIfNullRegister(string userEmail)
    {
        //TODO GET MAIL FROM AUTH HEADER?
        //TODO CHECK WITH VICTOR IS THIS NEEDED
        var user = userService.GetUserByEmail(userEmail)
                   ?? userService.AddUser(new UserCreationDto() { Email = userEmail });
        return Ok(user);
    }

    [HttpPost("createUser")]//TODO delete this and refactor
    public ActionResult<UserDto> CreateUser(UserCreationDto creationDto)
    {
        var userDto = userService.AddUser(creationDto);
        return Ok(userDto);
    }

    [HttpGet("validatePassword")] //TODO Delete or refactor
    public ActionResult<bool> ValidatePassword(string password)
    {
        var authHeaders = Request.Headers.Authorization.ToString();
        var email = JwtDecoder.DecodeJwtEmail(authHeaders);
        var result = userService.ValidatePassword(password, email);
        if (result) return Ok(result);
        return Unauthorized();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "OwnsResourcePolicy")]
    public ActionResult DeleteUser( string id)
    {
        userService.DeleteUser(id);
        return Ok();
    }
}