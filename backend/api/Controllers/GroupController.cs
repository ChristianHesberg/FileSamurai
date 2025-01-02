using application.dtos;
using application.services;
using application.transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController(IGroupService groupService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public ActionResult<GroupDto> PostGroup(GroupCreationDto group)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return Unauthorized("Authorization header is missing");
        }

        var email =  JwtDecoder.DecodeJwtEmail(authHeader.ToString());
       
        var res = groupService.AddGroup(group, email);
        return Ok(res);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<GroupDto> GetGroup(string id)
    {
        var group = groupService.GetGroup(id);
        return group == null ? NotFound() : Ok(group);
    }

    [HttpPost("addUser")] 
    [Authorize (Policy = "GroupAddUser") ]
    public ActionResult<bool> AddUserToGroup(AddUserToGroupDto dto)
    {
        var res = groupService.AddUserToGroup(dto);
        return Ok(res);
    }

    
}