using application.dtos;
using application.ports;
using application.validation;
using core.models;
using FluentValidation;

namespace application.services;

public class GroupService(
    IGroupPort groupPort,
    IValidator<GroupCreationDto> groupCreationValidator
    ) : IGroupService
{
    public GroupDto AddGroup(GroupCreationDto group, string email)
    {
        var validationResult = groupCreationValidator.Validate(group);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
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

    public bool AddUserToGroup(AddUserToGroupDto toGroupDto)
    {
        return groupPort.AddUserToGroup(toGroupDto.UserEmail, toGroupDto.GroupId);
    }

    public void DeleteGroup(string id)
    {
        groupPort.DeleteGroup(id);
    }
}