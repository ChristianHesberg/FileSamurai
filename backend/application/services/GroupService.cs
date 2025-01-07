using application.dtos;
using application.ports;
using core.models;

namespace application.services;

public class GroupService(IGroupPort groupPort) : IGroupService
{
    public GroupDto AddGroup(GroupCreationDto group, string email)
    {
        var converted = new Group()
        {
            Name = group.Name,
            CreatorEmail = email
        };
        var res = groupPort.AddGroup(converted);

        return new GroupDto()
        {
            Id = res.Id,
            Name = res.Name,
            GroupEmail = res.CreatorEmail
        };
    }

    public GroupDto GetGroup(string id)
    {
        var group = groupPort.GetGroup(id);
        return new GroupDto()
            {
                Id = group.Id,
                Name = group.Name,
                GroupEmail = group.CreatorEmail
            };
    }

    public UserDto AddUserToGroup(AddUserToGroupDto toGroupDto)
    {
        var user = groupPort.AddUserToGroup(toGroupDto.UserEmail, toGroupDto.GroupId);
        return new UserDto()
        {
            Email = user.Email,
            Id = user.Id
        };
    }

    public List<GroupDto> GetGroupsForEmail(string email)
    {
        var groups = groupPort.GetGroupsForEmail(email);
        return groups.Select(g => new GroupDto() { Name = g.Name, GroupEmail = g.CreatorEmail, Id = g.Id }).ToList();
    }

    public List<UserDto> GetUsersInGroup(string groupId)
    {
        var users = groupPort.GetUsersInGroup(groupId);
        return users.Select(u => new UserDto() { Id = u.Id, Email = u.Email }).ToList();
    }

    public void RemoveUserFromGroup(string groupId, string userId)
    {
        groupPort.RemoveUserFromGroup(groupId, userId);
    }

    public void DeleteGroup(string id)
    {
        groupPort.DeleteGroup(id);
    }
}