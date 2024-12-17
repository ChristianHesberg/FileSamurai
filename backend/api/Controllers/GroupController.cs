using application.dtos;
using application.services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController(IGroupService groupService) : ControllerBase
{
    [HttpPost]
    public ActionResult<GroupDto> PostGroup(GroupCreationDto group)
    {
        var res = groupService.AddGroup(group);
        return Ok(res);
    }
    
    [HttpGet("{id}")]
    public ActionResult<GroupDto> GetGroup(string id)
    {
        var group = groupService.GetGroup(id);
        return group == null ? NotFound() : Ok(group);
    }

    [HttpPost("addUser")]
    public ActionResult<bool> AddUserToGroup(AddUserToGroupDto dto)
    {
        var res = groupService.AddUserToGroup(dto);
        return Ok(res);
    }
}