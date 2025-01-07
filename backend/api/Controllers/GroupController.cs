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
    
    //todo add policy
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<GroupDto> GetGroup(string id)
    {
        var group = groupService.GetGroup(id);
        return Ok(group);
    }

    [HttpPost("addUser")]
    [Authorize(Policy = "GroupAddUser")]
    public ActionResult<UserDto> AddUserToGroup(AddUserToGroupDto dto)
    {
            var res = groupService.AddUserToGroup(dto);
            return Ok(res);
    }
    
    //todo missing auth
    [HttpGet("groupsForEmail")]
    public ActionResult<List<GroupDto>> GetGroupsForEmail()
    {
        var auth = Request.Headers.Authorization;
        var email = JwtDecoder.DecodeJwtEmail(auth.ToString());

        return Ok(groupService.GetGroupsForEmail(email));
    }

    //todo missing auth
    [HttpGet("usersInGroup")]
    public ActionResult<List<UserDto>> GetUsersInGroup(string groupId)
    {
        var users = groupService.GetUsersInGroup(groupId);
        return Ok(users);
    }

    //todo missing auth
    [HttpDelete("removeUserFromGroup")]
    public ActionResult RemoveUserFromGroup(string groupId, string userId)
    {
            groupService.RemoveUserFromGroup(groupId, userId);
            return Ok();
    }

    //todo auth
    [HttpDelete("{id}")]
    public ActionResult DeleteGroup(string id)
    {
        groupService.DeleteGroup(id);
        return Ok();
    }
}