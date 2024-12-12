using application.ports;
using core.models;

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
    
    
}