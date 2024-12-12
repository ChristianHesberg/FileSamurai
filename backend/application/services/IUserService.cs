using application.dtos;
using core.models;

namespace application.services;

public interface IUserService
{
    public UserDto AddUser(UserCreationDto user);
    public UserDto? GetUser(string id);
    public UserDto? GetUserByEmail(string email);
}