﻿using application.dtos;
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
    [Authorize(Policy = "GetUserPK")]
    public ActionResult<UserRsaPrivateKeyDto> GetPrivateKey(string id)  
    {  
        var userRsaKeyPair = userKeyPairService.GetUserPrivateKey(id);  
        return Ok(userRsaKeyPair);  
    }
    
    [HttpGet("public/{id}")]  
    [Authorize] 
    public ActionResult<string> GetPublicKey(string id)  
    {  
        var publicKey = userKeyPairService.GetUserPublicKey(id);  
        return Ok(publicKey);  
    }

    [HttpPost]
    [Authorize(Policy = "PostRSAKeyPair")]
    public ActionResult Post(UserRsaKeyPairDto keyPair)
    {
        userKeyPairService.AddUserRsaKeyPair(keyPair);
        return Ok();  
    }  
}