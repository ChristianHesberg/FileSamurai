using application.dtos;
using application.services;
using core.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class KeyPairController(IUserKeyPairService userKeyPairService) : ControllerBase
{
    [HttpGet("private/{id}")]  
    public ActionResult<UserRsaPrivateKeyDto> GetPrivateKey(string id)  
    {  
        var userRsaKeyPair = userKeyPairService.GetUserPrivateKey(id);  
  
        if (userRsaKeyPair == null)  
        {  
            return NotFound();  
        }  
        return Ok(userRsaKeyPair);  
    }
    
    [HttpGet("public/{id}")]  
    [Authorize]
    public ActionResult<string> GetPublicKey(string id)  
    {  
        var publicKey = userKeyPairService.GetUserPublicKey(id);  
  
        if (publicKey == null)  
        {  
            return NotFound();  
        }  
        return Ok(publicKey);  
    }

    [HttpPost]
    [Authorize]
    public ActionResult Post(UserRsaKeyPairDto keyPair)
    {
        userKeyPairService.AddUserRsaKeyPair(keyPair);
        return Ok();  
    }  
}