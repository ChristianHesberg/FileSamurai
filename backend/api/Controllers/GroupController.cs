using System.Data;
using application.dtos;
using application.services;
using application.transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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

        var email = JwtDecoder.DecodeJwtEmail(authHeader.ToString());

        var res = groupService.AddGroup(group, email);

        groupService.AddUserToGroup(new AddUserToGroupDto() { GroupId = res.Id, UserEmail = email });

        return Ok(res);
    }
    
    [HttpGet("{groupId}")]
    [Authorize(Policy = "GroupGet")]
    public ActionResult<GroupDto> GetGroup(string groupId)
    {
        var group = groupService.GetGroup(groupId);
        return Ok(group);
    }

    [HttpPost("users")]
    [Authorize(Policy = "GroupAddUser")]
    public ActionResult<UserDto> AddUserToGroup(AddUserToGroupDto dto)
    {
            var res = groupService.AddUserToGroup(dto);
            return Ok(res);
    }
    
    [HttpGet("groupsForEmail")]
    [Authorize]
    public ActionResult<List<GroupDto>> GetGroupsForEmail()
    {
        var auth = Request.Headers.Authorization;
        var email = JwtDecoder.DecodeJwtEmail(auth.ToString());

        return Ok(groupService.GetGroupsForEmail(email));
    }

    [HttpGet("users/{groupId}")]
    [Authorize(Policy = "GroupGet")]
    public ActionResult<List<UserDto>> GetUsersInGroup(string groupId)
    {
        var users = groupService.GetUsersInGroup(groupId);
        return Ok(users);
    }

    [HttpDelete("users")]
    [Authorize(Policy = "GroupOwnerPolicy")]
    public ActionResult RemoveUserFromGroup([FromQuery] string groupId, [FromQuery] string userId)
    {
            groupService.RemoveUserFromGroup(groupId, userId);
            return Ok();
    }

    [HttpDelete("{groupId}")]
    [Authorize(Policy = "GroupOwnerPolicy")]
    public ActionResult DeleteGroup(string groupId)
    {
        groupService.DeleteGroup(groupId);
        return Ok();
    }
}