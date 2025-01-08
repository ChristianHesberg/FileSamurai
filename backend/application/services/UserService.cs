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
    public UserDto AddUser(UserCreationDto user)
    {
        var validationResult = userCreationDtoValidator.Validate(user);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
        var converted = new User()
        {
            Email = user.Email,
            HashedPassword = user.HashedPassword,
            Salt = user.Salt
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
    
    
    public void DeleteUser(string id)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(id);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
       userPort.DeleteUser(id);
    }
}