﻿using application.dtos;
using core.models;

namespace application.services;

public interface IGroupService
{
    public GroupDto AddGroup(GroupCreationDto group, string email);

    public GroupDto GetGroup(string id);
    public UserDto AddUserToGroup(AddUserToGroupDto toGroupDto);
    public List<GroupDto> GetGroupsForEmail(string email);
    public List<UserDto> GetUsersInGroup(string groupId);
    public void RemoveUserFromGroup(string groupId, string userId);
    public void DeleteGroup(string id);

}