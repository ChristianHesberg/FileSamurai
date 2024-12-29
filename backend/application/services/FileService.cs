using application.dtos;
using application.ports;
using application.transformers;
using core.models;
using File = core.models.File;

namespace application.services;

public class FileService(IFilePort filePort): IFileService
{
    public PostFileResultDto AddFile(AddFileDto file)
    {
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

    public GetFileDto? GetFile(string fileId, string userId)
    {
        var file = filePort.GetFile(fileId);
        var accessObject = filePort.GetUserFileAccess(userId, fileId);
        
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
        var converted = new UserFileAccess()
        {
            UserId = userFileAccess.UserId,
            FileId = userFileAccess.FileId,
            EncryptedFileKey = userFileAccess.EncryptedFileKey,
            Role = userFileAccess.Role
        };
        filePort.AddUserFileAccess(converted);
    }

    public AddOrGetUserFileAccessDto? GetUserFileAccess(string userId, string fileId)
    {
        var res = filePort.GetUserFileAccess(userId, fileId);
        return res == null ? null : new AddOrGetUserFileAccessDto()
        {
            UserId = res.UserId,
            FileId = res.FileId,
            EncryptedFileKey = res.EncryptedFileKey,
            Role = res.Role
        };
    }
}