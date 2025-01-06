using core.models;

namespace application.ports;

public interface IGroupPort
{
    public Group AddGroup(Group group);
    public Group GetGroup(string id);
    public bool AddUserToGroup(string userEmail, string groupId);
    void DeleteGroup(string id);
}