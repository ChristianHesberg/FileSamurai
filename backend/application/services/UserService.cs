using application.dtos;
using application.ports;
using core.models;

namespace application.services;

public class UserService(IUserPort userPort) : IUserService
{
    public UserDto AddUser(UserCreationDto user)
    {
        var converted = new User()
        {
            Email = user.Email
        };
        var res = userPort.AddUser(converted);
        return new UserDto()
        {
            Id = res.Id,
            Email = res.Email
        };
    }

    public UserDto? GetUser(string id)
    {
        var user = userPort.GetUser(id);
        return user == null
            ? null
            : new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };
    }

    public UserDto? GetUserByEmail(string email)
    {
        var user = userPort.GetUserByEmail(email);
        return user == null
            ? null
            : new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };
    }

    public List<GroupDto>? GetGroupsForUser(string id)
    {
        var groups = userPort.GetGroupsForUser(id);
        if (groups == null) return null;
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
}