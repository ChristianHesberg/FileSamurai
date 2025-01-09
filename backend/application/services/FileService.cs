using application.dtos;
using application.ports;
using application.validation;
using core.models;
using FluentValidation;
using FluentValidation.Results;
using File = core.models.File;
using Group = core.models.Group;

namespace application.services;

public class FileService(
    IFilePort filePort,
    IValidator<AddFileDto> addFileDtoValidator,
    IValidator<GetFileOrAccessInputDto> getFileOrAccessInputDtoValidator,
    IValidator<FileDto> fileDtoValidator,
    IValidator<AddOrGetUserFileAccessDto> addOrGetUserFileAccessDtoValidator,
    IEnumerable<IValidator<string>> stringValidators ) : IFileService
{
    public PostFileResultDto AddFile(AddFileDto file)
    {
        ValidationResult validationResult = addFileDtoValidator.Validate(file);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var converted = new File()
        {
            Title = file.Title,
            FileContents = file.FileContents,
            Nonce = file.Nonce,
            GroupId = file.GroupId
        };
        var result = filePort.AddFile(converted);

        var dto = new PostFileResultDto()
        {
            Id = result.Id,
            Title = result.Title
        };
        return dto;
    }

    public GetFileDto GetFile(GetFileOrAccessInputDto dto)
    {
        var validationResult = getFileOrAccessInputDtoValidator.Validate(dto);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var file = filePort.GetFile(dto.FileId);
        var accessObject = filePort.GetUserFileAccess(dto.UserId, dto.FileId);

        var convertedFile = new FileDto()
        {
            Id = file.Id,
            Title = file.Title,
            FileContents = file.FileContents,
            Nonce = file.Nonce,
            GroupId = file.GroupId
        };
        
        var convertedAccessObject = new AddOrGetUserFileAccessDto()
        {
            FileId = accessObject.FileId,
            UserId = accessObject.UserId,
            EncryptedFileKey = accessObject.EncryptedFileKey,
            Role = accessObject.Role
        };
        
        return new GetFileDto()
        {
            File = convertedFile,
            UserFileAccess = convertedAccessObject
        };
    }


    public bool UpdateFile(FileDto file)
    {
        var validationResult = fileDtoValidator.Validate(file);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var converted = new File()
        {
            Id = file.Id,
            Title = file.Title,
            FileContents = file.FileContents,
            Nonce = file.Nonce
        };
        
        return filePort.UpdateFile(converted);
    }

    public void AddUserFileAccess(AddOrGetUserFileAccessDto userFileAccess)
    {
        var validationResult = addOrGetUserFileAccessDtoValidator.Validate(userFileAccess);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var converted = new UserFileAccess()
        {
            UserId = userFileAccess.UserId,
            FileId = userFileAccess.FileId,
            EncryptedFileKey = userFileAccess.EncryptedFileKey,
            Role = userFileAccess.Role
        };
        
        filePort.AddUserFileAccess(converted);
    }

    public void AddUserFileAccesses(List<AddOrGetUserFileAccessDto> userFileAccessDtos)
    {
        foreach (var access in userFileAccessDtos)
        {
            var validationResult = addOrGetUserFileAccessDtoValidator.Validate(access);
            ValidationUtilities.ThrowIfInvalid(validationResult);
        }

        var accesses = userFileAccessDtos.Select(userFileAccess => new UserFileAccess()
        {
            UserId = userFileAccess.UserId,
            FileId = userFileAccess.FileId,
            EncryptedFileKey = userFileAccess.EncryptedFileKey,
            Role = userFileAccess.Role
        }).ToList();
        
        
        filePort.AddUserFileAccesses(accesses);
    }

    public AddOrGetUserFileAccessDto GetUserFileAccess(GetFileOrAccessInputDto dto)
    {
        var validationResult = getFileOrAccessInputDtoValidator.Validate(dto);
        ValidationUtilities.ThrowIfInvalid(validationResult);

        var res = filePort.GetUserFileAccess(dto.UserId, dto.FileId);
        return new AddOrGetUserFileAccessDto()
            {
                UserId = res.UserId,
                FileId = res.FileId,
                EncryptedFileKey = res.EncryptedFileKey,
                Role = res.Role
            };
    }

    public void DeleteUserFileAccess(GetFileOrAccessInputDto dto)
    {
        var validationResult = getFileOrAccessInputDtoValidator.Validate(dto);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
        filePort.DeleteUserFileAccess(dto.UserId, dto.FileId);
    }

    public Group GetFileGroup(string fileId)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(fileId);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
        return filePort.GetFileGroup(fileId);
    }

    public List<UserFileAccess> GetAllUserFileAccess(string fileId)
    {
        var guidValidator = ValidationUtilities.GetValidator<GuidValidator>(stringValidators);  
        var validationResult = guidValidator.Validate(fileId);  
        ValidationUtilities.ThrowIfInvalid(validationResult); 
        
        return filePort.GetAllUserFileAccess(fileId);
    }
    
    public List<FileOptionDto> GetFileOptionDtos(string userId)
    {
        var files = filePort.GetAllFilesUserHasAccessTo(userId);
        return files.Select(x => new FileOptionDto(){Id = x.Id,Name = x.Title}).ToList();
    }

}