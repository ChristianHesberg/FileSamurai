using System.Data;
using application.dtos;
using application.ports;
using core.errors;
using core.models;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.adapters;

public class GroupAdapter(Context context) : IGroupPort
{
    public Group AddGroup(Group group)
    {
        try
        {
            var added = context.Groups.Add(group);
            context.SaveChanges();
            return added.Entity;
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }

    public Group GetGroup(string id)
    {
        var res = context.Groups.Find(id);
        if (res == null) throw new KeyNotFoundException($"Could not find group with id: {id}");
        return res;
    }

    public User AddUserToGroup(string userEmail, string groupId)
    {
        var user = context.Users.FirstOrDefault(user => user.Email == userEmail);
        if (user == null) throw new KeyNotFoundException("no found user");
        var group = context.Groups.Include(g => g.Users).FirstOrDefault(group => group.Id == groupId);
        if (group == null) throw new KeyNotFoundException("no group found");
        if (group.Users.Any(u => u.Id == user.Id))
            throw new EntityAlreadyExistsException("user id already in the given group");
        try
        {
            group.Users.Add(user);
            context.SaveChanges();
            return user;
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
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
        if (group == null) throw new KeyNotFoundException();

        var user = group.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) throw new KeyNotFoundException();
        try
        {
            group.Users.Remove(user);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }

    public void DeleteGroup(string id)
    {
        var group = context.Groups.Find(id);
        if (group == null) throw new KeyNotFoundException();
        try
        {
            context.Groups.Remove(group);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }
}