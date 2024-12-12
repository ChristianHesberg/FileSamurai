using core.models;

namespace application.ports;

public interface IUserPort
{
    public User AddUser(User user);
    public User? GetUser(string id);
    public User? GetUserByEmail(string email);
}