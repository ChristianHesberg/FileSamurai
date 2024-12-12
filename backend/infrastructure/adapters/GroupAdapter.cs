using application.ports;
using core.models;

namespace infrastructure.adapters;

public class GroupAdapter(Context context) : IGroupPort
{
    public Group AddGroup(Group group)
    {
        var added =context.Groups.Add(group);
        context.SaveChanges();
        return added.Entity;
    }

    public Group? GetGroup(string id)
    {
        return context.Groups.Find(id);
    }
}