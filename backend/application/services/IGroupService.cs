using application.dtos;
using core.models;

namespace application.services;

public interface IGroupService
{
    public GroupDto AddGroup(GroupCreationDto group);
    public GroupDto? GetGroup(string id);
}