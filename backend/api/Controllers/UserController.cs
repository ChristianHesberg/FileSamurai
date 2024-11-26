using core.models;
using core.services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("key-pair/{id}")]  
    public ActionResult<UserRsaKeyPair> Get(string id)  
    {  
        var userRsaKeyPair = _userService.GetUserRsaKeyPair(id);  
  
        if (userRsaKeyPair == null)  
        {  
            return NotFound();  
        }  
  
        return Ok(userRsaKeyPair);  
    }
}