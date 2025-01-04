using System.Data;
using application.dtos;
using application.ports;
using core.models;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.adapters;

public class GroupAdapter(Context context) : IGroupPort
{
    public Group AddGroup(Group group)
    {
        var added = context.Groups.Add(group);
        context.SaveChanges();
        return added.Entity;
    }

    public Group? GetGroup(string id)
    {
        return context.Groups.Find(id);
    }

    public User AddUserToGroup(string userEmail, string groupId)
    {
        var user = context.Users.FirstOrDefault(user => user.Email == userEmail);
        if (user == null) throw new KeyNotFoundException("no found user");
        var group = context.Groups.Include(g => g.Users).FirstOrDefault(group => group.Id == groupId);
        if (group == null) throw new KeyNotFoundException("no group found");
        if (group.Users.Any(u => u.Id == user.Id)) throw new DuplicateNameException("user id already in the given group");
        group.Users.Add(user);
        context.SaveChanges();
        return user;
    }

    public List<Group> GetGroupsForEmail(string email)
    {
        return context.Groups.Where(x => x.CreatorEmail == email).ToList();
    }

    public List<User> GetUsersInGroup(string groupId)
    {
        var group = context.Groups.Include(x => x.Users).FirstOrDefault(x => x.Id == groupId);
        return group == null ? [] : group.Users.ToList();
    }

    public void RemoveUserFromGroup(string groupId, string userId)
    {
        var group = context.Groups.Include(u => u.Users).FirstOrDefault(g => g.Id == groupId);
        var user = group?.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return;
        group?.Users.Remove(user);
        context.SaveChanges();
    }
}