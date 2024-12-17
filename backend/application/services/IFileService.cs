using application.dtos;
using core.models;
using File = core.models.File;

namespace application.services;

public interface IFileService
{
    public PostFileResultDto AddFile(AddFileDto file);
    public GetFileDto? GetFile(string fileId, string userId);
    public bool UpdateFile(FileDto orGetFile);
    public void AddUserFileAccess(AddOrGetUserFileAccessDto orGetUserFileAccess);
    public AddOrGetUserFileAccessDto? GetUserFileAccess(string fileId, string userId);
}