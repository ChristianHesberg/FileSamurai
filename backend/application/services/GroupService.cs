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
            Name = res.Name
        };
    }

    public GroupDto? GetGroup(string id)
    {
        var group = groupPort.GetGroup(id);
        return group == null
            ? null
            : new GroupDto()
            {
                Id = group.Id,
                Name = group.Name,
                GroupEmail = group.CreatorEmail
            };
    }

    public bool AddUserToGroup(AddUserToGroupDto toGroupDto)
    {
        return groupPort.AddUserToGroup(toGroupDto.UserEmail, toGroupDto.GroupId);
    }
}