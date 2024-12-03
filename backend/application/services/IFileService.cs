using application.dtos;
using core.models;
using File = core.models.File;

namespace application.services;

public interface IFileService
{
    public void AddFile(AddFileDto file);
    public UpdateOrGetFileDto? GetFile(string fileId);
    public bool UpdateFile(UpdateOrGetFileDto orGetFile);
    public void AddUserFileAccess(AddOrGetUserFileAccessDto orGetUserFileAccess);
    public AddOrGetUserFileAccessDto? GetUserFileAccess(string userId, string fileId);
}