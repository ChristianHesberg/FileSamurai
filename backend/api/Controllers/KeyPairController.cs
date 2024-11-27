using core.models;
using core.services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class KeyPairController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]  
    public ActionResult<UserRsaKeyPair> Get(string id)  
    {  
        var userRsaKeyPair = userService.GetUserRsaKeyPair(id);  
  
        if (userRsaKeyPair == null)  
        {  
            return NotFound();  
        }  
        return Ok(userRsaKeyPair);  
    }

    [HttpPost]
    public ActionResult<UserRsaKeyPair> Post(string password, string id)
    {
        return Ok();  
    }  
}