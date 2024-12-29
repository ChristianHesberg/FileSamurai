using application.dtos;
using application.ports;
using application.validation;
using core.models;
using FluentValidation;
using FluentValidation.Results;
using File = core.models.File;

namespace application.services;

public class FileService(
    IFilePort filePort,
    IValidator<AddFileDto> addFileDtoValidator,
    IValidator<GetFileOrAccessInputDto> getFileOrAccessInputDtoValidator,
    IValidator<FileDto> fileDtoValidator,
    IValidator<AddOrGetUserFileAccessDto> addOrGetUserFileAccessDtoValidator
    ): IFileService
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
            Tag = file.Tag,
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

    public GetFileDto? GetFile(GetFileOrAccessInputDto dto)
    {
        ValidationResult validationResult = getFileOrAccessInputDtoValidator.Validate(dto);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
        var file = filePort.GetFile(dto.FileId);
        var accessObject = filePort.GetUserFileAccess(dto.UserId, dto.FileId);
        
        if (file == null || accessObject == null) return null;

        var convertedFile = new FileDto()
        {
            Id = file.Id,
            Title = file.Title,
            FileContents = file.FileContents,
            Nonce = file.Nonce,
            Tag = file.Tag,
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
        ValidationResult validationResult = fileDtoValidator.Validate(file);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
        var converted = new File()
        {
            Id = file.Id,
            Title = file.Title,
            FileContents = file.FileContents,
            Nonce = file.Nonce,
            Tag = file.Tag,
        };
        return filePort.UpdateFile(converted);
    }

    public void AddUserFileAccess(AddOrGetUserFileAccessDto userFileAccess)
    {
        ValidationResult validationResult = addOrGetUserFileAccessDtoValidator.Validate(userFileAccess);
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

    public AddOrGetUserFileAccessDto? GetUserFileAccess(GetFileOrAccessInputDto dto)
    {
        ValidationResult validationResult = getFileOrAccessInputDtoValidator.Validate(dto);
        ValidationUtilities.ThrowIfInvalid(validationResult);
        
        var res = filePort.GetUserFileAccess(dto.UserId, dto.FileId);
        return res == null ? null : new AddOrGetUserFileAccessDto()
        {
            UserId = res.UserId,
            FileId = res.FileId,
            EncryptedFileKey = res.EncryptedFileKey,
            Role = res.Role
        };
    }
}