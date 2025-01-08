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
    public ActionResult<UserDto> PostUser(UserCreationDto dto)
    {
        var authHeaders = Request.Headers.Authorization.ToString();
        var email = JwtDecoder.DecodeJwtEmail(authHeaders);

        if (dto.Email != email) return Unauthorized();
        
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

    [HttpDelete("{id}")]
    [Authorize(Policy = "OwnsResourcePolicy")]
    public ActionResult DeleteUser( string id)
    {
        userService.DeleteUser(id);
        return Ok();
    }
}