using application.dtos;
using application.services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
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
}