using core.models;
using core.services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("key-pair/{id}")]  
    public ActionResult<UserRsaKeyPair> Get(string id)  
    {  
        var userRsaKeyPair = userService.GetUserRsaKeyPair(id);  
  
        if (userRsaKeyPair == null)  
        {  
            return NotFound();  
        }  
  
        return Ok(userRsaKeyPair);  
    }
}