using System.Text.Json;
using application.dtos;
using application.ports;
using application.transformers;
using core.models;

namespace application.services;

public class UserService(IUserPort userPort) : IUserService
{
    public UserDto AddUser(UserCreationDto user)
    {
        var salt = PasswordHasher.GenerateSalt();
        Console.WriteLine(JsonSerializer.Serialize(user));
        var hashedPw = PasswordHasher.HashPassword(user.Password, salt);

        var converted = new User()
        {
            Email = user.Email,
            HashedPassword = hashedPw,
            Salt = salt
        };
        var res = userPort.AddUser(converted);
        return new UserDto()
        {
            Id = res.Id,
            Email = res.Email
        };
    }

    public UserDto GetUser(string id)
    {
        var user = userPort.GetUser(id);
        return new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };
    }

    public UserDto GetUserByEmail(string email)
    {
        var user = userPort.GetUserByEmail(email);
        return new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };
    }

    public List<GroupDto> GetGroupsForUser(string id)
    {
        var groups = userPort.GetGroupsForUser(id);

        var list = new List<GroupDto>();
        foreach (var group in groups)
        {
            list.Add(new GroupDto()
            {
                Id = group.Id,
                Name = group.Name
            });
        }

        return list;
    }

    public bool ValidatePassword(string password, string email)
    {
        var user = userPort.GetUserByEmail(email);
        return PasswordHasher.VerifyPassword(password, user.HashedPassword, user.Salt);
    }
    
    public void DeleteUser(string id)
    {
       userPort.DeleteUser(id);
    }
}