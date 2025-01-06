using application.dtos;
using application.ports;
using core.models;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.adapters;

public class UserAdapter(Context context) : IUserPort
{
    public User AddUser(User user)
    {
        var added =context.Users.Add(user);
        context.SaveChanges();
        return added.Entity;
    }

    public User GetUser(string id)
    {
        var res = context.Users.Find(id);
        if (res == null) throw new KeyNotFoundException($"Could not find user with id: {id}");
        return res;
    }

    public User GetUserByEmail(string email)
    {
        var res = context.Users.FirstOrDefault(user => user.Email == email);
        if (res == null) throw new KeyNotFoundException($"Could not find user with email: {email}");
        return res;
    }

    public List<Group> GetGroupsForUser(string id)
    {
        var user = context.Users.Include(user => user.Groups).FirstOrDefault(e => e.Id == id);
        if (user == null) throw new KeyNotFoundException($"Could not find user with id: {id}");
        return user.Groups ?? [];
    }

    public void DeleteUser(string id)
    {
        var user = context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        context.Users.Remove(user);
        context.SaveChanges();
    }
}