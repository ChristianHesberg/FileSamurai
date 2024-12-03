using application.services;
using core.models;
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
    
    [HttpGet("public/{id}")]  
    public ActionResult<UserRsaKeyPair> GetPublicKey(string id)  
    {  
        var publicKey = userService.GetUserPublicKey(id);  
  
        if (publicKey == null)  
        {  
            return NotFound();  
        }  
        return Ok(publicKey);  
    }

    [HttpPost]
    public ActionResult<UserRsaKeyPair> Post(UserRsaKeyPair keyPair)
    {
        var res = userService.AddUserRsaKeyPair(keyPair);
        return Ok(res);  
    }  
}