using System.Text.Json;
using application.dtos;
using application.ports;
using application.transformers;
using application.validation;
using core.models;
using FluentValidation;

namespace application.services;

public class UserService(
    IUserPort userPort,
    IValidator<UserCreationDto> userCreationDtoValidator,
    IEnumerable<IValidator<string>> stringValidators
    ) : IUserService
{
    //todo we should not be receiving a password here
    public UserDto AddUser(UserCreationDto user)
    {
        var validationResult = userCreationDtoValidator.Validate(user);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
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
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(id);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
        var user = userPort.GetUser(id);
        return new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };
    }

    public UserDto GetUserByEmail(string email)
    {
        var guidValidator = ValidationUtilities.GetValidator<EmailValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(email);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
        var user = userPort.GetUserByEmail(email);
        return new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };
    }

    public List<GroupDto> GetGroupsForUser(string id)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(id);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
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

    //todo should be removed
    public bool ValidatePassword(string password, string email)
    {
        var user = userPort.GetUserByEmail(email);
        return PasswordHasher.VerifyPassword(password, user.HashedPassword, user.Salt);
    }
    
    public void DeleteUser(string id)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(id);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
       userPort.DeleteUser(id);
    }
}