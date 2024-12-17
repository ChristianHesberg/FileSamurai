﻿using application.dtos;
using application.ports;
using core.models;
using File = core.models.File;

namespace application.services;

public class FileService(IFilePort filePort): IFileService
{
    public void AddFile(AddFileDto file)
    {
        var converted = new File()
        {
            Title = file.Title,
            FileContents = file.FileContents,
            Nonce = file.Nonce,
            Tag = file.Tag,
            GroupId = file.GroupId
        };
        filePort.AddFile(converted);
    }

    public (UpdateOrGetFileDto, AddOrGetUserFileAccessDto)? GetFile(string fileId, string userId)
    {
        var file = filePort.GetFile(fileId);
        var accessObject = filePort.GetUserFileAccess(userId, fileId);
        
        if (file == null || accessObject == null) return null;

        var convertedFile = new UpdateOrGetFileDto()
        {
            Id = file.Id,
            Title = file.Title,
            FileContents = file.FileContents
        };
        var convertedAccessObject = new AddOrGetUserFileAccessDto()
        {
            FileId = accessObject.FileId,
            UserId = accessObject.UserId,
            EncryptedFileKey = accessObject.EncryptedFileKey,
            Role = accessObject.Role
        };
        return (convertedFile, convertedAccessObject);
    }

    public bool UpdateFile(UpdateOrGetFileDto file)
    {
        var converted = new File()
        {
            Id = file.Id,
            Title = file.Title,
            FileContents = file.FileContents
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