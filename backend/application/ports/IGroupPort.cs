using application.dtos;
using core.models;

namespace application.ports;

public interface IGroupPort
{
    public Group AddGroup(Group group);
    public Group? GetGroup(string id);
    public User AddUserToGroup(string userEmail, string groupId);
    public List<Group> GetGroupsForEmail(string email);
    public List<User> GetUsersInGroup(string groupId);
    void RemoveUserFromGroup(string groupId, string userId);
}