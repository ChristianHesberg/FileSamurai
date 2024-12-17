using application.ports;
using core.models;
using Microsoft.EntityFrameworkCore;

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

    public bool AddUserToGroup(string userEmail, string groupId)
    {
        var user = context.Users.FirstOrDefault(user => user.Email == userEmail);
        if (user == null) return false;
        var group = context.Groups.Include(g => g.Users).FirstOrDefault(group => group.Id == groupId);
        if (group == null || group.Users.Any(u => u.Id == user.Id)) return false;
        group.Users.Add(user);
        context.SaveChanges();
        return true;
    }
}