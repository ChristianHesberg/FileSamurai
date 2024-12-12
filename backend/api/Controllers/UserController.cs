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

    [HttpGet("email/{id}")]
    public ActionResult<UserDto> GetUserByEmail(string email)
    {
        var user = userService.GetUserByEmail(email);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public ActionResult<string> PostUser(UserCreationDto user)
    {
        var res = userService.AddUser(user);
        //TODO create jwt from res
        return Ok();
    }
    
}