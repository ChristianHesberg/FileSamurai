using application.dtos;
using application.services;
using core.models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class KeyPairController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]  
    public ActionResult<UserRsaPrivateKeyDto> GetPrivateKey(string id)  
    {  
        var userRsaKeyPair = userService.GetUserPrivateKey(id);  
  
        if (userRsaKeyPair == null)  
        {  
            return NotFound();  
        }  
        return Ok(userRsaKeyPair);  
    }
    
    [HttpGet("public/{id}")]  
    public ActionResult<string> GetPublicKey(string id)  
    {  
        var publicKey = userService.GetUserPublicKey(id);  
  
        if (publicKey == null)  
        {  
            return NotFound();  
        }  
        return Ok(publicKey);  
    }

    [HttpPost]
    public ActionResult Post(UserRsaKeyPairDto keyPair)
    {
        userService.AddUserRsaKeyPair(keyPair);
        return Ok();  
    }  
}