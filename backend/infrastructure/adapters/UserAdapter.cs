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

    public User? GetUser(string id)
    {
        return context.Users.Find(id);
    }

    public User? GetUserByEmail(string email)
    {
        return context.Users.FirstOrDefault(user => user.Email == email);
    }

    public List<Group>? GetGroupsForUser(string id)
    {
        var user = context.Users.Include(user => user.Groups).FirstOrDefault(e => e.Id == id);
        return user?.Groups;
    }
}