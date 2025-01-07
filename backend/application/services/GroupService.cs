using application.dtos;
using application.ports;
using application.validation;
using core.models;
using FluentValidation;

namespace application.services;

public class GroupService(
    IGroupPort groupPort,
    IValidator<GroupCreationDto> groupCreationValidator,
    IValidator<AddUserToGroupDto> addUserToGroupValidator,
    IEnumerable<IValidator<string>> stringValidators
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
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);
        var validationResult = guidValidator.Validate(id);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var group = groupPort.GetGroup(id);
        return new GroupDto()
        {
            Id = group.Id,
            Name = group.Name,
            GroupEmail = group.CreatorEmail
        };
    }

    public List<GroupDto> GetGroupsForEmail(string email)
    {
        var guidValidator = ValidationUtilities.GetValidator<EmailValidator>(stringValidators);
        var validationResult = guidValidator.Validate(email);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var groups = groupPort.GetGroupsForEmail(email);
        return groups.Select(g => new GroupDto() { Name = g.Name, GroupEmail = g.CreatorEmail, Id = g.Id }).ToList();
    }

    public List<UserDto> GetUsersInGroup(string groupId)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);
        var validationResult = guidValidator.Validate(groupId);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var users = groupPort.GetUsersInGroup(groupId);
        return users.Select(u => new UserDto() { Id = u.Id, Email = u.Email }).ToList();
    }

    public void RemoveUserFromGroup(string groupId, string userId)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);
        var validationResultGroup = guidValidator.Validate(groupId);
        var validationResultUser = guidValidator.Validate(userId);
        ValidationUtilities.ThrowIfInvalid(validationResultGroup);
        ValidationUtilities.ThrowIfInvalid(validationResultUser);

        groupPort.RemoveUserFromGroup(groupId, userId);
    }

    public UserDto AddUserToGroup(AddUserToGroupDto dto)
    {
        var validationResult = addUserToGroupValidator.Validate(dto);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var user = groupPort.AddUserToGroup(dto.UserEmail, dto.GroupId);
        return new UserDto()
        {
            Email = user.Email,
            Id = user.Id
        };
    }
    
    public void DeleteGroup(string id)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);
        var validationResult = guidValidator.Validate(id);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        groupPort.DeleteGroup(id);
    }
}