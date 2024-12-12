using core.models;

namespace application.ports;

public interface IGroupPort
{
    public Group AddGroup(Group group);
    public Group? GetGroup(string id);
}